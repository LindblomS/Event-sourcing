namespace ConsoleApp1.Domain;

class NameChanged : PersonDomainEvent
{
    public NameChanged(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
