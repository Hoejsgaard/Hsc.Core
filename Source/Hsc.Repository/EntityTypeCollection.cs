using System.Collections.ObjectModel;
using Hsc.Model.Knowledge;

namespace Hsc.Repository
{
    public class EntityTypeCollection : KeyedCollection<string, EntityType>
    {
        protected override string GetKeyForItem(EntityType item)
        {
            return item.Name;
        }
    }
}
