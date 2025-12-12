using Newtonsoft.Json;
using System;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class Errors
	{
        [Impl(8)] public static void Argument<T>(string paramName, T value) => throw new ArgumentException($"{paramName} = {value}");

        [Impl(8)] public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        [Impl(8)] public static void ArgumentNull() => throw new ArgumentNullException();

        [Impl(8)] public static void IndexOutOfRange(int index) => throw new IndexOutOfRangeException($"index = {index}");

        [Impl(8)] public static void ArgumentOutOfRange() => throw new ArgumentOutOfRangeException();
        [Impl(8)] public static void ArgumentOutOfRange(string message) => throw new ArgumentOutOfRangeException(string.Empty, message);
        [Impl(8)] public static T ArgumentOutOfRange<T>(string paramName, T value) => throw new ArgumentOutOfRangeException($"{paramName} = {value}");

        [Impl(8)] public static void Rank(int length, int size) => throw new RankException($"Dimensionality {length} is not {size}");
        [Impl(8)] public static void Rank(string message) => throw new RankException(message);

        [Impl(8)] public static void InvalidOperation() => throw new InvalidOperationException();
        [Impl(8)] public static void InvalidOperation(string message) => throw new InvalidOperationException(message);

        [Impl(8)] public static void AddItem(string value) => throw new($"{value} has already been added.");
        [Impl(8)] public static void NotFound(string value) => throw new($"{value} not found.");

        [Impl(8)] public static void Message(string message) => throw new(message);

        [Impl(8)] public static Exception NotSupportedRead(Type type) => new NotSupportedException($"Deserialization of type {type} is not supported.");
        [Impl(8)] public static Exception NotSupportedWrite(Type type) => new NotSupportedException($"Serialization of type {type} is not supported.");
        [Impl(8)] public static Exception JsonSerialization(Type type)
        { return new JsonSerializationException($"Converter cannot write specified value to JSON. {type} is required."); }
    }
}
