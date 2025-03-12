//Assets\Vurbiri\Runtime\Utilities\Errors.cs
using System;

namespace Vurbiri
{
    public static class Errors
	{
        public static void CheckForNull<T>(T value) where T : class
        {
           if (value == null) 
                throw new ArgumentNullException(nameof(value));
        }

        public static void CheckIndex(int index, int minExclude, int maxInclude)
        {
            if (index < minExclude | index >= maxInclude)
                throw new IndexOutOfRangeException($"index = {index}");
        }

        public static void CheckForMin<T>(T value, T minExclude) where T : IComparable<T>
        {
            if (value.CompareTo(minExclude) < 0)
                throw new ArgumentOutOfRangeException($"{value} < {minExclude}");
        }

        public static void CheckForMinMax<T>(T value, T minExclude, T maxInclude) where T : IComparable<T>
        {
            if (value.CompareTo(minExclude) < 0 | value.CompareTo(maxInclude) >= 0)
                throw new ArgumentOutOfRangeException($"{value} < {minExclude} || {value} >= {maxInclude}");
        }

        public static void Argument<T>(string paramName, T value) => throw new ArgumentException($"{paramName} = {value}");
        public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        public static T ArgumentOutOfRange<T>(string paramName, T value) => throw new ArgumentOutOfRangeException($"{paramName} = {value}");
        public static void IndexOutOfRange(int index) => throw new IndexOutOfRangeException($"index = {index}");
        
        public static void InvalidOperation() => throw new InvalidOperationException();

        public static void AddItem(string value) => throw new($"{value} has already been added.");
        public static void NotFound(string value) => throw new($"{value} not found.");
        public static void Error(string message) => throw new(message);
    }
}
