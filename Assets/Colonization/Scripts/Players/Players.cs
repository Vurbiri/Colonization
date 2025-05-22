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
        private readonly Hexagons hexagons;

        public Human Player => _humans[PlayerId.Player];
        public Satan Satan => _satan;
        public Human this[int index] => _humans[index];

        #region Constructor
        public Players(Settings settings, GameEvents game, Hexagons hexagons, Crossroads crossroads, GameplayStorage storage)
        {
            HumanStorage[] playerStorages = storage.Humans;

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i] = new(i, playerStorages[i], settings, hexagons, crossroads);

            _satan = new(storage.Satan, settings, hexagons, _humans);

            game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            game.Subscribe(GameModeId.StartTurn, OnStartTurn);
            game.Subscribe(GameModeId.Profit, OnProfit);
            game.Subscribe(GameModeId.Play, OnPlay);
        }
        #endregion

        public void OnEndTurn(TurnQueue turnQueue, int hexId)
        {
            if (turnQueue.currentId == PlayerId.Satan)
                _satan.EndTurn();
            else
                _humans[turnQueue.currentId.Value].EndTurn();
        }
        public void OnStartTurn(TurnQueue turnQueue, int hexId)
        {
            if (turnQueue.currentId == PlayerId.Satan)
                _satan.StartTurn();
            else
                _humans[turnQueue.currentId.Value].StartTurn();
        }
        public void OnProfit(TurnQueue turnQueue, int hexId)
        {
            _satan.Profit(hexId);
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i].Profit(hexId);
        }
        public void OnPlay(TurnQueue turnQueue, int hexId)
        {
            if (turnQueue.currentId == PlayerId.Satan)
                _satan.Play();
            else
                _humans[turnQueue.currentId.Value].Play();
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
            public Prices prices;
            public HumanAbilitiesScriptable humanStates;
            public EconomicPerksScriptable economicPerks;
            public MilitaryPerksScriptable militaryPerks;
            public RoadFactory roadFactory;
            [Space]
            public DemonInitializer demonPrefab;
            public DemonBuffsScriptable demonBuffs;
            [Space]
            public BuffsScriptable artefact;
            public Transform actorsContainer;

            public void Dispose()
            {
                humanStates.Dispose();
                economicPerks.Dispose();
                militaryPerks.Dispose();
                demonBuffs.Dispose();
                artefact.Dispose();
                humanStates = null; economicPerks = null; militaryPerks = null;
                demonBuffs = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory.OnValidate();

                EUtility.SetPrefab(ref warriorPrefab);
                EUtility.SetPrefab(ref demonPrefab);

                EUtility.SetScriptable(ref prices);
                EUtility.SetScriptable(ref humanStates);
                EUtility.SetScriptable(ref economicPerks);
                EUtility.SetScriptable(ref militaryPerks);
                EUtility.SetScriptable(ref demonBuffs);
                EUtility.SetScriptable(ref artefact);

                EUtility.SetObject(ref actorsContainer, "Actors");
            }
#endif
        }
        #endregion
    }
}
