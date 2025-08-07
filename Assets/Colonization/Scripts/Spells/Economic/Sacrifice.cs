using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;
using Vurbiri.UI;
using static Vurbiri.Colonization.Actors.Actor;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        public class Sacrifice : ASpell
        {
            private readonly WaitResultSource<Actor> _waitActor = new();
            private readonly string _msgKey, _strCost;
            private string _strMsg;
            private readonly SpellDamager _damage;
            private WaitButton _waitButton;
            private Actor _target;
            private Coroutine _coroutine;
            private int _currentPlayer;

            private Sacrifice(int type, int id) : base(type, id) 
            {
                _cost.Set(CurrencyId.Blood, s_settings.sacrificeBloodCost);
                _damage = new(s_settings.sacrificePierce);

                _msgKey = string.Concat(s_keys[type][id], "Msg");
                _strCost = _cost.PlusToString();
            }
            public static void Create() => new Sacrifice(EconomicSpellId.Type, EconomicSpellId.Sacrifice);

            public override bool Prep(SpellParam param)
            {
                _canCast = false;
                if (!s_isCast & _coroutine == null && s_actors[param.playerId].Count > 0 && s_humans[param.playerId].IsPay(_cost))
                {
                    _target = null;
                    var actors = s_actors[PlayerId.Satan];
                    if (actors.Count > 0)
                    {
                        _target = actors.Random;
                    }
                    else
                    {
                        for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                        {
                            actors = s_actors[playerId];
                            if (actors.Count > 0 & GameContainer.Diplomacy.GetRelation(param.playerId, playerId) == Relation.Enemy)
                            {
                                if(_target == null || Chance.Rolling())
                                    _target = actors.Random;
                            }
                        }
                    }
                    _canCast = _target != null;
                }
                return _canCast;
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
                    var actors = s_actors[PlayerId.Person];
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

                EventBus.EventActorSelect.Add(SetActor);
                yield return _waitActor.Restart();

                EndSelect();

                var sacrifice = _waitActor.Value;
                _damage.attack = sacrifice.CurrentHP * s_settings.sacrificeHPPercent / 100;

                CameraController.ToPosition(sacrifice.Position, true);
                yield return HitSFX.Hit(s_settings.sacrificeKnifeSFX, s_sfxUser, sacrifice.Skin);
                yield return sacrifice.Death().SetWaitState(DeathState.Animation);

                yield return CameraController.ToPosition(_target.Position, true);
                _damage.Apply(_target); s_humans[_currentPlayer].Pay(_cost);
                yield return HitSFX.Hit(s_settings.sacrificeTargetSFX, s_sfxUser, _target.Skin);

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
                _coroutine = null; _target = null; _waitButton = null;
                _currentPlayer = PlayerId.None;

                s_isCast.False();
            }

            private void SetActor(Actor actor) => _waitActor.SetResult(actor);

            protected override string GetDesc(Localization localization)
            {
                _strMsg = string.Concat(TAG.ALING_CENTER, _strName, "\n", localization.GetText(FILE, _msgKey), TAG.ALING_OFF);

                return string.Concat(localization.GetFormatText(FILE, _descKey, s_settings.sacrificeHPPercent, s_settings.sacrificePierce), _strCost);
            }
        }
    }
}
