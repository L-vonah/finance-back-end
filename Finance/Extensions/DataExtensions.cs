using Finance.Exceptions;

namespace Finance.Extensions
{
    public static class DataExtensions
    {
        public static T RequireNotNull<T>(this T obj)
        {
            if (obj == null)
            {
                throw new EntityNotNullException(typeof(T));
            }
            return obj;
        }
    }
}
