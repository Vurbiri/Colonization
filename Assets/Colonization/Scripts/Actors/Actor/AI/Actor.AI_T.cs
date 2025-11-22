using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TSettings, TActorId, TStateId> : AI, IEquatable<Actor>
            where TSettings : ActorsAISettings<TActorId, TStateId> where TActorId : ActorId<TActorId> where TStateId : ActorAIStateId<TStateId>
        {
            protected static readonly TSettings s_settings;
            private static readonly Func<AI<TSettings, TActorId, TStateId>, State>[] s_factories = new Func<AI<TSettings, TActorId, TStateId>, State>[ActorAIStateId<TStateId>.Count];

            static AI()
            {
                s_settings = SettingsFile.Load<TSettings>();
                s_settings.Init();
            }

            protected readonly Actor _actor;
            private readonly Status _status;
            private readonly Goals _goals;
            private readonly ActorAISettings _aISettings;
            private readonly State _goalSetting;
            private State _current;

            protected AI(Actor actor, Goals goals)
            {
                _status = new();
                _actor = actor;
                _goals = goals;
                _aISettings = s_settings[_actor._id];
                _current = _goalSetting = new GoalSetting(this, GetStates());

                // ========= Local ==========
                State[] GetStates()
                {
                    State[] states = new State[ActorAIStateId<TStateId>.Count];
                    for (int i = 0; i < ActorAIStateId<TStateId>.Count; ++i)
                        states[i] = s_factories[i](this);

                    return states;
                }
            }

            sealed public override IEnumerator Execution_Cn()
            {
                int key;
                do
                {
#if TEST_AI
                    Log.Info($"[{ActorTypeId.GetName(_actor)}AI_{_actor.Index}] {_actor.Owner} state [{_current}]");
#endif
                    _status.Update(_actor);
                    yield return StartCoroutine(_current.Execution_Cn(Out<bool>.Get(out key)));
                    _status.Clear();
                }
                while (Out<bool>.Result(key));
            }

            sealed public override void Dispose() => _current.Dispose();

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _actor.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => _actor.StopCoroutine(coroutine);

            public bool Equals(Actor actor) => _actor == actor;

            protected static void SetFactories(ReadOnlyIdArray<TStateId, Func<AI<TSettings, TActorId, TStateId>, State>> factories)
            {
#if TEST_AI
                // Validate
                for (int i = 0; i < ActorAIStateId<TStateId>.Count; ++i)
                    if (factories[i] == null)
                        UnityEngine.Debug.LogError($"Factory {ActorAIStateId<TStateId>.Names_Ed[i]} = null)");
#endif
                var priority = s_settings.GetPriority();

                for (int i = 0; i < ActorAIStateId<TStateId>.Count; ++i)
                    s_factories[i] = factories[priority[i]];
            }

            protected static State GetEscapeSupport(AI<TSettings, TActorId, TStateId> parent) => new EscapeSupport(parent);
            protected static State GetCombat(AI<TSettings, TActorId, TStateId> parent)        => new Combat(parent);
            protected static State GetSupport(AI<TSettings, TActorId, TStateId> parent)       => new Support(parent);
            protected static State GetMoveToHelp(AI<TSettings, TActorId, TStateId> parent)    => new MoveToHelp(parent);
            protected static State GetDefense(AI<TSettings, TActorId, TStateId> parent)       => new Defense(parent);
            protected static State GetMoveToRaid(AI<TSettings, TActorId, TStateId> parent)    => new MoveToRaid(parent);
        }
    }
}
