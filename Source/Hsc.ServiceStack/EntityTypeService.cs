using Hsc.Model.Knowledge;
using Hsc.Repository;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace Hsc.ServiceStack
{
    public class EntityTypeService : RestServiceBase<EntityType>
    {
        public IEntityTypeRepository _entityTypeRepository { get; set; }

        public override object OnGet(EntityType entityType)
        {
            return new EntityTypeResponse
            {
                EntityType = _entityTypeRepository.Get(entityType.Name)
            };
        }

        public override object OnPost(EntityType entityType)
        {
            _entityTypeRepository.Create(entityType);

            var entityTypeResponse = new EntityTypeResponse
            {
                EntityType = _entityTypeRepository.Get(entityType.Name)
            };

            return new HttpResult(entityTypeResponse)
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Headers =
                { 
                    {HttpHeaders.Location, this.RequestContext.AbsoluteUri.WithTrailingSlash() + entityTypeResponse.EntityType.Name }
                }
            };
        }

        public override object OnPut(EntityType entityType)
        {
            // not implemented
            return null;
        }

        public override object OnDelete(EntityType entityType)
        {
            _entityTypeRepository.Delete(entityType.Name);
            return null;
        }
    }
}