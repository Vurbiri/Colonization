using System;

namespace Vurbiri.Colonization
{
    public enum EdificeGroup
    {
        None,
        Shrine,
        Port,
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
            EdificeType.PortOne or EdificeType.PortTwo or EdificeType.LighthouseOne or EdificeType.LighthouseTwo => EdificeGroup.Port,
            EdificeType.Camp or EdificeType.Town or EdificeType.Capital => EdificeGroup.Urban,
            _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение EdificeType - {self} в ToGroup()"),
        };

        public static PlayerAbilityType ToAbilityType(this EdificeGroup self) => self switch
        {
            EdificeGroup.None => PlayerAbilityType.None,
            EdificeGroup.Shrine => PlayerAbilityType.MaxShrine,
            EdificeGroup.Port => PlayerAbilityType.MaxPort,
            EdificeGroup.Urban => PlayerAbilityType.MaxUrban,
            _ => throw new ArgumentOutOfRangeException("self", $"Неожидаемое значение EdificeGroup - {self} в ToAbilityType()"),
        };

    }
}
