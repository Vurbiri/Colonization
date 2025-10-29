using System;
using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract class AI<T> : IEquatable<Actor> where T : AStates
        {
            protected readonly Actor _actor;
            protected readonly T _action;

            protected AI(Actor actor)
            {
                _actor = actor;
                _action = (T)actor._states;
            }

            public bool Equals(Actor actor) => _actor == actor;
            public static bool Equals(AI<T> self, Actor actor) => self._actor == actor;

            [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => _actor.StartCoroutine(routine);
            [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => _actor.StopCoroutine(coroutine);
        }
    }
}
