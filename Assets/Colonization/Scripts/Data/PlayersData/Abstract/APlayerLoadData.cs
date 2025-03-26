//Assets\Colonization\Scripts\Data\PlayersData\Abstract\APlayerLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public abstract class APlayerLoadData
	{
        public readonly bool isLoaded;
        public readonly int[] artefact;
        public readonly List<ActorLoadData> actors;

        public APlayerLoadData(int[] artefact, List<ActorLoadData> actors)
        {
            isLoaded = true;
            this.artefact = artefact;
            this.actors = actors;
        }

        public APlayerLoadData()
        {
            isLoaded = false;
            actors = new(0);
        }
    }
}
