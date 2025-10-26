using System.Collections;
using System.Text;
using Vurbiri.International;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class AIController
	{
		sealed private class Diplomat : Counselor
        {
            private static readonly DiplomatSettings s_settings;

            private readonly WaitResultSource<bool> _waitGift = new();
            private readonly MainCurrencies _gift = new(), _clone = new();
            public int _ratio;
            private string _msg;

            static Diplomat() => s_settings = SettingsFile.Load<DiplomatSettings>();

            public Diplomat(AIController parent) : base(parent)
			{
				parent._subscription += Localization.Instance.Subscribe(SetMsg);
                _ratio = s_settings.ratio;
            }

            [Impl(256)] public void Update() => _ratio = s_settings.ratio;

            public WaitResult<bool> Receive(int giver, MainCurrencies gift)
			{
                int amount = gift.Amount * _ratio;
                if (GameContainer.Diplomacy.IsGreatFriend(HumanId, giver))
                    amount <<= 1;
                else if (GameContainer.Diplomacy.IsGreatEnemy(HumanId, giver))
                    amount >>= 1;

                bool result = amount > Resources.Amount;
                if (result)
                {
                    Resources.Add(gift);
                    GameContainer.Diplomacy.Gift(HumanId, giver);
                }

                return _waitGift.SetResult(result);
            }

            public override IEnumerator Execution_Cn()
            {
                for (Id<PlayerId> id = PlayerId.None; id.Next();)
                    yield return TryGive_Cn(id);

                // ===== Local =====
                IEnumerator TryGive_Cn(Id<PlayerId> receiver)
                {
                    int amount = Resources.Amount;
                    if (receiver != HumanId & amount > s_settings.minAmount)
                    {
                        if (Chance.Rolling((GameContainer.Diplomacy[receiver, HumanId] - s_settings.relationOffset) + (amount - MaxResources << s_settings.shiftMax)))
                        {
                            _gift.Clear(); _clone.Import(Resources);
                            int countGift = 1 + UnityEngine.Random.Range(0, amount - s_settings.minAmount >> s_settings.shiftAmount);
                            for (int i = 0, max; i < countGift; i++)
                            {
                                max = _clone.MaxIndex;
                                _clone.Decrement(max);
                                _gift.Increment(max);
                            }

                            string msg = null;
                            if (receiver == PlayerId.Person)
                            {
                                StringBuilder sb = new(TAG.ALING_CENTER, 256);
                                sb.Append(GameContainer.UI.PlayerNames[HumanId]); sb.Append(" "); sb.AppendLine(_msg);
                                _gift.PlusToStringBuilder(sb); sb.Append(TAG.ALING_OFF);
                                msg = sb.ToString();
                            }
                            var wait = GameContainer.Humans[receiver].OnGift(HumanId, _gift, msg);
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
