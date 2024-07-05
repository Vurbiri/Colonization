using Newtonsoft.Json;
using System;
using UnityEngine;

//[DefaultExecutionOrder(-2)]
public class GameSettingsData : ASingleton<GameSettingsData>
{
    [Space]
    [SerializeField] private string _keySave = "gsd";
    [Space]
    [SerializeField, Range(4, 6)] private int _circleMax = 5;
    [SerializeField, Range(0, 100)] private int _chanceWater = 33;

    private GameSave _data;
    private bool _isNewRecord = false;

    public bool IsNewGame => _data.modeStart == GameModeStart.New;
    public int CircleMax => _circleMax;
    public int ChanceWater => _chanceWater;
    public bool IsRecord => _data.score > _data.maxScore;
    public int Score { get => _data.score; private set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public int MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value); } }
    
    public event Action<int> EventChangeScore;
    public event Action<int> EventChangeMaxScore;

    public bool Initialize(bool isLoad)
    {
        bool result = isLoad && Load();
        if (!result)
            _data = new(_circleMax);

        _isNewRecord = _data.score > _data.maxScore;

        return result;
    }

    private bool Load()
    {
        Return<GameSave> data = Storage.Load<GameSave>(_keySave);
        if (data.Result)
            _data = data.Value;

        return data.Result;
    }
    public void Save(Action<bool> callback = null) => StartCoroutine(Storage.Save_Coroutine(_keySave, _data, callback));

    public void StartGame()
    {
        _data.modeStart = GameModeStart.Continue;
    }
        
    public void ResetGame()
    {
        if (IsRecord)
            MaxScore = Score;

        _data.Reset(_circleMax);

        _isNewRecord = false;
        Save();
        EventChangeScore?.Invoke(0);
    }

    #region Nested: GameSave
    //***********************************
    private class GameSave
    {
        [JsonProperty("gms")]
        public GameModeStart modeStart = GameModeStart.New;
        [JsonProperty("cm")]
        public int circleMax = 4;
        [JsonProperty("sc")]
        public int score = 0;
        [JsonProperty("ms")]
        public int maxScore = 0;

        [JsonConstructor]
        public GameSave(GameModeStart modeStart, int circleMax, int score, int maxScore)
        {
            this.modeStart = modeStart;
            this.circleMax = circleMax;
            this.score = score;
            this.maxScore = maxScore;
        }
        public GameSave(int circleMax)
        {
            Reset(circleMax);
            maxScore = 0;
        }
        public GameSave()
        {
            Reset(4);
            maxScore = 0;
        }

        public void Reset(int circleMax)
        {
            modeStart = GameModeStart.New;
            this.circleMax = circleMax;
            score = 0;
        }
    }
    #endregion
}
