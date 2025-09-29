using System;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class GameSettings : IReactive<GameSettings, bool>
    {
        private bool _isLoad;
        private int _maxScore;
        private readonly RBool _hexagonShow;
        private readonly RBool _trackingCamera;
        private readonly bool _isTutorial;

        private readonly VAction<GameSettings, bool> _eventChanged = new();

        public bool IsLoad
        {
            get => _isLoad;
            set { if (!value) Reset(0); else _isLoad = true; }
        }
        public int MaxScore => _maxScore;
        public RBool HexagonShow => _hexagonShow;
        public RBool TrackingCamera => _trackingCamera;
        public bool IsTutorial => _isTutorial;

        public GameSettings() : this(false, 0, true, true, true) { }
        private GameSettings(bool isLoad, int maxScore, bool isHexagonShow, bool trackingCamera, bool isTutorial = false)
        {
            _isLoad = isLoad;
            _maxScore = maxScore;
            _hexagonShow = new(isHexagonShow);
            _trackingCamera = new(trackingCamera);
            _isTutorial = isTutorial;

            _hexagonShow.Subscribe(OnChangedReactiveValue, false);
            _trackingCamera.Subscribe(OnChangedReactiveValue, false);
        }

        public Subscription Subscribe(Action<GameSettings, bool> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this, instantGetValue);

        public void Start()
        {
            _isLoad = true;
            _eventChanged.Invoke(this, false);
        }

        public void Reset()
        {
            int score = 0;
            if (_isLoad && ProjectContainer.StorageService.TryGet(SAVE_KEYS.SCORE, out int[] scores))
                score = scores[PlayerId.Person];

            Reset(score);
        }
        public void Reset(int score)
        {
            _isLoad = false;
            if (score > _maxScore) _maxScore = score;

            _eventChanged.Invoke(this, true);
        }

        private void OnChangedReactiveValue(bool value) => _eventChanged.Invoke(this, false);
    }
}
