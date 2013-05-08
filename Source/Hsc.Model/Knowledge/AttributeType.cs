using System.Collections.Generic;

namespace Hsc.Model.Knowledge
{
    public class AttributeType
    {
        public AttributeType(string name, DataType dataType)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new KnowledgeException();
            }
            Name = name;
            DataType = dataType;
            ValidationRules = new List<ValidationRule>();
        }

        public AttributeType()
        {
            // for serialization
            ValidationRules = new List<ValidationRule>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public DataType DataType { get; set; }
        public List<ValidationRule> ValidationRules { get; set; }

        public ValidationResult ValidateValue(object value)
        {
            foreach (ValidationRule rule in ValidationRules)
            {
                ValidationResult result = rule.Validate(value);
                if (!result.IsValid)
                {
                    return ValidationResult.Invalid(result.Message);
                }
            }
            return ValidationResult.Valid();
        }
    }
}