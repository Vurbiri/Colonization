//Assets\Colonization\Scripts\GameLoop\GameLoop.cs
namespace Vurbiri.Colonization
{
    sealed public partial class GameLoop : GameEvents
    {
        private Id<GameModeId> _gameMode;
        private TurnQueue _turnQueue;
        private int _hexId;

        private GameState _gameState;

        private GameLoop()
        {
            _gameMode = GameModeId.Play; //GameModeId.Init
            _turnQueue = new(PlayerId.Player);
            _hexId = -1;
        }

        private GameLoop(Id<GameModeId> gameMode, TurnQueue turnQueue, int hexId)
        {
            _gameMode = gameMode;
            _turnQueue = turnQueue;
            _hexId = hexId;
        }

        public static GameLoop Create(GameState gameState)
        {
            if (!gameState.TryGetGame(out GameLoop instance))
            {
                instance = new();
                gameState.SaveGame(instance);
            }

            for (int i = 0; i < GameModeId.Count; i++)
                instance._changingGameModes[i] += (_, _) => { };

            instance._gameState = gameState;
            return instance;
        }

        public void Start()
        {
            _gameState.Start();
            Change(_gameMode);
        }

        public void Init()
        {
            _turnQueue.Next();
            if(_turnQueue.currentId != PlayerId.HumansCount)
                Change(GameModeId.Init);
            else
                StartTurn();
        }

        public void Play()
        {
            Change(GameModeId.Play);
        }

        public void EndTurn()
        {
            Change(GameModeId.EndTurn);
        }

        public void StartTurn()
        {
            _turnQueue.Next();

            Change(GameModeId.StartTurn);
        }

        public void WaitRoll()
        {
            Change(GameModeId.WaitRoll);
        }

        public void Roll(int newValue)
        {
            _hexId = newValue;

            Change(GameModeId.Roll);
        }

        public void Profit()
        {
            Change(GameModeId.Profit);
        }

        private void Change(Id<GameModeId> gameMode)
        {
            _changingGameModes[_gameMode = gameMode].Invoke(_turnQueue, _hexId);
            _gameState.SaveGame(this);
        }

        //public void EndTurnPlayer()
        //{
        //    _players.EndTurn(_turnQueue.currentId.Value);

        //    _dice.Roll();
        //    ACurrencies free = _hexagons.Profit(_dice.last, _dice.current);
        //    _players.Profit(_dice.current, free);

        //    _turnQueue.Next();

        //    int currentId = _turnQueue.currentId.Value;

        //    //_diplomacy.StartTurn(currentId);
        //    _players.StartTurn(currentId);
        //    _inputController.GameplayMap = _turnQueue.IsCurrentPlayer;
        //    _players.Play(currentId);
        //}
    }


}
