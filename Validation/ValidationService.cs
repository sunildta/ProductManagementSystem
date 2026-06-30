using Microsoft.Extensions.DependencyInjection;
using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ProductManagementSystem.Validation;
/// <summary>
/// Generic wrapper around FluentValidation. Resolves IValidator&lt;T&gt; from the
/// DI container for whatever entity type is passed in — works for Product,
/// Category, Supplier, and ProductReview without duplicating this class four times.
/// Registered as Transient — stateless, cheap to create each time.
/// </summary>
public class ValidationService : IValidationService
{
    private readonly IServiceProvider _provider;

    public ValidationService(IServiceProvider provider) => _provider = provider;

    public bool Validate<T>(T entity, out IReadOnlyList<string> errors)
    {
        var validator = _provider.GetService<IValidator<T>>();

        if (validator is null)
        {
            errors = new List<string> { $"No validator registered for {typeof(T).Name}." };
            return false;
        }

        var result = validator.Validate(entity!);
        errors = result.Errors.Select(e => e.ErrorMessage).ToList();
        return result.IsValid;
    }
}

/*
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
*/