namespace ConsoleApp1.Domain;
class Created : PersonDomainEvent
{
    public Created(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}
