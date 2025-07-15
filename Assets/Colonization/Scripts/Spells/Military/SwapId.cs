using System.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SwapId : ASpell
        {
            private readonly WaitResultSource<Hexagon> _waitHexagon = new();
            private Unsubscription _unsubscription;
            private Hexagon _selectedA;

            private SwapId() { }
            public static void Create() => s_spells[TypeOfPerksId.Military][MilitarySpellId.SwapId] = new SwapId();

            public override void Cast(SpellParam param, CurrenciesLite resources)
            {
                if (s_actors[param.playerId].Count < 2)
                    return;

                if (s_actors[param.playerId].Count == 2)
                {
                    foreach (var actor in s_actors[param.playerId])
                    {
                        if(_selectedA == null)
                            _selectedA = actor.Hexagon;
                        else
                            Swap(param.playerId, actor.Hexagon, resources);
                    }
                    return;
                }

                s_coroutines.Run(Cast_Cn(param.playerId, resources));
               
            }

            private IEnumerator Cast_Cn(int playerId, CurrenciesLite resources)
            {
                if (playerId == PlayerId.Person)
                {
                    foreach (var actor in s_actors[playerId])
                        actor.SetHexagonSelectableForSwap();
                }

                _unsubscription = s_triggerBus.EventHexagonSelect.Add(hexagon => _waitHexagon.SetResult(hexagon));

                yield return _waitHexagon.Restart();
                _selectedA = _waitHexagon.Value;
                _selectedA.SetSelectedForSwap(s_settings.swapHexColor);

                yield return _waitHexagon.Restart();
                _unsubscription.Unsubscribe();

                if (playerId == PlayerId.Person)
                {
                    foreach (var actor in s_actors[playerId])
                        actor.SetHexagonUnselectableForSwap();
                }

                Swap(playerId, _waitHexagon.Value, resources);
            }

            private void Swap(int playerId, Hexagon selectedB, CurrenciesLite resources)
            {
                s_hexagons.SwapId(_selectedA, selectedB, s_settings.swapHexColor);
                s_humans[playerId].AddResources(resources);
                _selectedA = null;
            }
        }
    }
}
