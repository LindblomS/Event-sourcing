namespace ConsoleApp1.Domain;

class ThingRemoved : PersonDomainEvent
{
    public ThingRemoved(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}
