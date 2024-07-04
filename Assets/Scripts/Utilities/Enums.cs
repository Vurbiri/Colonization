using System;

public enum TextFiles
{
    Main,
    City,
}


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

public enum Resource
{
    Land01,
    Land02,
    Land03,
    Land04,
    Land05,
    Water,
    Gate
}

public enum LinkType
{
    DR_UL,
    DL_UR,
    DD_UU, 
}

public enum CityGroup
{
    Signpost = -1,
    Military,
    Economy,
    Industry,
    Magic,
    Politics,
    Water,
    Shrine
}

public enum CityBuildType
{
    None,
    Berth,
    Port,
    Shrine,
    Build,
    Upgrade
}

public enum CityType
{
    Signpost,
    Shrine,

    Berth,
    Port,

    Watchtower,
    Barracks,
    Castle,

    Store,
    Warehouse,
    Exchange,

    Workshop,
    Forge,
    Manufactory,

    Library,
    MageTower,
    University,

    TownHall,
    Police,
    Magistrate
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

    public static CityGroup ToGroup(this CityType self) => self switch
    {
        CityType.Signpost                                             => CityGroup.Signpost,
        CityType.Shrine                                               => CityGroup.Shrine,
        CityType.Berth or CityType.Port                               => CityGroup.Water,
        CityType.Watchtower or CityType.Barracks or CityType.Castle   => CityGroup.Military,
        CityType.Store or CityType.Warehouse or CityType.Exchange     => CityGroup.Economy,
        CityType.Workshop or CityType.Forge or CityType.Manufactory   => CityGroup.Industry,
        CityType.Library or CityType.MageTower or CityType.University => CityGroup.Magic,
        CityType.TownHall or CityType.Police or CityType.Magistrate   => CityGroup.Politics,
        _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение CityType: {self}"),
    };

    public static CityType ToCityType(this CityBuildType self) => self switch
    {
        CityBuildType.Berth => CityType.Berth,
        CityBuildType.Port => CityType.Port,
        CityBuildType.Shrine => CityType.Shrine,
        _ => throw new ArgumentOutOfRangeException("ToCityType", $"Неожидаемое значение CityBuildType: {self}"),
    };
}

