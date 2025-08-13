using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    public class ActorsFactory : System.IDisposable
    {
        private readonly Transform _container;
        private readonly Actor[] _prefabs = new Actor[ActorTypeId.Count];
        private readonly ActorSettings[][] _settings = new ActorSettings[ActorTypeId.Count][];

        public ActorSettings this[Actor actor] => _settings[actor.TypeId][actor.Id];

        public ActorsFactory(Settings settings) 
        {
            _container = settings.container;

            _prefabs [ActorTypeId.Warrior] = settings.warriorPrefab;
            _settings[ActorTypeId.Warrior] = settings.warriorsSettings.Init();

            _prefabs [ActorTypeId.Demon]   = settings.demonPrefab;
            _settings[ActorTypeId.Demon]   = settings.demonsSettings.Init();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor Create(int type, int id, ActorInitData initData, Hexagon startHex)
        {
            Actor actor = UnityEngine.Object.Instantiate(_prefabs[type], _container);
            actor.Setup(_settings[type][id], initData, startHex);
            return actor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Actor Load(int type, ActorInitData initData, ActorLoadData loadData)
        {
            Actor actor = Create(type, loadData.state.id, initData, GameContainer.Hexagons[loadData.keyHex]);
            actor.SetLoadData(loadData);
            return actor;
        }

        public void Dispose()
        {
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
