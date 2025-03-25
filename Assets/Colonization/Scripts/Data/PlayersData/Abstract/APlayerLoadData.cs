//Assets\Colonization\Scripts\Data\PlayersData\Abstract\APlayerLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public abstract class APlayerLoadData
	{
        public readonly bool isLoaded;
        public readonly int[] artefact;
        public readonly ActorLoadData[] actors;

        public APlayerLoadData(int[] artefact, List<int[][]> actorsData)
        {
            isLoaded = true;
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

        public APlayerLoadData()
        {
            isLoaded = false;
            actors = new ActorLoadData[0];
        }
    }
}
