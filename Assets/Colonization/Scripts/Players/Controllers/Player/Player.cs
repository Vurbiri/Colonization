using System;
using System.Collections;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public abstract class Player : IDisposable
    {
        protected static readonly WaitRealtime s_delayHalfSecond = new(0.5f);

        protected static RInt s_shrinesCount = new();
        public static ReactiveValue<int> ShrinesCount { [Impl(256)] get => s_shrinesCount; }

        protected readonly Id<PlayerId> _id;
        protected readonly bool _isPerson;
        protected readonly PlayerInteractable _interactable;
        protected Subscription _subscription;

        protected readonly WaitAll _waitAll;
        protected Coroutine _coroutine;

        public Id<PlayerId> Id { [Impl(256)] get => _id; }
        public bool IsPerson { [Impl(256)] get => _isPerson; }
        public ReadOnlyReactiveSet<Actor> Actors { [Impl(256)] get => GameContainer.Actors[_id]; }
        public ReactiveValue<bool> Interactable { [Impl(256)] get => _interactable; }

        protected Player(Id<PlayerId> playerId, bool isPerson)
        {
            _id = playerId;
            _isPerson = isPerson;
            _waitAll = new(GameContainer.Shared);
            _interactable = new(playerId, _subscription);
        }

        #region ---------------- Diplomacy ----------------
        [Impl(256)] public bool IsFriend(Id<PlayerId> id) => GameContainer.Diplomacy.IsFriend(_id, id);
        [Impl(256)] public bool IsGreatFriend(Id<PlayerId> id) => GameContainer.Diplomacy.IsGreatFriend(_id, id);
        [Impl(256)] public bool IsEnemy(Id<PlayerId> id) => GameContainer.Diplomacy.IsEnemy(_id, id);
        [Impl(256)] public bool IsGreatEnemy(Id<PlayerId> id) => GameContainer.Diplomacy.IsGreatEnemy(_id, id);
        #endregion

        [Impl(256)] protected Coroutine StartCoroutine(IEnumerator routine) => GameContainer.Shared.StartCoroutine(routine);
        [Impl(256)] protected void StopCoroutine(Coroutine coroutine) => GameContainer.Shared.StopCoroutine(coroutine);

        public void OnGameOver()
        {
            _waitAll?.Stop();
            if(_coroutine != null)
                StopCoroutine(_coroutine);
        }

        public virtual void Dispose()
        {
            OnGameOver();
            _subscription?.Dispose();
        }

        public static void Reset()
        {
            s_shrinesCount.UnsubscribeAll();
            s_shrinesCount.Value = 0;
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public BuffsScriptable artefact;
            [Space]
            public HumanAbilitiesScriptable humanAbilities;
            public RoadFactory roadFactory;
            [Space]
            public BuffsScriptable satanLeveling;
            public void Dispose()
            {
                humanAbilities.Dispose(); satanLeveling.Dispose(); artefact.Dispose();
                humanAbilities = null; satanLeveling = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory ??= new();
                roadFactory.OnValidate();

                EUtility.SetScriptable(ref humanAbilities);
                EUtility.SetScriptable(ref satanLeveling, "Satan Leveling");
                EUtility.SetScriptable(ref artefact, "Artefact");
            }
#endif
        }
        #endregion
    }
}
