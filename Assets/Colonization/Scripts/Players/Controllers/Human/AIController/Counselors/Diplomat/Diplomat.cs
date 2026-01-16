using System.Collections;
using System.Text;
using Vurbiri.Collections;
using Vurbiri.International;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class AIController
	{
		sealed private class Diplomat : Counselor
        {
            private static readonly DiplomatSettings s_settings;

            private readonly WaitResultSource<bool> _waitGift = new();
            private string _giftMsg, _colonyMsg, _portMsg;
            private int _ratio;
            
            static Diplomat() => s_settings = SettingsFile.Load<DiplomatSettings>();

            public Diplomat(AIController parent) : base(parent)
			{
                _ratio = s_settings.ratio;
                Localization.Subscribe(SetText);
            }

            [Impl(256)] public void Update() => _ratio = s_settings.ratio;

            public WaitResult<bool> Receive(int giver, LiteCurrencies gift)
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

                return _waitGift.Return(result);
            }

            public override IEnumerator Execution_Cn()
            {
                for (Id<PlayerId> id = PlayerId.None; id.Next(PlayerId.HumansCount);)
                    yield return TryGive_Cn(id);

#if TEST_AI
                UnityEngine.Debug.Log($"[Diplomat] {HumanId}");
                if (HumanId == PlayerId.Person) yield break;
#endif
                int relation = GameContainer.Diplomacy[HumanId.Value, PlayerId.Person];
                if (relation > 0)
                {
                    ColoniesCheck(Colonies, s_settings.colonyRelationOffset - relation, s_settings.colonyPenalty, _colonyMsg);
                    ColoniesCheck(Ports, s_settings.portRelationOffset - relation, s_settings.portPenalty, _portMsg);
                }

                #region Local ColoniesCheck(..), IsOccupation(..)
                // =============================================================================================
                [Impl(256)] void ColoniesCheck(ReadOnlyReactiveList<Crossroad> colonies, int chance, int penalty, string msg)
                {
                    if (chance > 0)
                        for (int i = 0; i < colonies.Count; ++i)
                            if (IsOccupation(colonies[i].Hexagons, chance, penalty, msg))
                                break;
                }
                // =============================================================================================
                bool IsOccupation(ReadOnlyArray<Hexagon> hexagons, int chance, int penalty, string msg)
                {
                    Hexagon hexagon;
                    for (int i = 0; i < Crossroad.HEX_COUNT; ++i)
                    {
                        hexagon = hexagons[i];
                        if (hexagon.OwnerId == PlayerId.Person && !hexagon.Owner.IsInCombat() && Chance.Rolling(chance >> (hexagon.Owner.IsGuard() ? 1 : 0)))
                        {
                            Banner.Open(msg, MessageTypeId.Error, s_settings.bannerTime, true);
                            GameContainer.Diplomacy.Occupation(HumanId.Value, PlayerId.Person, penalty);
                            return true;
                        }
                    }
                    return false;
                }
                // =============================================================================================
                #endregion
            }

            public override void Dispose() => Localization.Unsubscribe(SetText);

            private void SetText(Localization localization)
            {
                var name = GameContainer.UI.PlayerNames[HumanId];

                _giftMsg   = string.Format(localization.GetText(s_settings.giftMsg), name);
                _colonyMsg = string.Format(localization.GetText(s_settings.colonyMsg), name);
                _portMsg   = string.Format(localization.GetText(s_settings.portMsg), name);
            }

            private IEnumerator TryGive_Cn(Id<PlayerId> receiver)
            {
                int amount = Resources.Amount;
                if (receiver != HumanId & amount > s_settings.minAmount)
                {
                    if (Chance.Rolling((GameContainer.Diplomacy[receiver, HumanId] - s_settings.relationOffset) + (amount - MaxResources << s_settings.shiftMax)))
                    {
                        LiteCurrencies gift = new(), clone = new(Resources);

                        int countGift = 1 + UnityEngine.Random.Range(0, amount - s_settings.minAmount >> s_settings.shiftAmount);
                        for (int i = 0, max; i < countGift; i++)
                        {
                            max = clone.MaxIndex;
                            clone.Decrement(max);
                            gift.Increment(max);
                        }

                        string msg = null;
                        if (receiver == PlayerId.Person)
                        {
                            StringBuilder sb = new(256);
                            sb.AppendLine(_giftMsg);
                            gift.PlusToStringBuilder(sb);
                            msg = sb.ToString();
                        }
                        var wait = GameContainer.Humans[receiver].OnGift(HumanId, gift, msg);
                        yield return wait;
                        if (wait) Resources.Remove(gift);
                    }
                }

                yield break;
            }
        }
	}
}
