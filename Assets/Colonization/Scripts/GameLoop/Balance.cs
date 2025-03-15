//Assets\Colonization\Scripts\GameLoop\Balance.cs
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public class Balance
	{
        public const int MIN = -666, MAX = 667;
        public const int PER_PERK = -1, PER_CHRINE = 15;

        private ReactiveValue<int> _value;



        public Balance(Human[] humans)
        {
            for (int i = 0; i < PlayerId.PlayersCount; i++)
            {
                humans[i].Shrines.Subscribe(OnShrineBuild, false);
            }
        }

        private void OnShrineBuild(int index, Crossroad crossroad, TypeEvent type)
        {
            if (type == TypeEvent.Add)
                _value.Value += PER_CHRINE;
        }
    }
}
