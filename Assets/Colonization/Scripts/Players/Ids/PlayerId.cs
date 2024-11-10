namespace Vurbiri.Colonization
{
    public class PlayerId : AIdType<PlayerId>
    {
        public const int Demons         = -2;
        public const int None           = -1;
        public const int Player         = 0;
        public const int AI_01          = 1;
        public const int AI_02          = 2;
        
        [NotId] public const int PlayersCount = 3;

        static PlayerId() => RunConstructor();
        private PlayerId() { }
    }
}
