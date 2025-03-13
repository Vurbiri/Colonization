//Assets\Colonization\Scripts\Players\Players.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly Player[] _players = new Player[PlayerId.PlayersCount];

        public Player Player => _players[PlayerId.Player];

        #region Constructor
        public Players(Settings settings, ProjectSaveData saveData)
        {
            PlayerSaveData[] playersData = saveData.PlayersSaveData;

            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _players[i] = new(i, playersData[i], settings);
        }
        #endregion

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _players[i].Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _players[i].Dispose();
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public PricesScriptable prices;
            public PlayerAbilitiesScriptable states;
            public RoadFactory roadFactory;
            [Space]
            public DemonInitializer demonPrefab;
            [Space]
            public Transform actorsContainer;

            public void Dispose()
            {
                states.Dispose();
                states = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory.OnValidate();

                if (warriorPrefab == null)
                    warriorPrefab = EUtility.FindAnyPrefab<WarriorInitializer>();
                if (demonPrefab == null)
                    demonPrefab = EUtility.FindAnyPrefab<DemonInitializer>();
                if (actorsContainer == null)
                    actorsContainer = EUtility.FindObjectByName<Transform>("Actors");
                if (prices == null)
                    prices = EUtility.FindAnyScriptable<PricesScriptable>();
                if (states == null)
                    states = EUtility.FindAnyScriptable<PlayerAbilitiesScriptable>();
            }
#endif
        }
        #endregion
    }
}
