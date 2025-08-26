using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public class ActorsFactory : System.IDisposable
    {
        private readonly Transform _container;
        private readonly Actor[] _prefabs = new Actor[ActorTypeId.Count];
        private readonly ActorSettings[][] _settings = new ActorSettings[ActorTypeId.Count][];

        protected readonly ReactiveSet<Actor>[] _actors = new ReactiveSet<Actor>[PlayerId.Count];

        public ReadOnlyReactiveSet<Actor> this[int playerId] { [Impl(256)] get => _actors[playerId]; }

        public ActorsFactory(Settings settings) 
        {
            _container = settings.container;

            _prefabs [ActorTypeId.Warrior] = settings.warriorPrefab;
            _settings[ActorTypeId.Warrior] = settings.warriorsSettings.Init();

            _prefabs [ActorTypeId.Demon]   = settings.demonPrefab;
            _settings[ActorTypeId.Demon]   = settings.demonsSettings.Init();

            for (int i = 0; i < PlayerId.Count; i++)
                _actors[i] = new();
        }

        [Impl(256)] public Actor Create(int type, int id, ActorInitData initData, Hexagon startHex)
        {
            Actor actor = UnityEngine.Object.Instantiate(_prefabs[type], _container);
            actor.Setup(_settings[type][id], initData, startHex);
            _actors[initData.owner].Add(actor);

            return actor;
        }

        [Impl(256)] public Actor Load(int type, ActorInitData initData, ActorLoadData loadData)
        {
            Actor actor = Create(type, loadData.state.id, initData, GameContainer.Hexagons[loadData.keyHex]);
            actor.SetLoadData(loadData);
            return actor;
        }

        [Impl(256)] public Skills GetSkills(Actor actor) => _settings[actor.TypeId][actor.Id].Skills;

        public void Dispose()
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _actors[i].Dispose();

            for (int i = 0; i < ActorTypeId.Count; i++)
                for (int j = _settings[i].Length; j >= 0; j--)
                    _settings[i][j].Dispose();
        }

        #region Nested Settings
        // ****************************************************************
        [System.Serializable]
        public class Settings : System.IDisposable
        {
            public Transform container;
            [Space]
            public Warrior warriorPrefab;
            public WarriorsSettingsScriptable warriorsSettings;
            [Space]
            public Demon demonPrefab;
            public DemonsSettingsScriptable demonsSettings;

            public void Dispose()
            {
                warriorsSettings.Dispose();
                demonsSettings.Dispose();
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (Application.isPlaying) return;

                EUtility.SetObject(ref container, "Actors");

                EUtility.SetPrefab(ref warriorPrefab);
                EUtility.SetScriptable(ref warriorsSettings);

                EUtility.SetPrefab(ref demonPrefab);
                EUtility.SetScriptable(ref demonsSettings);
            }
#endif
        }
        #endregion
    }
}
