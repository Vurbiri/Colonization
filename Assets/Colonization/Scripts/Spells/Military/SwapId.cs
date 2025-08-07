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
            private readonly string _msgKey, _strCost;
            private string _strMsg;
            private WaitButton _waitButton;
            private Coroutine _coroutine;
            private Hexagon _selectedA;
            private int _currentPlayer;

            private SwapId(int type, int id) : base(type, id)
            {
                _msgKey = string.Concat(s_keys[type][id], "Msg");
                _strCost = "\n".Concat(string.Format(TAG.CURRENCY, CurrencyId.Mana, _cost[CurrencyId.Mana]));
            }
            public static void Create() =>  new SwapId(MilitarySpellId.Type, MilitarySpellId.SwapId);

            public override bool Prep(SpellParam param)
            {
                var human = s_humans[param.playerId];
                return _canCast = !s_isCast && human.IsPay(_cost) & human.Actors.Count >= 2 & _coroutine == null;
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

                    _waitButton = MessageBox.Open(_strMsg, MBButton.Cancel);
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
                    _waitButton.Reset(); _waitButton = null;
                    foreach (var actor in s_actors[PlayerId.Person])
                        actor.SetHexagonUnselectable();
                }

                _coroutine = null; _selectedA = null; 
                _currentPlayer = PlayerId.None;
                s_isCast.False();
            }

            private void SetHexagon(Hexagon hexagon) => _waitHexagon.SetResult(hexagon);

            protected override string GetDesc(Localization localization)
            {
                _strMsg = string.Concat(TAG.ALING_CENTER, _strName, "\n", localization.GetText(FILE, _msgKey), TAG.ALING_OFF);

                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.sacrificeHPPercent, s_settings.sacrificePierce), _strCost);
            }
        }
    }
}
