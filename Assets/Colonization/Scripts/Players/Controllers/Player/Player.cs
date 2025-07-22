using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
	public abstract class Player
    {
		protected static GameStates s_states;
        protected static Hexagons s_hexagons;
        protected static Crossroads s_crossroads;
        protected static GameLoop s_game;
        protected static CameraController s_cameraController;
        
        protected readonly int _id;
        protected readonly bool _isPerson;

        public static GameStates States => s_states;

        protected Player(int playerId)
        {
            _id = playerId;
            _isPerson = playerId == PlayerId.Person;
        }

        public static void Clear()
        {
            SpellBook.Clear(); Actor.Clear();
            s_hexagons = null; s_crossroads = null; s_game = null;
            s_cameraController = null;
            s_states.Clear();
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

            [NonSerialized] public GameplayStorage storage;
            [NonSerialized] public GameplayTriggerBus triggerBus;

            public Settings Init(GameplayContent init)
            {
                s_states.score = new Score(init.storage);
                s_states.balance = new Balance(init.storage, init.gameLoop);
                s_states.diplomacy = new Diplomacy(init.storage, init.gameLoop);
                s_states.prices = init.prices;

                s_hexagons = init.hexagons;
                s_crossroads = init.crossroads;
                s_game = init.gameLoop;
                s_cameraController = init.cameraController;

                SpellBook.Init(init);
                Actor.Init(s_states.diplomacy, init.triggerBus);
                roadFactory.Init(init.sharedRepository);

                storage = init.storage;
                triggerBus = init.triggerBus;

                return this;
            }

            public void Dispose()
            {
                humanAbilities.Dispose(); perks.Dispose(); demonLeveling.Dispose(); artefact.Dispose();
                humanAbilities = null; perks = null; demonLeveling = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory?.OnValidate();

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
