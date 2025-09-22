namespace Vurbiri.Colonization
{
    public static class TAG
    {
        public const string SPRITE = "<sprite={0}>";

        public const string CURRENCY = "<sprite={0}><space=0.1em>{1}<space=0.3em>";
        public const string COLOR_CURRENCY = "<sprite={0}>{2}<space=0.1em>{1}<space=0.3em>";

        //public const string COLOR = "<color={0}>";
        public const string COLOR = "<{0}>";
        public const string COLOR_OFF = "</color>";

        public const string ALING_CENTER = "<align=\"center\">";
        public const string ALING_OFF = "</align>";

        public static readonly int TAG_SPRITE_LENGTH = SPRITE.Length;
    }
}
