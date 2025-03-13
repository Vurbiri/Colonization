//Assets\Colonization\Scripts\Characteristics\Buffs\Settings\DemonBuffSettings.cs
namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class DemonBuffSettings : BuffSettings
    {
        public int levelUP;

        public DemonBuffSettings() : base() {}

        public DemonBuffSettings(DemonBuffSettings other) : base(other) => levelUP = other.levelUP;
    }
}
