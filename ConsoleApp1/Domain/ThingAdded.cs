namespace ConsoleApp1.Domain;

class ThingAdded : PersonDomainEvent
{
    public ThingAdded(Thing thing)
    {
        Thing = thing;
    }

    public Thing Thing { get; private set; }
}
