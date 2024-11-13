namespace Vurbiri.Colonization
{
    public readonly struct ActorLoadData
	{
        public readonly int id;
        public readonly Key keyHex;
        public readonly int currentHP;
        public readonly int currentAP;
		public readonly int move;
		public readonly Effect[] effects;

		public ActorLoadData(int[][] actorData)
        {
            keyHex = new(actorData[0]);
            id = actorData[1][0];
            currentHP = actorData[1][1];
            currentAP = actorData[1][2];
            move = actorData[1][3];

            int count = actorData.Length - 2;
            effects = new Effect[count];
            for (int i = 0, j = 2; i < count; i++, j++)
                effects[i] = new(actorData[j]);
        }
    }
}
