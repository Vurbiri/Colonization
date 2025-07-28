using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;
using static Vurbiri.Colonization.GameContainer;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        public class Sacrifice : ASpell
        {
            
            private readonly WaitResultSource<Actor> _waitActor = new();
            private readonly Id<MBButtonId>[] _buttons = { MBButtonId.Cancel };
            private Actor _target;
            private WaitButton _waitButton;
            private Coroutine _coroutine;
            private Unsubscription _unsubscription;
            private string _text;
            private int _currentPlayer;

            private Sacrifice(int type, int id) : base(type, id) 
            {
                Localization.Instance.Subscribe(SetText);
            }
            public static void Create() => new Sacrifice(EconomicSpellId.Type, EconomicSpellId.Sacrifice);

            public override bool Prep(SpellParam param)
            {
                _canCast = false;
                if (_coroutine == null && s_actors[param.playerId].Count > 0 && s_humans[param.playerId].IsPay(_cost))
                {
                    _target = null;
                    var actors = s_actors[PlayerId.Satan];
                    if (actors.Count > 0)
                    {
                        _target = actors.GetRandom();
                    }
                    else
                    {
                        for (int playerId = 0; playerId < PlayerId.HumansCount; playerId++)
                        {
                            actors = s_actors[playerId];
                            if (actors.Count > 0 & GameContainer.Diplomacy.GetRelation(param.playerId, playerId) == Relation.Enemy)
                            {
                                if(_target == null || Chance.Rolling())
                                    _target = actors.GetRandom();
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

            public override void Clear(int type, int id)
            {
                Localization.Instance.Unsubscribe(SetText);
                s_spells[type][id] = null;
            }

            public override void Cancel()
            {
                _coroutine.Stop();
                //EndCast();
            }
            private void Cancel(Id<MBButtonId> id) => Cancel();

            private IEnumerator Cast_Cn()
            {

                if (_currentPlayer == PlayerId.Person)
                {
                    foreach (var actor in s_actors[PlayerId.Person])
                        actor.Hexagon.ShowMark(false);

                    _waitButton = MessageBox.Open(_text, _buttons);
                    _waitButton.AddListener(Cancel);
                }

                EventBus.EventActorSelect.Add(SetActor);
                yield return _waitActor.Restart();

                if (_currentPlayer == PlayerId.Person)
                {
                    _waitButton.Reset();
                    foreach (var actor in s_actors[PlayerId.Person])
                        actor.Hexagon.HideMark();
                }

                EventBus.EventActorSelect.Remove(SetActor);

                var sacrifice = _waitActor.Value;

                CameraController.ToPosition(sacrifice.Position);
                yield return sacrifice.Death_Cn();
                yield return CameraController.ToPosition(_target.Position);
            }

            private void SetText(Localization localization) => _text = localization.GetText(s_settings.sacrificeText);
            private void SetActor(Actor actor) => _waitActor.SetResult(actor);
        }
    }
}
