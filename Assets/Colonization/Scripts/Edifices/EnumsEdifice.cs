using System;

namespace Vurbiri.Colonization
{
    public enum EdificeGroup
    {
        None,
        Shrine,
        Water,
        Urban
    }

    public enum EdificeType
    {
        None,

        Shrine,

        PortOne,
        PortTwo,
        LighthouseOne,
        LighthouseTwo,

        Camp,
        Town,
        Capital
    }

    public static class ExtensionsEdificeGroup
    {
        public static EdificeGroup ToGroup(this EdificeType self) => self switch
        {
            EdificeType.None => EdificeGroup.None,
            EdificeType.Shrine => EdificeGroup.Shrine,
            EdificeType.PortOne or EdificeType.PortTwo or EdificeType.LighthouseOne or EdificeType.LighthouseTwo => EdificeGroup.Water,
            EdificeType.Camp or EdificeType.Town or EdificeType.Capital => EdificeGroup.Urban,
            _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение CityType: {self}"),
        };

    }
}
