using System.Runtime.Serialization;
using Hsc.Model.Knowledge;

namespace Hsc.Model.Operation
{
    [DataContract]
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

        [DataMember]
        public AttributeType AttributeType { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                ValidationResult validationResult = AttributeType.ValidateValue(value);
                if (validationResult.IsValid)
                    _value = value;
                else
                    throw new OperationException();
            }
        }
    }
}
