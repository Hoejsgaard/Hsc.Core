namespace Hsc.Repository
{
    public interface IRepository
    {
        IEntityRepository Entities { get; }
        IEntityTypeRepository EntityTypes { get; }
    }
}
