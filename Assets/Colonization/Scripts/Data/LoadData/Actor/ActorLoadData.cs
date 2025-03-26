//Assets\Colonization\Scripts\Data\LoadData\Actor\ActorLoadData.cs
using Newtonsoft.Json;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Data
{
    [JsonConverter(typeof(Actor.Converter))]
    public class ActorLoadData
	{
        public readonly Key keyHex;
        public readonly ActorState state;
        public readonly ReactiveEffect[] effects;

        public ActorLoadData(Key keyHex, ActorState state, ReactiveEffect[] effects)
        {
            this.keyHex = keyHex;
            this.state = state;
            this.effects = effects;
        }
    }
}
