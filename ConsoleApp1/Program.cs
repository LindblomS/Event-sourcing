using ConsoleApp1.Domain;
using ConsoleApp1.Infrastructure;

IEventStore eventstore = new EventStore(new SqlConnectionProvider());
IRepository<Person> repository = new PersonRepository(eventstore);

var person = new Person(Guid.NewGuid(), "Samuel");
person.AddThing(new("Glass", 7));
person.AddThing(new("Öl", 100));
person.AddThing(new("Godis", 5));
person.ChangeName("Lindblom");
person.RemoveThing("Godis");

repository.Add(person);

person = repository.Get(person.Id);
person.RemoveThing("Glass");
repository.Add(person);
person = repository.Get(person.Id);

Console.WriteLine($"Name: {person.Name}");
Console.WriteLine("Things:");
foreach (var thing in person.Things)
    Console.WriteLine($"    Name: {thing.Name}");

Console.ReadKey();


