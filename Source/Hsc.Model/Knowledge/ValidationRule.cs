using System;
using System.Runtime.Serialization;

namespace Hsc.Model.Knowledge
{
    [DataContract]
    public class ValidationRule
    {
        [DataMember]
        public Func<object, ValidationResult> ValidationFunction;

        public virtual ValidationResult Validate(object obj)
        {
            return ValidationFunction(obj);
        }
    }
}
