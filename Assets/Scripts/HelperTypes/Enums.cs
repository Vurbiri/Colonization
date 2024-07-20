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
    Bank,
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

public static class ExtensionsEnumsGame
{

    public static CityGroup ToGroup(this CityType self) => self switch
    {
        CityType.Signpost => CityGroup.Signpost,
        CityType.Shrine => CityGroup.Shrine,
        CityType.Berth or CityType.Port => CityGroup.Water,
        CityType.Watchtower or CityType.Barracks or CityType.Castle => CityGroup.Military,
        CityType.Store or CityType.Bank or CityType.Exchange => CityGroup.Economy,
        CityType.Workshop or CityType.Forge or CityType.Manufactory => CityGroup.Industry,
        CityType.Library or CityType.MageTower or CityType.University => CityGroup.Magic,
        CityType.TownHall or CityType.Police or CityType.Magistrate => CityGroup.Politics,
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

