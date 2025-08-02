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
        sealed private class Zeal : ASpell
        {
            private readonly WaitResultSource<Actor> _waitActor = new();
            private readonly Effect _heal, _addAP, _move;
            private WaitButton _waitButton;
            private Coroutine _coroutine;
            private string _text;
            private int _currentPlayer;

            private Zeal(int type, int id) : base(type, id)
            {
				_heal  = new(ActorAbilityId.CurrentHP, TypeModifierId.TotalPercent, s_settings.zealPercentHeal);
                _addAP = new(ActorAbilityId.CurrentAP, TypeModifierId.Addition,     s_settings.zealAddAP);
                _move  = new(ActorAbilityId.IsMove,    TypeModifierId.Addition,     1);

                Localization.Instance.Subscribe(SetText);
            }
            public static void Create() => new Zeal(MilitarySpellId.Type, MilitarySpellId.Zeal);

            public override bool Prep(SpellParam param)
            {
                var human = s_humans[param.playerId];
                return _canCast = human.IsPay(_cost) & human.Actors.Count > 0 & _coroutine == null;
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
                    var actors = s_actors[PlayerId.Person];
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

                    _waitButton = MessageBox.Open(_text, MBButton.Cancel);
                    _waitButton.AddListener(Cancel);
                }

                EventBus.EventActorSelect.Add(SetActor);
                yield return _waitActor.Restart();

                EndSelect();

                var target = _waitActor.Value;

                yield return CameraController.ToPosition(target.Position, true);

                target.ApplyEffect(_heal); target.ApplyEffect(_addAP); target.ApplyEffect(_move);
                s_humans[_currentPlayer].Pay(_cost);

                yield return HitSFX.Hit(s_settings.zealSFX, s_sfxUser, target.Skin);

                EndCast();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void EndSelect()
            {
                EventBus.EventActorSelect.Remove(SetActor);
                if (_currentPlayer == PlayerId.Person)
                {
                    _waitButton.Reset();
                    foreach (var actor in s_actors[PlayerId.Person])
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

            private void SetText(Localization localization) => _text = localization.GetText(s_settings.zealText);
            private void SetActor(Actor actor) => _waitActor.SetResult(actor);
        }
	}
}
