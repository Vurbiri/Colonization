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
        private readonly Human[] _humans = new Human[PlayerId.PlayersCount];
        private readonly Satan _satan;

        public Human Player => _humans[PlayerId.Player];
        public Satan Satan => _satan;
        public Human this[int index] => _humans[index];

        #region Constructor
        public Players(Settings settings, GameplaySaveData saveData)
        {
            Hexagons land = SceneObjects.Get<Hexagons>();
            HumanSaveData[] playersData = saveData.Humans;

            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _humans[i] = new(i, playersData[i], settings, land);

            _satan = new(saveData.Satan, settings, land);

        }
        #endregion

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _humans[i].Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            _satan.Dispose();
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _humans[i].Dispose();
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public PricesScriptable prices;
            public HumanAbilitiesScriptable humanStates;
            public RoadFactory roadFactory;
            [Space]
            public DemonInitializer demonPrefab;
            public SatanAbilitiesScriptable satanStates;
            public DemonBuffsScriptable demonBuffs;
            [Space]
            public BuffsScriptable artefact;
            public Transform actorsContainer;

            public void Dispose()
            {
                humanStates.Dispose();
                satanStates.Dispose();
                demonBuffs.Dispose();
                artefact.Dispose();
                humanStates = null; satanStates = null;  demonBuffs = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory.OnValidate();

                if (warriorPrefab == null)
                    warriorPrefab = EUtility.FindAnyPrefab<WarriorInitializer>();
                if (prices == null)
                    prices = EUtility.FindAnyScriptable<PricesScriptable>();
                if (humanStates == null)
                    humanStates = EUtility.FindAnyScriptable<HumanAbilitiesScriptable>();

                if (demonPrefab == null)
                    demonPrefab = EUtility.FindAnyPrefab<DemonInitializer>();
                if (satanStates == null)
                    satanStates = EUtility.FindAnyScriptable<SatanAbilitiesScriptable>();
                if (demonBuffs == null)
                    demonBuffs = EUtility.FindAnyScriptable<DemonBuffsScriptable>();

                if (artefact == null)
                    artefact = EUtility.FindAnyScriptable<BuffsScriptable>();
                if (actorsContainer == null)
                    actorsContainer = EUtility.FindObjectByName<Transform>("Actors");
            }
#endif
        }
        #endregion
    }
}
