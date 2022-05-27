namespace ConsoleApp1.Domain;
abstract class Entity
{
    readonly List<IDomainEvent> events;

    public int Version { get; set; }
    public IReadOnlyCollection<IDomainEvent> Events { get => events.AsReadOnly(); }

    public Entity()
    {
        events = new List<IDomainEvent>();
    }

    protected void AddEvent(IDomainEvent domainEvent)
    {
        events.Add(domainEvent);
    }

    public void ClearEvents()
    {
        events.Clear();
    }
}
