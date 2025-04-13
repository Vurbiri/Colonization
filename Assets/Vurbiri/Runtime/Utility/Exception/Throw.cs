//Assets\Vurbiri\Runtime\Utility\Exception\Throw.cs
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public static class Throw
	{
        #region ArgumentNull
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfNull<T>(T value) where T : class
        {
            if (value == null) Errors.ArgumentNull();
        }
        public static void IfNull<T>(T value, string paramName) where T : class
        {
            if (value == null) Errors.ArgumentNull(paramName);
        }
        #endregion

        public static void IfIndexOutOfRange(int index, int maxExclude)
        {
            if (index < 0 | index >= maxExclude)
                Errors.IndexOutOfRange(index);
        }

        #region ArgumentOutOfRange
        public static void IfOutOfRange<T>(T value, T minInclude, T maxExclude) where T : IComparable<T>
        {
            if (value.CompareTo(minInclude) < 0 | value.CompareTo(maxExclude) >= 0)
                Errors.ArgumentOutOfRange($"Value {value} is less than {minInclude} or greater than or equal to {maxExclude}");
        }
        public static void IfLess<T>(T value, T minInclude) where T : IComparable<T>
        {
            if (value.CompareTo(minInclude) < 0)
                Errors.ArgumentOutOfRange($"Value {value} is less than {minInclude}");
        }
        public static void IfGreater<T>(T value, T maxInclude) where T : IComparable<T>
        {
            if (value.CompareTo(maxInclude) > 0)
                Errors.ArgumentOutOfRange($"Value {value} is greater than Max: {maxInclude}");
        }
        public static void IfGreater<T>(T value, T maxInclude, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(maxInclude) > 0)
                Errors.ArgumentOutOfRange($"{paramName} ({value}) is greater than Max: {maxInclude}");
        }
        public static void IfZero(int value)
        {
            if (value == 0) Errors.ArgumentOutOfRange($"Value is 0");
        }
        public static void IfZero(float value)
        {
            if (Mathf.Approximately(value, 0f)) Errors.ArgumentOutOfRange($"Value is 0");
        }
        public static void IfNegative(int value)
        {
            if (value < 0) Errors.ArgumentOutOfRange($"Value {value} is negative");
        }
        public static void IfNegative(int value, string paramName)
        {
            if (value < 0) Errors.ArgumentOutOfRange($"{paramName} ({value}) is negative");
        }
        #endregion

        #region Rank
        public static void IfLengthNotEqual<T>(IReadOnlyCollection<T> value, int size)
        {
            if (value.Count != size)
                Errors.Rank(value.Count, size);
        }
        public static void IfLengthNotEqual<T>(IReadOnlyCollection<T> valueA, IReadOnlyCollection<T> valueB)
        {
            if (valueA.Count != valueB.Count)
                Errors.Rank(valueA.Count, valueB.Count);
        }
        public static void IfLengthNotEqual(int length, int size)
        {
            if (length != size) Errors.Rank(length, size);
        }

        public static void IfLengthZero<T>(IReadOnlyCollection<T> value)
        {
            if (value.Count == 0) Errors.Rank("Dimensionality is zero");
        }
        #endregion
    }
}
