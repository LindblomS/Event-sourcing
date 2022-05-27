namespace ConsoleApp1.Domain;

class Person : Entity, IAggregateRoot
{
    Guid id;
    string name;
    readonly List<Thing> things;

    public Guid Id { get => id; }
    public string Name { get => name; }
    public IReadOnlyCollection<Thing> Things { get => things.AsReadOnly(); }

    public Person(Guid id, string name)
    {
        this.id = id;
        this.name = name;
        things = new List<Thing>();
        AddEvent(new Created(this.id, this.name));
    }

    public void ChangeName(string name)
    {
        this.name = name;
        AddEvent(new NameChanged(this.name));
    }

    public void AddThing(Thing thing)
    {
        if (things.Exists(x => x.Name == thing.Name))
            return;

        things.Add(thing);
        AddEvent(new ThingAdded(thing));
    }

    public void RemoveThing(string name)
    {
        var thing = things.SingleOrDefault(x => x.Name == name);

        if (thing is null)
            return;

        things.Remove(thing);
        AddEvent(new ThingRemoved(name));
    }
}
