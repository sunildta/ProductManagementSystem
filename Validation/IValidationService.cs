using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagementSystem.Validation
{
    public interface IValidationService
    {
        ///<summary>
        ///validate product using data annotation, return true if valid
        ///else populates<paramref name="error"/>
        ///</summary>
        bool Validate(Product product, out IReadOnlyList<string> errors);
    }
}
