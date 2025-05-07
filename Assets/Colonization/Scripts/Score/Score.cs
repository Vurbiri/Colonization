//Assets\Colonization\Scripts\Score\Score.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Score : IReactive<IReadOnlyList<int>>
    {
		private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly Signer<IReadOnlyList<int>> _eventChanged = new();

        private Score(int[] values)
		{
			_values = values;
			_settings = SettingsFile.Load<ScoreSettings>();
        }
        public static Score Create(ProjectStorage storage)
        {
            bool isLoad = storage.TryGetScoreData(out int[] data);

            Score score = new(data);
            storage.ScoreBind(score, !isLoad);
            return score;
        }

        public Unsubscriber Subscribe(Action<IReadOnlyList<int>> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values);
    }
}
