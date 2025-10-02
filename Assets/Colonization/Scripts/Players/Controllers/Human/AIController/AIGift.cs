using System.Collections;
using System.Text;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class AIController
	{
		private class Gift
		{
            private static readonly AIGiftSettings s_settings;

            private readonly WaitResultSource<bool> _waitGift = new();
            private readonly AIController _parent;
            private readonly int _relationOffset;
            private readonly CurrenciesLite _gift = new(), _clone = new();
            public int _ratio;
            private string _msg;

            private int Id { [Impl(256)] get => _parent._id; }
            private Currencies Resources { [Impl(256)] get => _parent._resources; }
            private int MaxResources { [Impl(256)] get => _parent._abilities[HumanAbilityId.MaxMainResources]; }

            static Gift() => s_settings = SettingsFile.Load<AIGiftSettings>();

            public Gift(AIController parent)
			{
				parent._subscription += Localization.Instance.Subscribe(SetMsg);
                _ratio = s_settings.ratio;
                _parent = parent;
                _relationOffset = GameContainer.Diplomacy.Max >> s_settings.shiftRelation;
            }

            [Impl(256)] public void Update() => _ratio = s_settings.ratio;

            public WaitResult<bool> Receive(int giver, CurrenciesLite gift)
			{
                int amount = gift.Amount * _ratio;
                if (GameContainer.Diplomacy.IsGreatFriend(Id, giver))
                    amount <<= 1;
                else if (GameContainer.Diplomacy.IsGreatEnemy(Id, giver))
                    amount >>= 1;

                bool result = amount > Resources.Amount;
                if (result)
                {
                    Resources.Add(gift);
                    GameContainer.Diplomacy.Gift(Id, giver);
                }

                return _waitGift.SetResult(result);
            }

            public IEnumerator TryGive_Cn()
            {
                for(int i = 0; i < PlayerId.HumansCount; i++)
                    yield return TryGive_Cn(i);

                //Local
                IEnumerator TryGive_Cn(int receiver)
                {
                    int amount = Resources.Amount;
                    if (receiver != Id & amount > s_settings.minAmount)
                    {
                        if (Chance.Rolling((GameContainer.Diplomacy[receiver, Id] - _relationOffset) + (amount - MaxResources << s_settings.shiftMax)))
                        {
                            _gift.Clear(); _clone.Import(Resources);
                            int countGift = 1 + UnityEngine.Random.Range(0, amount - s_settings.minAmount >> s_settings.shiftAmount);
                            for (int i = 0; i < countGift; i++)
                                _gift.IncrementMain(_clone.DecrementMaxMain());

                            string msg = null;
                            if (receiver == PlayerId.Person)
                            {
                                StringBuilder sb = new(TAG.ALING_CENTER, 256);
                                sb.Append(GameContainer.UI.PlayerNames[Id]); sb.Append(" "); sb.AppendLine(_msg);
                                _gift.MainPlusToStringBuilder(sb); sb.Append(TAG.ALING_OFF);
                                msg = sb.ToString();
                            }
                            var wait = GameContainer.Players.Humans[receiver].OnGift(Id, _gift, msg);
                            yield return wait;
                            if (wait) Resources.Remove(_gift);
                        }
                    }

                    yield break;
                }
            }

            private void SetMsg(Localization localization) => _msg = localization.GetText(s_settings.msg);
        }
	}
}
