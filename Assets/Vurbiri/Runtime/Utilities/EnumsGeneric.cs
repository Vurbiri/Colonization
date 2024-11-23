//Assets\Vurbiri\Runtime\Utilities\EnumsGeneric.cs
using System;
namespace Vurbiri
{
    public enum AudioType
    {
        Music,
        SFX,
    }

    public enum AvatarSize
    {
        Small,
        Medium,
        Large
    }

    public enum MessageType
    {
        Normal,
        Warning,
        Error,
        FatalError
    }

    public enum GameModeStart
    {
        New,
        Continue
    }

    public class Enum<T> where T : Enum
    {
        public static int Count => Enum.GetNames(typeof(T)).Length;
        public static T[] Values => (T[])Enum.GetValues(typeof(T));
        public static string[] Names => Enum.GetNames(typeof(T));
        public static T Rand(int minInclusive, int maxExclusive) => UnityEngine.Random.Range(minInclusive, maxExclusive).ToEnum<T>();
    }
}

