using System.Collections.Generic;
using Hsc.Model.Knowledge;
using Hsc.Repository;

namespace Hsc.SqlRepository.Meta
{
    public interface ISqlAttributeTypeRepository
    {
        AttributeType Read(int id);

        AttributeTypeCollection Read(EntityType entityType, IEntityTypeRepository entityTypeRepository);

        void Create(AttributeType attributeType, EntityType onEntity);

        List<AttributeType> ReadAll();

        void Delete(int id);

        void CreateTable();

        void Create(AttributeTypeCollection attributeTypeCollection, EntityType entityType);
    }
}