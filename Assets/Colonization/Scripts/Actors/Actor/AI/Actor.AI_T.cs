using System;
using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TSettings, TActorId, TStateId> : AI, IEquatable<Actor>, IDisposable
            where TSettings : ActorsAISettings<TActorId, TStateId> where TActorId : ActorId<TActorId> where TStateId : ActorAIStateId<TStateId>
        {
            protected static readonly WaitFrames s_waitBeforeSelecting = new(10);
            protected static readonly TSettings s_settings;

            static AI()
            {
                s_settings = SettingsFile.Load<TSettings>();
                s_settings.Init();
            }

            private readonly Status _status;
            private readonly Actor _actor;
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
            }

            public IEnumerator Execution_Cn()
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

            public void Dispose() => _current.Dispose();

            protected abstract State[] GetStates();

            protected static void StatesSort(State[] states)
            {
                var priority = s_settings.Priority;

                for (int i = states.Length - 1, j; i > 0;)
                {
                    j = priority.IndexOf(states[i].Id);
                    if (i != j)
                        (states[i], states[j]) = (states[j], states[i]);
                    else
                        --i;
                }
            }

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _actor.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => _actor.StopCoroutine(coroutine);

            public bool Equals(Actor actor) => _actor == actor;
        }
    }
}
