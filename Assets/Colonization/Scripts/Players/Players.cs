using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly IPlayerController[] _players = new IPlayerController[PlayerId.Count];

        public Human Player { get; }
        public Satan Satan { get; }

        public Players(Settings settings, Game game, GameplayStorage storage)
        {
            HumanStorage[] playerStorages = storage.Humans;
            Human[] humans = new Human[PlayerId.HumansCount];

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _players[i] = humans[i] = new(i, playerStorages[i], settings);
            
            _players[PlayerId.Satan] = Satan = new(storage.Satan, settings, humans);
            
            Player = humans[PlayerId.Player];

            game.Subscribe(GameModeId.Init,      (turn, _) => _players[turn.currentId.Value].Init());
            game.Subscribe(GameModeId.Play,      (turn, _) => _players[turn.currentId.Value].Play());
            game.Subscribe(GameModeId.EndTurn,   (turn, _) => _players[turn.currentId.Value].EndTurn());
            game.Subscribe(GameModeId.StartTurn, (turn, _) => _players[turn.currentId.Value].StartTurn());
            game.Subscribe(GameModeId.Profit,    OnProfit);
        }

        public void OnProfit(TurnQueue turnQueue, int hexId)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].Profit(turnQueue.currentId, hexId);
        }

        public void Dispose()
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].Dispose();
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

            public Score score;
            public Hexagons hexagons;
            public Crossroads crossroads;

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
