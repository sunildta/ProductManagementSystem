namespace ProductManagementSystem.Validation
{
    /// <summary>
    /// Generic validator contract. Each entity type (Product, Category, Supplier,
    /// ProductReview) gets its own implementation registered in DI.
    /// </summary>
    public interface IValidator<T>
    {
        FluentValidationResult Validate(T entity);
    }
}
