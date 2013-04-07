using System;

namespace Hsc.Model.Knowledge
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string Message { get; private set; }

        private ValidationResult()
        {
        }

        public static ValidationResult Valid()
        {
            return new ValidationResult
            {
                IsValid = true,
                Message = String.Empty
            };
        }

        public static ValidationResult Invalid(string message)
        {
            return new ValidationResult
            {
                IsValid = false,
                Message = message
            };
        }
        
        public override string ToString()
        {
            if (IsValid)
                return string.Format("Valid");
            else
                return string.Format("Invalid : {0}", Message);
        }
    }
}
