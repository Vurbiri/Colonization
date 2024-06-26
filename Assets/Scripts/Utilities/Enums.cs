using System;
using System.ComponentModel;

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

public enum AddMode
{
    Last,
    First
}

public enum PlayerType
{
    None = -1,
    Human,
    AI_01,
    AI_02,
    AI_03
}

public enum SurfaceType
{
    Ground01,
    Ground02,
    Ground03,
    Ground04,
    Ground05,
    Water,
    Gate
}

public enum LinkType
{
    DR_UL,
    DL_UR,
    DD_UU, 
}

public enum CityType
{
    Signpost,
    Shrine,
    Camp,
    Watchtower,
    Castle,
    Stronghold
}

public class Enum<T> where T : Enum
{
    public static int Count => Enum.GetNames(typeof(T)).Length;
    public static T[] GetValues() => (T[])Enum.GetValues(typeof(T));
    public static string[] GetNames() => Enum.GetNames(typeof(T));
    public static T Rand(int minInclusive, int maxExclusive) => UnityEngine.Random.Range(minInclusive, maxExclusive).ToEnum<T>();
}

public static class ExtensionsEnum
{
    public static int ToInt<T>(this T self) where T : Enum => Convert.ToInt32(self);
    public static int ToInt<T>(this T self, int offset) where T : Enum => Convert.ToInt32(self) + offset;
    public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);
}

