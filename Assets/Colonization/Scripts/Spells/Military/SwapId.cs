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

            private SwapId(int type, int id) : base(type, id)
            {
                Localization.Instance.Subscribe(SetText);
            }
            public static void Create() =>  new SwapId(TypeOfPerksId.Military, MilitarySpellId.SwapId);

            public override bool Prep(SpellParam param)
            {
                var human = s_humans[param.playerId];
                return _canCast = human.IsPay(_cost) & human.Actors.Count >= 2 & _coroutine == null;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    if (s_actors[param.playerId].Count == 2)
                    {
                        foreach (var actor in s_actors[param.playerId])
                        {
                            if (_selectedA == null)
                                _selectedA = actor.Hexagon;
                            else
                                Swap(param.playerId, actor.Hexagon);
                        }
                    }
                    else
                    {
                        _currentPlayer = param.playerId;
                        _coroutine = Cast_Cn().Start();
                    }
                    _canCast = false;
                }
            }

            public override void Clear(int type, int id)
            {
                Localization.Instance.Unsubscribe(SetText);
                s_spells[type][id] = null;
            }

            private IEnumerator Cast_Cn()
            {
                s_isCast.True();

                if (_currentPlayer == PlayerId.Person)
                {
                    foreach (var actor in s_actors[_currentPlayer])
                        actor.SetHexagonSelectableForSwap();

                    _waitButton = MessageBox.Open(_text, _buttons);
                    _waitButton.AddListener(Cancel);
                }

                _unsubscription = GameContainer.EventBus.EventHexagonSelect.Add(hexagon => _waitHexagon.SetResult(hexagon));

                yield return _waitHexagon.Restart();
                _selectedA = _waitHexagon.Value;
                _selectedA.SetSelectedForSwap(s_settings.swapHexColor);

                yield return _waitHexagon.Restart();

                Swap(_currentPlayer, _waitHexagon.Value);
                EndCast();
            }

            private void Cancel(Id<MBButtonId> id)
            {
                _coroutine.Stop();
                EndCast();
            }

            private void EndCast()
            {
                _unsubscription?.Unsubscribe();
                _unsubscription = null;

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

            private void Swap(int playerId, Hexagon selectedB)
            {
                GameContainer.Hexagons.SwapId(_selectedA, selectedB, s_settings.swapHexColor);
                s_humans[playerId].Pay(_cost);
                _selectedA = null;
            }

            private void SetText(Localization localization) => _text = localization.GetText(s_settings.swapText);
        }
    }
}
