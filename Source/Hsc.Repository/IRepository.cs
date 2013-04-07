namespace Hsc.Repository
{
    public interface IRepository
    {
        IEntityRepository Entities { get; set; }
        IEntityTypeRepository EntityTypes { get; set; }
    }
}
