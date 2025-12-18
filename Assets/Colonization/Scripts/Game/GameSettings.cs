using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class GameSettings : IReactive<GameSettings, bool>
    {
        private bool _isLoad;
        private int _maxScore;
        private readonly RBool _hexagonShow;
        private readonly RBool _trackingCamera;
        private readonly bool _isFirst;

        private readonly VAction<GameSettings, bool> _eventChanged = new();

        public bool IsLoad
        {
            [Impl(256)] get => _isLoad;
            [Impl(256)] set { if (!value) Reset(0); else _isLoad = true; }
        }
        public int MaxScore { [Impl(256)] get => _maxScore; }
        public RBool HexagonShow { [Impl(256)] get => _hexagonShow; }
        public RBool TrackingCamera { [Impl(256)] get => _trackingCamera; }
        public bool IsFirstStart { [Impl(256)] get => _isFirst; }

        public GameSettings() : this(false, 0, true, true, true) { }
        private GameSettings(bool isLoad, int maxScore, bool isHexagonShow, bool trackingCamera, bool isTutorial = false)
        {
            _isLoad = isLoad;
            _maxScore = maxScore;
            _hexagonShow = new(isHexagonShow);
            _trackingCamera = new(trackingCamera);
            _isFirst = isTutorial;

            _hexagonShow.Subscribe(OnChangedReactiveValue, false);
            _trackingCamera.Subscribe(OnChangedReactiveValue, false);
        }

        [Impl(256)] public Subscription Subscribe(Action<GameSettings, bool> action, bool instantGetValue = true) => _eventChanged.Add(action, this, instantGetValue, instantGetValue);

        [Impl(256)]
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
        [Impl(256)]
        public void Reset(int score)
        {
            _isLoad = false;
            if (score > _maxScore) _maxScore = score;

            _eventChanged.Invoke(this, true);
        }

        private void OnChangedReactiveValue(bool value) => _eventChanged.Invoke(this, false);
    }
}
