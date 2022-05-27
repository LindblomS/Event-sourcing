namespace ConsoleApp1.Domain;
interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    void Add(TEntity entity);
    TEntity Get(Guid id);
}
