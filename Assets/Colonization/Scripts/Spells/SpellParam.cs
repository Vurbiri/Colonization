namespace Vurbiri.Colonization
{
	public struct SpellParam
	{
        public Id<PlayerId> playerId;
        public int valueA;
        public int valueB;

        public SpellParam(Id<PlayerId> playerId)
        {
            this.playerId = playerId;
            valueA = valueB = 0;
        }
        public SpellParam(Id<PlayerId> playerId, int valueA)
        {
            this.playerId = playerId;
            this.valueA = valueA;
            valueB = 0;
        }
        public SpellParam(Id<PlayerId> playerId, int valueA, int valueB)
        {
            this.playerId = playerId;
            this.valueA = valueA;
            this.valueB = valueB;
        }

        public void Reset() => valueA = valueB = 0;
    }
}
