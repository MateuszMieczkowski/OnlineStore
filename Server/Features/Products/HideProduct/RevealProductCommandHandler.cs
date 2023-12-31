﻿using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Infrastructure;
using OnlineStore.Shared.Products;

namespace OnlineStore.Server.Features.Products.HideProduct;

public class RevealProductCommandHandler : ICommandHandler<RevealProduct>
{
    private readonly OnlineStoreDbContext _dbContext;

    public RevealProductCommandHandler(OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RevealProduct request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.IsHidden, false),
                cancellationToken: cancellationToken);
    }
}