using System.Collections;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;
using static Vurbiri.Colonization.GameContainer;

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
            private Hexagon _selectedA;
            private string _text;
            private int _currentPlayer;

            private SwapId(int type, int id) : base(type, id)
            {
                Localization.Instance.Subscribe(SetText);
            }
            public static void Create() =>  new SwapId(MilitarySpellId.Type, MilitarySpellId.SwapId);

            public override bool Prep(SpellParam param)
            {
                var human = s_humans[param.playerId];
                return _canCast = human.IsPay(_cost) & human.Actors.Count >= 2 & _coroutine == null;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_isCast.True();
                    _currentPlayer = param.playerId;
                    _coroutine = Cast_Cn().Start();

                    _canCast = false;
                }
            }

            public override void Clear(int type, int id)
            {
                Localization.Instance.Unsubscribe(SetText);
                s_spells[type][id] = null;
            }

            public override void Cancel()
            {
                if (_coroutine != null)
                {
                    if (_selectedA != null) _selectedA.ResetCaptionColor();
 
                    _coroutine.Stop();
                     EndCast();
                }
            }
            private void Cancel(Id<MBButtonId> id) => Cancel();

            private IEnumerator Cast_Cn()
            {
                

                if (_currentPlayer == PlayerId.Person)
                {
                    foreach (var actor in s_actors[PlayerId.Person])
                        actor.SetHexagonSelectable();

                    _waitButton = MessageBox.Open(_text, _buttons);
                    _waitButton.AddListener(Cancel);
                }

                EventBus.EventHexagonSelect.Add(SetHexagon);
                yield return _waitHexagon.Restart();
                _selectedA = _waitHexagon.Value;
                _selectedA.SetSelectedForSwap(s_settings.swapHexColor);

                yield return _waitHexagon.Restart();

                GameContainer.Hexagons.SwapId(_selectedA, _waitHexagon.Value, s_settings.swapHexColor, s_settings.swapShowTime);
                
                s_humans[_currentPlayer].Pay(_cost);
                EndCast();
            }

            

            private void EndCast()
            {
                EventBus.EventHexagonSelect.Remove(SetHexagon);

                if (_currentPlayer == PlayerId.Person)
                {
                    _waitButton.Reset();
                    foreach (var actor in s_actors[PlayerId.Person])
                        actor.SetHexagonUnselectable();
                }

                _coroutine = null;
                _selectedA = null;
                _currentPlayer = PlayerId.None;
                s_isCast.False();
            }

            private void SetText(Localization localization) => _text = localization.GetText(s_settings.swapText);
            private void SetHexagon(Hexagon hexagon) => _waitHexagon.SetResult(hexagon);
        }
    }
}
