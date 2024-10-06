namespace Vurbiri.Colonization
{
    public class PlayerId : AIdType<PlayerId>
    {
        
        public const int Human          = 0;
        public const int AI_01          = 1;
        public const int AI_02          = 2;
        [HideAllNextIds]
        public const int None           = 3;

        static PlayerId() { RunConstructor(); }
        private PlayerId() { }
    }
}
