namespace ProductManagementSystem.Validation
{
    /// <summary>
    /// Simple result object returned by IValidator&lt;T&gt;.Validate().
    /// </summary>
    public class FluentValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<FluentValidationError> Errors { get; } = new();

        public static FluentValidationResult Success() => new();

        public static FluentValidationResult WithErrors(params string[] messages)
        {
            var result = new FluentValidationResult();
            foreach (var msg in messages)
                result.Errors.Add(new FluentValidationError(msg));
            return result;
        }
    }

    public class FluentValidationError
    {
        public string ErrorMessage { get; }
        public FluentValidationError(string errorMessage) => ErrorMessage = errorMessage;
    }
}
