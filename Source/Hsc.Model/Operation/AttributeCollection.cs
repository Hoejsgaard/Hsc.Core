using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hsc.Model.Knowledge;

namespace Hsc.Model.Operation
{
    public class AttributeCollection : KeyedCollection<string, Attribute>
    {
        public AttributeCollection(AttributeTypeCollection attributeTypeCollection)
        {
            if (attributeTypeCollection == null)
            {
                throw new OperationException();
            }
            AttributeTypes = attributeTypeCollection;
        }

        public AttributeTypeCollection AttributeTypes { get; private set; }

        public new Attribute this[string key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch (KeyNotFoundException)
                {
                    AttributeType attributeType = AttributeTypes[key];
                    var attribute = new Attribute(key, attributeType);
                    Add(attribute);
                    return attribute;
                }
            }
        }

        protected override string GetKeyForItem(Attribute item)
        {
            return item.Name;
        }

        public Attribute Add(string attributeName)
        {
            AttributeType type = AttributeTypes[attributeName];
            var attribute = new Attribute(attributeName, type);
            Add(attribute);
            return attribute;
        }

        public new void Add(Attribute attribute)
        {
            ThrowIfNotValid(attribute);
            base.Add(attribute);
        }

        private void ThrowIfNotValid(Attribute attribute)
        {
            if (!HasLegalName(attribute) ||
                !HasCorrectType(attribute))
            {
                throw new OperationException();
            }
        }

        private bool HasLegalName(Attribute attribute)
        {
            return AttributeTypes.Contains(attribute.Name);
        }

        private bool HasCorrectType(Attribute attribute)
        {
            AttributeType attributeType = AttributeTypes[attribute.Name];
            return attribute.AttributeType == attributeType;
        }
    }
}