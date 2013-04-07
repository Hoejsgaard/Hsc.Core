using System.Collections.ObjectModel;

namespace Hsc.Model.Knowledge
{
    public class AttributeTypeCollection : KeyedCollection<string, AttributeType>
    {
        protected override string GetKeyForItem(AttributeType item)
        {
            return item.Name;
        }

        public void Add(AttributeTypeCollection attributes)
        {
            if (attributes != null)
            {
                foreach (AttributeType attributeType in attributes)
                {
                    Add(attributeType);
                }
            }
        }
    }
}