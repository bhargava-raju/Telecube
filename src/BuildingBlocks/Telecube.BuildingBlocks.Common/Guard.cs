namespace Telecube.BuildingBlocks.Common
{
    public static class Guard
    {
        public static void AgainstNull(object? value, string paramName)
        {
            if(value is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void AgainstNullOrEmpty(string? value, string paramName)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value cannot be empty.", paramName);
            }
        }
    }
}
