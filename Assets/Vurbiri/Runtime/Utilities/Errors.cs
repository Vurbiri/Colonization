//Assets\Vurbiri\Runtime\Utilities\Errors.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vurbiri
{
    public static class Errors
	{
        #region ThrowIf..
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull<T>(T value) where T : class
        {
           if (value == null)
                ArgumentNull(nameof(value));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfOutOfRange(int index, int maxExclude)
        {
            if (index < 0 | index >= maxExclude)
                IndexOutOfRange(index);
        }

        public static void ThrowIfLess<T>(T value, T minInclude) where T : IComparable<T>
        {
            if (value.CompareTo(minInclude) < 0)
                ArgumentOutOfRange($"{value} < {minInclude}");
        }

        public static void ThrowIfOutOfRange<T>(T value, T minInclude, T maxExclude) where T : IComparable<T>
        {
            if (value.CompareTo(minInclude) < 0 | value.CompareTo(maxExclude) >= 0)
                ArgumentOutOfRange($"{value} < {minInclude} || {value} >= {maxExclude}");
        }

        public static void ThrowIfLengthNotEqual<T>(IReadOnlyCollection<T> value, int size)
        {
            if (value.Count != size)
                Rank(value.Count, size);
        }
        public static void ThrowIfLengthNotEqual<T>(IReadOnlyCollection<T> valueA, IReadOnlyCollection<T> valueB)
        {
            if (valueA.Count != valueB.Count)
                Rank(valueA.Count, valueB.Count);
        }
        public static void ThrowIfLengthNotEqual(int length, int size)
        {
            if (length != size)
                Rank(length, size);
        }

        public static void ThrowIfLengthZero<T>(IReadOnlyCollection<T> value)
        {
            if (value.Count == 0)
                ArgumentOutOfRange($"Length = 0");
        }
        #endregion

        #region Exceptions
        public static void Argument<T>(string paramName, T value) => throw new ArgumentException($"{paramName} = {value}");
        public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        public static void IndexOutOfRange(int index) => throw new IndexOutOfRangeException($"index = {index}");
        public static void ArgumentOutOfRange(string message) => throw new ArgumentOutOfRangeException(message);
        public static T ArgumentOutOfRange<T>(string paramName, T value) => throw new ArgumentOutOfRangeException($"{paramName} = {value}");
        public static void Rank(int length, int size) => throw new RankException($"Length {length} != {size}");
        
        public static void InvalidOperation() => throw new InvalidOperationException();

        public static void AddItem(string value) => throw new($"{value} has already been added.");
        public static void NotFound(string value) => throw new($"{value} not found.");
        
        public static void Error(string message) => throw new(message);
        #endregion

        public static Exception NotSupportedRead(Type type) => new NotSupportedException($"Deserialization of type {type} is not supported.");
        public static Exception NotSupportedWrite(Type type) => new NotSupportedException($"Serialization of type {type} is not supported.");
        public static Exception JsonSerialization(Type type)
        { return new JsonSerializationException($"Converter cannot write specified value to JSON. {type} is required."); }
    }
}
