namespace ConsoleApp1.Infrastructure;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using ConsoleApp1.Domain;
using Dapper;

class EventStore : IEventStore
{
    readonly IConnectionProvider connectionProvider;

    public EventStore(IConnectionProvider connectionProvider)
    {
        this.connectionProvider = connectionProvider;
    }

    public (IEnumerable<IDomainEvent> events, int version) Load(Guid aggregateId)
    {
        const string sql = @$"
            select 
                [data] as {nameof(EventStoreDto.Data)},
                [version] as {nameof(EventStoreDto.Version)},
                [created] as {nameof(EventStoreDto.Created)},
                [name] as {nameof(EventStoreDto.Name)},
                [aggregate_id] as {nameof(EventStoreDto.AggregateId)},
                [aggregate_name] as {nameof(EventStoreDto.Aggregate)}
            from [event_store]
            where [aggregate_id] = @aggregateId
            order by [version] asc";

        using var connection = connectionProvider.GetConnection();
        var events = connection.Query<EventStoreDto>(sql, new { aggregateId });
        var domainEvents = events.Select(TransformEvent).Where(x => x != null);

        return (domainEvents, events.Last().Version);
    }

    IDomainEvent TransformEvent(EventStoreDto eventSelected)
    {
        return eventSelected.Name switch
        {
            nameof(Created) => TransformEvent<Created>(eventSelected.Data),
            nameof(NameChanged) => TransformEvent<NameChanged>(eventSelected.Data),
            nameof(ThingAdded) => TransformEvent<ThingAdded>(eventSelected.Data),
            nameof(ThingRemoved) => TransformEvent<ThingRemoved>(eventSelected.Data),
            _ => throw new NotImplementedException()
        };
    }

    IDomainEvent TransformEvent<TEvent>(string json) where TEvent : IDomainEvent
    {
        return JsonSerializer.Deserialize<TEvent>(json);
    }

    public void Save(IAggregateRoot aggregate, IEnumerable<IDomainEvent> events, string aggregateName)
    {
        const string sql = $@"
            insert into [event_store] (
                [version], 
                [aggregate_id], 
                [aggregate_name], 
                [name], 
                [data], 
                [created])
            values (
                @version, 
                @aggregateId, 
                @aggregate, 
                @name, 
                @data, 
                @created);";

        using var connection = connectionProvider.GetConnection();

        CheckConcurrency(aggregate, connection);

        var listOfEvents = events.Select(e => new
        {
            version = ++aggregate.Version,
            aggregateId = aggregate.Id,
            aggregate = aggregateName,
            name = e.GetType().Name,
            data = JsonSerializer.Serialize<object>(e),
            created = DateTime.UtcNow
        });

        connection.Execute(sql, listOfEvents);
    }

    static void CheckConcurrency(IAggregateRoot aggregate, IDbConnection connection)
    {
        if (aggregate.Version == 0)
            return;

        var latestVersion = GetLatestVersion(aggregate, connection);

        if (latestVersion != aggregate.Version)
            throw new DBConcurrencyException($"Event version was not latest. Current event version was {aggregate.Version} and latest event version is {latestVersion}");
    }

    static int GetLatestVersion(IAggregateRoot aggregate, IDbConnection connection)
    {
        const string sql = @"
            select top 1 [version] 
            from [event_store] 
            where [aggregate_id] = @aggregateId 
            order by [version] desc";

        return connection.QuerySingle<int>(sql, new { aggregateId = aggregate.Id });
    }

    class EventStoreDto
    {
        public string Data { get; set; }
        public int Version { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Name { get; set; }
        public string Aggregate { get; set; }
        public string AggregateId { get; set; }
    }
}
