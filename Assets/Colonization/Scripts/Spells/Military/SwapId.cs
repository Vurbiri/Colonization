using System.Collections;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class SwapId : ASpell
        {
            private readonly WaitResultSource<Hexagon> _waitHexagon = new();
            private readonly Id<MBButtonId>[] _buttons = { MBButtonId.Cancel };
            private WaitButton _waitButton;
            private Coroutine _coroutine;
            private Unsubscription _unsubscription;
            private Hexagon _selectedA;
            private string _text;
            private int _currentPlayer;

            private SwapId() 
            {
                Localization.Instance.Subscribe(SetText);
            }
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

                if (_coroutine == null)
                {
                    _currentPlayer = param.playerId;
                    _coroutine = Cast_Cn(resources).Run();
                }
               
            }

            public override void Clear()
            {
                Localization.Instance.Unsubscribe(SetText);
                s_spells[TypeOfPerksId.Military][MilitarySpellId.SwapId] = null;
            }

            private IEnumerator Cast_Cn(CurrenciesLite resources)
            {
                s_isCast.True();

                if (_currentPlayer == PlayerId.Person)
                {
                    foreach (var actor in s_actors[_currentPlayer])
                        actor.SetHexagonSelectableForSwap();

                    _waitButton = MessageBox.Open(_text, _buttons);
                    _waitButton.AddListener(Cancel);
                }

                _unsubscription = s_triggerBus.EventHexagonSelect.Add(hexagon => _waitHexagon.SetResult(hexagon));

                yield return _waitHexagon.Restart();
                _selectedA = _waitHexagon.Value;
                _selectedA.SetSelectedForSwap(s_settings.swapHexColor);

                yield return _waitHexagon.Restart();
                _unsubscription.Unsubscribe();

                Swap(_currentPlayer, _waitHexagon.Value, resources);
                EndCast();
            }

            private void Cancel(Id<MBButtonId> id)
            {
                _coroutine.Stop();
                EndCast();
            }

            private void EndCast()
            {
                if (_currentPlayer == PlayerId.Person)
                {
                    _waitButton.Reset();
                    foreach (var actor in s_actors[_currentPlayer])
                        actor.SetHexagonUnselectableForSwap();
                }

                _coroutine = null;
                _selectedA = null;
                s_isCast.False();
            }

            private void Swap(int playerId, Hexagon selectedB, CurrenciesLite resources)
            {
                s_hexagons.SwapId(_selectedA, selectedB, s_settings.swapHexColor);
                s_humans[playerId].AddResources(resources);
                _selectedA = null;
            }

            private void SetText(Localization localization) => _text = localization.GetText(s_settings.swapText);
        }
    }
}
