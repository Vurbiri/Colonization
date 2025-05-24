namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class BuffSettings
    {
        public int targetAbility;
        public int typeModifier;
        public int value;

        public BuffSettings() => typeModifier = -1;

        public BuffSettings(BuffSettings other)
        {
            targetAbility = other.targetAbility;
            typeModifier = other.typeModifier;
            value = other.value;
        }
    }
}
