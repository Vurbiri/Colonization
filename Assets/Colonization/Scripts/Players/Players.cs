//Assets\Colonization\Scripts\Players\Players.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using static Vurbiri.Colonization.PlayerId;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly Player _player;
        private readonly PlayerAI[] _playersAI = new PlayerAI[CountAI];

        public Player Player => _player;

        #region Constructor
        public Players(Settings settings, ProjectSaveData saveData)
        {
            PlayersSaveData playersData = saveData.PlayersSaveData;

            _player = new Player(0, playersData[0], settings);
            for (int i = 0, j = AI_01; i < CountAI; i++, j++)
                _playersAI[i] = new(j, playersData[j], settings);

            playersData.Save(true);
        }
        #endregion

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            _player.Profit(hexId, freeGroundRes);
            for (int i = 0; i < CountAI; i++)
                _playersAI[i].Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            _player.Dispose();
            for (int i = 0; i < CountAI; i++)
                _playersAI[i].Dispose();
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public DemonInitializer demonPrefab;
            public Transform actorsContainer;
            public PricesScriptable prices;
            public PlayerAbilitiesScriptable states;
            public RoadsFactory roadsFactory;

            public void Dispose()
            {
                states.Dispose();
                states = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (warriorPrefab == null)
                    warriorPrefab = EUtility.FindAnyPrefab<WarriorInitializer>();
                if (demonPrefab == null)
                    demonPrefab = EUtility.FindAnyPrefab<DemonInitializer>();
                if (prices == null)
                    prices = EUtility.FindAnyScriptable<PricesScriptable>();
                if (states == null)
                    states = EUtility.FindAnyScriptable<PlayerAbilitiesScriptable>();
                if (roadsFactory.prefab == null)
                    roadsFactory.prefab = EUtility.FindAnyPrefab<Roads>();
            }
#endif
        }
        #endregion
    }
}
