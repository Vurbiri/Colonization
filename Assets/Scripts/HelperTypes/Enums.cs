using System;

public enum AddMode
{
    First,
    Last
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
    DR_UL,
    DL_UR,
    DD_UU,
}

public enum SurfaceType
{
    Village,
    Plain,
    Crystals,
    Mountain,
    Forest,
    Water,
    Gate
}

public enum CurrencyType
{
    Food,
    Land02,
    Mana,
    Ore,
    Wood,
    Gate
}

public enum EdificeGroup
{
    None,
    Shrine,
    Water,
    Urban
}

public enum EdificeBuildType
{
    None,
    Build,
    Upgrade
}

public enum EdificeType
{
    None,

    Shrine,

    PortOne,
    PortTwo,

    Town,
    City,
    Capital
}


public static class ExtensionsEnumsGame
{

    public static EdificeGroup ToGroup(this EdificeType self) => self switch
    {
        EdificeType.None => EdificeGroup.None,
        EdificeType.Shrine => EdificeGroup.Shrine,
        EdificeType.PortOne or EdificeType.PortTwo => EdificeGroup.Water,
        EdificeType.Town or EdificeType.City or EdificeType.Capital => EdificeGroup.Urban,
        _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение CityType: {self}"),
    };

    public static EdificeBuildType ToBuildType(this EdificeType self) => self switch
    {
        EdificeType.None => EdificeBuildType.None,
        EdificeType.Shrine or EdificeType.PortOne or EdificeType.PortTwo or EdificeType.Town => EdificeBuildType.Build,
        EdificeType.City or EdificeType.Capital => EdificeBuildType.Upgrade,
        _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение CityType: {self}"),
    };
}

