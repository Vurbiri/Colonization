namespace Vurbiri.Colonization
{
	public class SpellParam
	{
        public int playerId;
        public int valueA;
        public int valueB;

        public SpellParam(int playerId)
        {
            this.playerId = playerId;
            valueA = valueB = 0;
        }
        public SpellParam(int playerId, int valueA)
        {
            this.playerId = playerId;
            this.valueA = valueA;
            valueB = 0;
        }
        public SpellParam(int playerId, int valueA, int valueB)
        {
            this.playerId = playerId;
            this.valueA = valueA;
            this.valueB = valueB;
        }

        public void Reset() => valueA = valueB = 0;
    }
}
