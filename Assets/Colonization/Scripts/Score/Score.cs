//Assets\Colonization\Scripts\Score\Score.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Score : IReactive<Score>
    {
        public const int MAX_SCORE = PlayerId.HumansCount;

        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly Signer<Score> _eventChanged = new();

        public int PlayerScore => _values[PlayerId.Player];
        public int MaxScore => _values[MAX_SCORE];

        public IReadOnlyList<int> Values => _values;

        private Score(int[] values)
		{
			_values = values;
			_settings = SettingsFile.Load<ScoreSettings>();
        }
        public static Score Create(ProjectStorage storage)
        {
            bool isLoad = storage.TryGetScoreData(out int[] data);
            if (!isLoad) data = new int[PlayerId.HumansCount + 1];

            Score score = new(data);
            storage.ScoreBind(score, !isLoad);
            return score;
        }

        public Unsubscriber Subscribe(Action<Score> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);

        public void Reset()
        {
            if (_values[PlayerId.Player] > _values[MAX_SCORE])
                _values[MAX_SCORE] = _values[PlayerId.Player];
            
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _values[i] = 0;

            _eventChanged.Invoke(this);
        }
    }
}
