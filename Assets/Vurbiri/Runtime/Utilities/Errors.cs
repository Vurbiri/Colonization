//Assets\Vurbiri\Runtime\Utilities\Errors.cs
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    public static class Errors
	{
        #region Check
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckForNull<T>(T value) where T : class
        {
           if (value == null)
                ArgumentNull(nameof(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckIndex(int index, int minExclude, int maxInclude)
        {
            if (index < minExclude | index >= maxInclude)
                IndexOutOfRange(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckForMin<T>(T value, T minExclude) where T : IComparable<T>
        {
            if (value.CompareTo(minExclude) < 0)
                ArgumentOutOfRange($"{value} < {minExclude}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckForMinMax<T>(T value, T minExclude, T maxInclude) where T : IComparable<T>
        {
            if (value.CompareTo(minExclude) < 0 | value.CompareTo(maxInclude) >= 0)
                ArgumentOutOfRange($"{value} < {minExclude} || {value} >= {maxInclude}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckArraySize<T>(IReadOnlyList<T> value, int size)
        {
            if (value.Count != size)
                Error($"Array size {value.Count} != {size}");
        }
        #endregion

        #region Exceptions
        public static void Argument<T>(string paramName, T value) => throw new ArgumentException($"{paramName} = {value}");
        public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        public static void IndexOutOfRange(int index) => throw new IndexOutOfRangeException($"index = {index}");
        public static void ArgumentOutOfRange(string message) => throw new ArgumentOutOfRangeException(message);
        public static T ArgumentOutOfRange<T>(string paramName, T value) => throw new ArgumentOutOfRangeException($"{paramName} = {value}");

        public static void InvalidOperation() => throw new InvalidOperationException();

        public static void AddItem(string value) => throw new($"{value} has already been added.");
        public static void NotFound(string value) => throw new($"{value} not found.");
        public static void Error(string message) => throw new(message);
        #endregion
    }
}
