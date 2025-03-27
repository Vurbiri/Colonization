//Assets\Colonization\Scripts\Players\Players.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly Human[] _humans = new Human[PlayerId.HumansCount];
        private readonly Satan _satan;

        public Human Player => _humans[PlayerId.Player];
        public Satan Satan => _satan;
        public Human this[int index] => _humans[index];

        #region Constructor
        public Players(Settings settings, GameplayStorage storage)
        {
            Hexagons land = SceneContainer.Get<Hexagons>();
            HumanStorage[] playerStorages = storage.Humans;

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i] = new(i, playerStorages[i], settings, land);

            _satan = new(storage.Satan, settings, land, _humans);

        }
        #endregion

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i].Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            _satan.Dispose();
            for (int i = 0; i < PlayerId.HumansCount; i++)
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
            public EconomicPerksScriptable economicPerks;
            public MilitaryPerksScriptable militaryPerks;
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
                economicPerks.Dispose();
                militaryPerks.Dispose();
                satanStates.Dispose();
                demonBuffs.Dispose();
                artefact.Dispose();
                humanStates = null; economicPerks = null; militaryPerks = null;
                satanStates = null;  demonBuffs = null; artefact = null;
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
                if (economicPerks == null)
                    economicPerks = EUtility.FindAnyScriptable<EconomicPerksScriptable>();
                if (militaryPerks == null)
                    militaryPerks = EUtility.FindAnyScriptable<MilitaryPerksScriptable>();

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
