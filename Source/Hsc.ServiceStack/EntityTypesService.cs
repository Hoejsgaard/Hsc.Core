using Hsc.Repository;
using ServiceStack.ServiceInterface;

namespace Hsc.ServiceStack
{
    public class EntityTypesService : RestServiceBase<EntityTypes>
    {
        public  IEntityTypeRepository _entityTypeRepository { get; set; }

        public override object OnGet(EntityTypes request)
        {
            return new EntityTypesResponse
                       {
                           EntityTypes = _entityTypeRepository.GetAll()
                       };
        }
    }
}