//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\ActorLoadData.cs
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Data
{
    public class ActorLoadData
	{
        public readonly int id;
        public readonly Key keyHex;
        public readonly int currentHP;
        public readonly int currentAP;
		public readonly int move;
        public readonly IReadOnlyList<ReactiveEffect> effects;

        public ActorLoadData(int[][] actorData)
        {
            int n = 0, m = 0;
            keyHex = new(actorData[n++]);

            id = actorData[n][m++];
            currentHP = actorData[n][m++];
            currentAP = actorData[n][m++];
            move = actorData[n++][m++];

            int count = actorData.Length - n;
            var effects = new ReactiveEffect[count];
            for (int l = 0; l < count; l++, n++)
                effects[l] = new(actorData[n]);

            this.effects = effects;
        }
    }
}
