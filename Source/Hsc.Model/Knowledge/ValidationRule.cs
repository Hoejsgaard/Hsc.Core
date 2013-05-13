using System;
using System.Runtime.Serialization;

namespace Hsc.Model.Knowledge
{
    public class ValidationRule
    {
        public Func<object, ValidationResult> ValidationFunction;

        public virtual ValidationResult Validate(object obj)
        {
            return ValidationFunction(obj);
        }
    }
}