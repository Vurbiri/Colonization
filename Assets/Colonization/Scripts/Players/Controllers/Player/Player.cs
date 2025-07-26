using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public abstract class Player : IDisposable
    {
        protected readonly int _id;
        protected readonly bool _isPerson;

        protected readonly ReactiveSet<Actor> _actors;

        public int Id => _id;
        public ReadOnlyReactiveSet<Actor> Actors => _actors;

        protected Player(int playerId, int actorCapacity)
        {
            _id = playerId;
            _isPerson = playerId == PlayerId.Person;

            _actors = new(actorCapacity);
        }

        public static void Init()  => SpellBook.Init();
        public static void Clear() => SpellBook.Clear();

        public virtual void Dispose()
        {
            _actors.Dispose();
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public HumanAbilitiesScriptable humanAbilities;
            public PerksScriptable perks;
            public RoadFactory roadFactory;
            [Space]
            public DemonInitializer demonPrefab;
            public BuffsScriptable demonLeveling;
            [Space]
            public BuffsScriptable artefact;
            public Transform actorsContainer;

            public void Dispose()
            {
                humanAbilities.Dispose(); perks.Dispose(); demonLeveling.Dispose(); artefact.Dispose();
                humanAbilities = null; perks = null; demonLeveling = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory ??= new();
                roadFactory.OnValidate();

                EUtility.SetPrefab(ref warriorPrefab);
                EUtility.SetPrefab(ref demonPrefab);

                EUtility.SetScriptable(ref humanAbilities);
                EUtility.SetScriptable(ref perks);
                EUtility.SetScriptable(ref demonLeveling, "DemonLevelingSettings");
                EUtility.SetScriptable(ref artefact, "ArtefactSettings");

                EUtility.SetObject(ref actorsContainer, "Actors");
            }
#endif
        }
        #endregion
    }
}
