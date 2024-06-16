using System;
using UnityEngine;

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

public enum LinkType
{
    NE_SW,
    SE_NW,
    S_N, 
}

public enum CrossroadType
{
    Up,
    Down,
    None
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

public class Enum<T> where T : Enum
{
    public static int Count => Enum.GetNames(typeof(T)).Length;
    public static T[] GetValues() => (T[])Enum.GetValues(typeof(T));
}



