using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using Vurbiri.UI;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization
{
	public partial class SpellBook
	{
        sealed private class Zeal : AMsgSpell
        {
            private readonly WaitResultSource<Actor> _waitActor = new();
            private readonly Effect _addAP, _move;
            private WaitButton _waitButton;
            private Coroutine _coroutine;
            private int _currentPlayer;

            private Zeal(int type, int id) : base(type, id)
            {
                _addAP = new(ActorAbilityId.CurrentAP, TypeModifierId.Addition, s_settings.zealAddAP);
                _move  = new(ActorAbilityId.IsMove,    TypeModifierId.Addition, 1);

                SetManaCost();
            }
            public static void Create() => new Zeal(MilitarySpellId.Type, MilitarySpellId.Zeal);

            public override bool Prep(SpellParam param)
            {
                var human = Humans[param.playerId];
                return _canCast = !s_isCast && human.IsPay(_cost) & human.Actors.Count > 0 & _coroutine == null;
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
                    _coroutine.Stop();
                    EndSelect();
                    EndCast();
                }
            }
            private void Cancel(Id<MBButtonId> id) => Cancel();

            private IEnumerator Cast_Cn()
            {
                if (_currentPlayer == PlayerId.Person)
                {
                    var actors = GameContainer.Actors[PlayerId.Person];
                    if (actors.Count == 1)
                    {
                        var actor = actors.First;
                        actor.Hexagon.ShowMark(true);
                        CameraController.ToPosition(actor.Position, true);
                    }
                    else
                    {
                        foreach (var actor in actors)
                            actor.Hexagon.ShowMark(true);
                    }

                    _waitButton = MessageBox.Open(_strMsg, MBButton.Cancel);
                    _waitButton.AddListener(Cancel);
                }
                else
                {
                    Banner.Open(_strName, MessageTypeId.Warning, 6f);
                }

                EventBus.EventActorSelect.Add(SetActor);
                yield return _waitActor.Restart();

                EndSelect();

                var target = _waitActor.Value;

                yield return CameraController.ToPosition(target.Position, true);

                target.ApplyEffect(_addAP); target.ApplyEffect(_move);
                Humans[_currentPlayer].Pay(_cost);

                yield return HitSFX.Hit(s_settings.zealSFX, null, target.Skin);

                EndCast();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void EndSelect()
            {
                EventBus.EventActorSelect.Remove(SetActor);
                if (_currentPlayer == PlayerId.Person)
                {
                    _waitButton.Reset();
                    foreach (var actor in GameContainer.Actors[PlayerId.Person])
                        actor.Hexagon.HideMark();
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void EndCast()
            {
                _coroutine = null; _waitButton = null;
                _currentPlayer = PlayerId.None;

                s_isCast.False();
            }

            private void SetActor(Actor actor) => _waitActor.SetResult(actor);

            protected override string GetDesc(Localization localization)
            {
                SetMsg(localization);

                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.zealAddAP), _strCost);
            }
        }
	}
}
