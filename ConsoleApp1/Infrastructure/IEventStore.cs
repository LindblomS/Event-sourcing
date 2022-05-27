namespace ConsoleApp1.Infrastructure;

using ConsoleApp1.Domain;
using System;
using System.Collections.Generic;

interface IEventStore
{
    void Save(IAggregateRoot aggregate, IEnumerable<IDomainEvent> events, string aggregateName);
    (IEnumerable<IDomainEvent> events, int version) Load(Guid aggregateId);
}
