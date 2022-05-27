namespace ConsoleApp1.Domain;
interface IAggregateRoot
{
    Guid Id { get; }
    int Version { get; set; }
}
