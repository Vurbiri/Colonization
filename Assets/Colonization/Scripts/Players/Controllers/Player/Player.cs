using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public abstract class Player : IDisposable
    {
        protected readonly int _id;
        protected readonly bool _isPerson;
        protected readonly RBool _interactable = new(false);
        protected Subscription _subscription;

        public int Id => _id;
        public ReadOnlyReactiveSet<Actor> Actors => GameContainer.Actors[_id];
        public ReactiveValue<bool> Interactable => _interactable;

        protected Player(int playerId)
        {
            _id = playerId;
            _isPerson = playerId == PlayerId.Person;
        }

        public virtual void Dispose()
        {
            _subscription?.Dispose();
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public BuffsScriptable artefact;
            [Space]
            public HumanAbilitiesScriptable humanAbilities;
            public PerksScriptable perks;
            public RoadFactory roadFactory;
            [Space]
            public BuffsScriptable satanLeveling;
            public void Dispose()
            {
                humanAbilities.Dispose(); perks.Dispose(); satanLeveling.Dispose(); artefact.Dispose();
                humanAbilities = null; perks = null; satanLeveling = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory ??= new();
                roadFactory.OnValidate();

                EUtility.SetScriptable(ref humanAbilities);
                EUtility.SetScriptable(ref perks);
                EUtility.SetScriptable(ref satanLeveling, "Satan Leveling");
                EUtility.SetScriptable(ref artefact, "Artefact");
            }
#endif
        }
        #endregion
    }
}
