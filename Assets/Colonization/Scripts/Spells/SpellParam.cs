namespace Vurbiri.Colonization
{
	public struct SpellParam
	{
        public int playerId;
        public int valueA;
        public int valueB;

        public SpellParam(int playerId)
        {
            this.playerId = playerId;
            valueA = valueB = 0;
        }
    }
}
