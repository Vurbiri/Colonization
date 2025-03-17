//Assets\Colonization\Scripts\Data\PlayersData\Abstract\APlayerLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public abstract class APlayerLoadData
	{
        public readonly IReadOnlyList<int> artefact;
        public readonly IReadOnlyList<ActorLoadData> actors;

        public APlayerLoadData(int[] artefact, List<int[][]> actorsData)
        {
            this.artefact = artefact;
            this.actors = CreateActorData(actorsData);

            #region Local: CreateActorData(...)
            //================================================================
            static ActorLoadData[] CreateActorData(List<int[][]> actorsData)
            {
                int count = actorsData.Count;
                ActorLoadData[] actors = new ActorLoadData[count];

                for (int i = 0; i < count; i++)
                    actors[i] = new(actorsData[i]);

                return actors;
            }
            #endregion
        }
    }
}
