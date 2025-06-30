using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly IPlayerController[] _players = new IPlayerController[PlayerId.Count];
        private readonly PlayersEquipment _playersEquipment;

        public Human Player { get; }
        public Satan Satan { get; }

        public Players(Settings settings, GameLoop game, GameplayStorage storage)
        {
            HumanStorage[] playerStorages = storage.Humans;
            _playersEquipment = settings.playersEquipment;

            PlayerController playerController = new(playerStorages, settings);
            _players[PlayerId.Player] = playerController; Player = playerController;

            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                _players[i] = new AIController(game, i, playerStorages[i], settings);

            SatanController satanController = new(game, storage.Satan, settings);
            _players[PlayerId.Satan] = satanController;  Satan = satanController;
 
            game.Subscribe(GameModeId.Landing,    (turn, _) => _players[turn.currentId.Value].OnLanding());
            game.Subscribe(GameModeId.EndLanding, (turn, _) => _players[turn.currentId.Value].OnEndLanding());
            game.Subscribe(GameModeId.EndTurn,    (turn, _) => _players[turn.currentId.Value].OnEndTurn());
            game.Subscribe(GameModeId.StartTurn,  (turn, _) => _players[turn.currentId.Value].OnStartTurn());
            game.Subscribe(GameModeId.Profit,     OnProfit);
            game.Subscribe(GameModeId.Play,       (turn, _) => _players[turn.currentId.Value].OnPlay());
        }

        public void Dispose()
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].Dispose();
        }

        private void OnProfit(TurnQueue turnQueue, int hexId)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].OnProfit(turnQueue.currentId, hexId);
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public Prices prices;
            public HumanAbilitiesScriptable humanStates;
            public PerksScriptable perks;
            public RoadFactory roadFactory;
            [Space]
            public DemonInitializer demonPrefab;
            public BuffsScriptable demonLeveling;
            [Space]
            public BuffsScriptable artefact;
            public Transform actorsContainer;

            [NonSerialized] public PlayersEquipment playersEquipment;

            [NonSerialized] public Coroutines coroutines;
            [NonSerialized] public CameraController cameraController;
            [NonSerialized] public Score score;
            [NonSerialized] public Balance balance;
            [NonSerialized] public Hexagons hexagons;
            [NonSerialized] public Crossroads crossroads;

            public void Init()
            {
                playersEquipment = new(prices);
            }

            public void Dispose()
            {
                humanStates.Dispose();
                perks.Dispose();
                demonLeveling.Dispose();
                artefact.Dispose();
                humanStates = null; perks = null;
                demonLeveling = null; artefact = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                roadFactory.OnValidate();

                EUtility.SetPrefab(ref warriorPrefab);
                EUtility.SetPrefab(ref demonPrefab);

                EUtility.SetScriptable(ref prices);
                EUtility.SetScriptable(ref humanStates);
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
