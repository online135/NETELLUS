namespace NETELLUS.Extensions
{
    public static class NullableObjectExtensions
    {
        public static bool IsNull<T>(this T objectInstance) where T : new()
        {
            return objectInstance is null;
        }

        public static bool IsNotNull<T>(this T objectInstance) where T : new()
        {
            return !objectInstance.IsNull();
        }
    }
}
