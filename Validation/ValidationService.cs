using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Validation
{
    public class ValidationService : IValidationService
    {
        public bool Validate(Product product, out IReadOnlyList<string> errors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(product);
            var isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            errors = results.Select(r => r.ErrorMessage ?? "unknown error.").ToList();
            return isValid;
        }
    }

}
