namespace ConsoleApp1.Infrastructure;

using ConsoleApp1.Domain;
using System;

class PersonRepository : IRepository<Person>
{
    readonly IEventStore eventStore;

    public PersonRepository(IEventStore eventStore)
    {
        this.eventStore = eventStore;
    }

    public void Add(Person entity)
    {
        eventStore.Save(entity, entity.Events, "Person");
    }

    public Person Get(Guid id)
    {
        var result = eventStore.Load(id);
        var events = result.events;

        Person person = null;

        foreach (var domainEvent in events)
        {
            switch (domainEvent)
            {
                case Created e:
                    person = new Person(e.Id, e.Name);
                    break;

                case NameChanged e:
                    person.ChangeName(e.Name);
                    break;

                case ThingAdded e:
                    person.AddThing(e.Thing);
                    break;

                case ThingRemoved e:
                    person.RemoveThing(e.Name);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        person.ClearEvents();
        person.Version = result.version;
        return person;
    }
}
