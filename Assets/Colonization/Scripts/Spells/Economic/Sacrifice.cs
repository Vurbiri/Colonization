using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;
using static Vurbiri.Colonization.Actor;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        sealed private class Sacrifice : AMsgSpell
        {
            private readonly WaitResultSource<Actor> _waitActor = new();
            private readonly SpellDamager _damage;
            private WaitButton _waitButton;
            private Actor _target;
            private Coroutine _coroutine;
            private int _currentPlayer;

            private Sacrifice(int type, int id) : base(type, id) 
            {
                _damage = new(s_settings.sacrificePierce);

                _strCost = _cost.PlusToString(SEPARATOR);
            }
            public static void Create() => new Sacrifice(EconomicSpellId.Type, EconomicSpellId.Sacrifice);

            public override bool Prep(SpellParam param)
            {
                var allActors = GameContainer.Actors;

                _target = null;
                if (!s_isCasting && allActors[param.playerId].Count > 0 && Humans[param.playerId].IsPay(_cost))
                {
                    var actors = allActors[PlayerId.Satan];
                    if (actors.Count > 0)
                    {
                        _target = actors.Random;
                    }
                    else
                    {
                        for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                        {
                            actors = allActors[playerId];
                            if (actors.Count > 0 & GameContainer.Diplomacy.IsEnemy(param.playerId, playerId))
                            {
                                if(_target == null || Chance.Rolling())
                                    _target = actors.Random;
                            }
                        }
                    }
                }
                return _canCast = _target != null; ;
            }

            public override void Cast(SpellParam param)
            {
                if (_canCast)
                {
                    s_isCasting.True();
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
                        actor.Hexagon.ShowMark(false);
                        CameraController.ToPosition(actor.Position, true);
                    }
                    else
                    {
                        foreach (var actor in actors)
                            actor.Hexagon.ShowMark(false);
                    }

                    _waitButton = MessageBox.Open(_strMsg, MBButton.Cancel);
                    _waitButton.AddListener(Cancel);
                }
                else
                {
                    Banner.Open(_strName, MessageTypeId.Warning, 6f);
                }

                EventBus.EventActorLeftSelect.Add(SetActor);
                yield return _waitActor.Restart();

                EndSelect();

                var sacrifice = _waitActor.Value;
                _damage.attack = sacrifice.CurrentHP * s_settings.sacrificeHPPercent / 100;

                yield return CameraController.ToPositionControlled(sacrifice);
                yield return SFX.Run(s_settings.sacrificeKnifeSFX, null, sacrifice.Skin);
                yield return sacrifice.Action.Death().SetWaitState(DeathStage.EndAnimation);

                yield return CameraController.ToPositionControlled(_target);
                _damage.Apply(_target); Humans[_currentPlayer].Pay(_cost);
                yield return SFX.Run(s_settings.sacrificeTargetSFX, null, _target.Skin);

                EndCast();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void EndSelect()
            {
                EventBus.EventActorLeftSelect.Remove(SetActor);
                GameContainer.InputController.Unselect();

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
                _coroutine = null; _target = null; _waitButton = null;
                _currentPlayer = PlayerId.None;

                s_isCasting.False();
            }

            private void SetActor(Actor actor) => _waitActor.SetResult(actor);

            protected override string GetDesc(Localization localization)
            {
                SetMsg(localization);

                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.sacrificeHPPercent, s_settings.sacrificePierce), _strCost);
            }
        }
    }
}
