using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Features.Accounts.Services;
using OnlineStore.Shared.Messages;

namespace OnlineStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly ILoggedUserService _loggedUserService;

    public MessagesController(OnlineStoreDbContext dbContext, ILoggedUserService loggedUserService)
    {
        _dbContext = dbContext;
        _loggedUserService = loggedUserService;
    }

    /// <summary>
    /// GET /messages
    /// Returns all messages. Access: authenticated users.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetAllMessages()
    {
        
        var messages = await _dbContext.Messages
            .Select(m => new MessageDto
            {
                Id = m.Id,
                AuthorId = m.AuthorId,
                Content = m.Content,
                CreatedAt = m.CreatedDate,
                UpdatedAt = m.ModifiedDate,
                AllowedEditors = m.AllowedEditors.Select(x => x.User!.Email).ToArray()
            })
            .ToListAsync();

        return Ok(messages);
    }

    /// <summary>
    /// POST /messages
    /// Creates a new message. Input: { content }. AuthorId is taken from the authenticated user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<MessageDto>> CreateMessage([FromBody] CreateMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Content is required.");
        }

        if (request.Content.Length > 2000)
        {
            return BadRequest("Content must not exceed 2000 characters.");
        }

        var userId = _loggedUserService.GetUserId();

        var message = new Entities.Message
        {
            Id = Guid.NewGuid(),
            AuthorId = userId,
            Content = request.Content,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();

        var messageDto = new MessageDto
        {
            Id = message.Id,
            AuthorId = message.AuthorId,
            Content = message.Content,
            CreatedAt = message.CreatedDate,
            UpdatedAt = message.ModifiedDate
        };

        return CreatedAtAction(nameof(GetAllMessages), new { id = message.Id }, messageDto);
    }

    /// <summary>
    /// DELETE /messages/{id}
    /// Deletes a message. Access: only the author.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteMessage([FromRoute] Guid id)
    {
        var message = await _dbContext.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound($"Message with id {id} not found.");
        }

        var currentUserId = _loggedUserService.GetUserId();
        // Check if current user is the author
        // Note: Since AuthorId is Guid but current user is int, we need a way to map them
        // For now, we'll use a simple comparison (you may need to adjust based on your actual user ID structure)
        if (!IsAuthor(message.AuthorId, currentUserId))
        {
            return Forbid();
        }

        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// PATCH /messages/{id}
    /// Updates the message content. Access: author or users listed in AllowedEditors.
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> UpdateMessage([FromRoute] Guid id, [FromBody] UpdateMessageRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Content is required.");
        }

        if (request.Content.Length > 2000)
        {
            return BadRequest("Content must not exceed 2000 characters.");
        }

        var message = await _dbContext.Messages
            .Include(m => m.AllowedEditors)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (message == null)
        {
            return NotFound($"Message with id {id} not found.");
        }

        var currentUserId = _loggedUserService.GetUserId();

        // Check if current user is the author or has permission
        if (!IsAuthor(message.AuthorId, currentUserId) && !HasEditPermission(message, currentUserId))
        {
            return Forbid();
        }

        message.Content = request.Content;
        message.ModifiedDate = DateTime.UtcNow;

        _dbContext.Messages.Update(message);
        await _dbContext.SaveChangesAsync();

        var messageDto = new MessageDto
        {
            Id = message.Id,
            AuthorId = message.AuthorId,
            Content = message.Content,
            CreatedAt = message.CreatedDate,
            UpdatedAt = message.ModifiedDate
        };

        return Ok(messageDto);
    }

    /// <summary>
    /// POST /messages/{id}/permissions/grant
    /// Grants edit permission to another user. Input: { userId }. Access: only the author.
    /// </summary>
    [HttpPost("{id:guid}/permissions/grant")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GrantPermission([FromRoute] Guid id, [FromBody] PermissionRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.UserEmail);
        if (user == null)
        {
            return NotFound($"User with e-mail {request.UserEmail} not found.");
        }
        
        var message = await _dbContext.Messages
            .Include(m => m.AllowedEditors)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (message == null)
        {
            return NotFound($"Message with id {id} not found.");
        }

        var currentUserId = _loggedUserService.GetUserId();

        // Only author can grant permissions
        if (!IsAuthor(message.AuthorId, currentUserId))
        {
            return Forbid();
        }

        // Check if permission already exists
        var existingPermission = message.AllowedEditors
            .FirstOrDefault(p => p.UserId == user.Id);

        if (existingPermission != null)
        {
            return BadRequest($"User {request.UserEmail} already has edit permission for this message.");
        }

        var permission = new Entities.MessagePermission
        {
            Id = Guid.NewGuid(),
            MessageId = id,
            UserId = user.Id
        };

        _dbContext.MessagePermissions.Add(permission);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// POST /messages/{id}/permissions/revoke
    /// Revokes edit permission from another user. Input: { userId }. Access: only the author.
    /// </summary>
    [HttpPost("{id:guid}/permissions/revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RevokePermission([FromRoute] Guid id, [FromBody] PermissionRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.UserEmail);
        if (user == null)
        {
            return NotFound($"User with e-mail {request.UserEmail} not found.");
        }
        
        var message = await _dbContext.Messages
            .Include(m => m.AllowedEditors)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (message == null)
        {
            return NotFound($"Message with id {id} not found.");
        }

        var currentUserId = _loggedUserService.GetUserId();

        // Only author can revoke permissions
        if (!IsAuthor(message.AuthorId, currentUserId))
        {
            return Forbid();
        }

        var permission = message.AllowedEditors
            .FirstOrDefault(p => p.UserId == user.Id);

        if (permission == null)
        {
            return BadRequest($"User {request.UserEmail} does not have edit permission for this message.");
        }

        _dbContext.MessagePermissions.Remove(permission);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool IsAuthor(int authorId, int currentUserId)
    {
        // Convert the int userId to Guid format for comparison
        return authorId == currentUserId;
    }

    private bool HasEditPermission(Entities.Message message, int currentUserId)
    {
        // Check if the current user has edit permission
        return message.AllowedEditors.Any(p => p.UserId == currentUserId);
    }
}

