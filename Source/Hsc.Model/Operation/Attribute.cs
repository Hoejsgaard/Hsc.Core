using System.Runtime.Serialization;
using Hsc.Model.Knowledge;

namespace Hsc.Model.Operation
{
    public class Attribute
    {
        private object _value;

        public Attribute(string name, AttributeType attributeType)
        {
            Name = name;
            AttributeType = attributeType;
        }

        public Attribute()
        {
            //serialization
        }

        public AttributeType AttributeType { get; private set; }

        public string Name { get; private set; }

        public object Value
        {
            get { return _value; }
            set
            {
                ValidationResult validationResult = AttributeType.ValidateValue(value);
                if (validationResult.IsValid)
                {
                    _value = value;
                }
                else
                {
                    throw new OperationException();
                }
            }
        }
    }
}