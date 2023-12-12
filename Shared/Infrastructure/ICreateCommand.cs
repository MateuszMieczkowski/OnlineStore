namespace OnlineStore.Shared.Infrastructure;

public interface ICreateCommand : ICommand
{
    public int CreatedId { get; set; }
}