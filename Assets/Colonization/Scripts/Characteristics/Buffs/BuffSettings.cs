namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class BuffSettings
    {
        public int targetAbility;
        public int typeModifier;
        public int value;
        public int advance;

        public BuffSettings() => typeModifier = -1;

        public BuffSettings(BuffSettings other)
        {
            targetAbility = other.targetAbility;
            typeModifier = other.typeModifier;
            value = other.value;
            advance = other.advance;
        }
    }
}
