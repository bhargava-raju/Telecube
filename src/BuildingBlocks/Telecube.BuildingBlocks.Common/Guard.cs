namespace Telecube.BuildingBlocks.Common
{
    public static class Guard
    {
        public static Result AgainstNull(object? value, string paramName)
        {
            if(value is null)
            {
                return Result.Failure(Error.Validation("Validation.Required", $"Value cannot be null. Parameter name: {paramName}"));
            }

            return Result.Success();
        }

        public static Result AgainstNullOrEmpty(string? value, string paramName)
        {
            if(string.IsNullOrEmpty(value))
            {
                return Result.Failure(Error.Validation("Validation.Required", $"Value cannot be empty. Parameter name: {paramName}"));
            }

            return Result.Success();
        }
    }
}
