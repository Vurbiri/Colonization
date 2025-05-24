using Newtonsoft.Json;
using System;

namespace Vurbiri
{
    public static class Errors
	{
        public static void Argument<T>(string paramName, T value) => throw new ArgumentException($"{paramName} = {value}");
        
        public static void ArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        public static void ArgumentNull() => throw new ArgumentNullException();
        
        public static void IndexOutOfRange(int index) => throw new IndexOutOfRangeException($"index = {index}");

        public static void ArgumentOutOfRange() => throw new ArgumentOutOfRangeException();
        public static void ArgumentOutOfRange(string message) => throw new ArgumentOutOfRangeException(string.Empty, message);
        public static T ArgumentOutOfRange<T>(string paramName, T value) => throw new ArgumentOutOfRangeException($"{paramName} = {value}");

        public static void Rank(int length, int size) => throw new RankException($"Dimensionality {length} is not {size}");
        public static void Rank(string message) => throw new RankException(message);

        public static void InvalidOperation() => throw new InvalidOperationException();

        public static void AddItem(string value) => throw new($"{value} has already been added.");
        public static void NotFound(string value) => throw new($"{value} not found.");
        
        public static void Message(string message) => throw new(message);


        public static Exception NotSupportedRead(Type type) => new NotSupportedException($"Deserialization of type {type} is not supported.");
        public static Exception NotSupportedWrite(Type type) => new NotSupportedException($"Serialization of type {type} is not supported.");
        public static Exception JsonSerialization(Type type)
        { return new JsonSerializationException($"Converter cannot write specified value to JSON. {type} is required."); }
    }
}
