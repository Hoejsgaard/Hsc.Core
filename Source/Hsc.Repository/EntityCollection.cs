using System.Collections.ObjectModel;
using Hsc.Model.Operation;

namespace Hsc.Repository
{
    public class EntityCollection : KeyedCollection<int, Entity>
    {
        protected override int GetKeyForItem(Entity item)
        {
            return item.Id;
        }
    }
}
