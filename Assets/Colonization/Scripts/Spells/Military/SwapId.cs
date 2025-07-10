using System.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SwapId : ASharedSpell
        {
            private readonly WaitResultSource<Hexagon> _waitHexagon = new();
            private readonly CurrenciesLite _cost = new();
            private Unsubscription _unsubscription;

            private SwapId() 
            {
                _cost.Add(CurrencyId.Mana, -s_costs[TypeOfPerksId.Military][MilitarySpellId.SwapId]);
            }
            public static void Create() => s_sharedSpells[TypeOfPerksId.Military][MilitarySpellId.SwapId] = new SwapId();

            public override bool Cast(SpellParam param, CurrenciesLite resources)
            {
                if (s_actors[param.playerId].Count < 2)
                    return false;

                if(param.playerId == PlayerId.Person)
                {
                    foreach (var actor in s_actors[param.playerId])
                        actor.SetHexagonSelectableForSwap();
                }

                s_coroutines.Run(Cast_Cn(param.playerId));

                _waitHexagon.Cancel();
                _unsubscription = s_triggerBus.EventHexagonSelect.Add(hexagon => _waitHexagon.SetResult(hexagon));

                return false;
            }

            private IEnumerator Cast_Cn(int playerId)
            {
                Hexagon selectedA, selectedB;

                yield return _waitHexagon;
                selectedA = _waitHexagon.Value;
                selectedA.SetSelectedForSwap(s_settings.swapHexColor);

                _waitHexagon.Cancel();
                yield return _waitHexagon;
                selectedB = _waitHexagon.Value;

                _unsubscription.Unsubscribe();
            }

        }
    }
}
