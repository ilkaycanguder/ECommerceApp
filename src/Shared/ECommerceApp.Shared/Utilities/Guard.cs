using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Shared.Utilities
{
    public static class Guard
    {
        public static T AgainstNull<T>(T? value, string parameterName) where T : class
        {
            if (value is null)
                throw new ArgumentNullException(parameterName);

            return value;
        }

        public static string AgainstNullOrEmpty(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or empty.", parameterName);

            return value;
        }

        public static int AgainstNegative(int value, string parameterName)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative.", parameterName);

            return value;
        }
    }
}
