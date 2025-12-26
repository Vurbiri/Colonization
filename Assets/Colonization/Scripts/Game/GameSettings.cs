using System;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class GameSettings
	{
		private bool _isLoad;
		private int _maxScore;
		private readonly RBool _hexagonShow;
		private readonly RBool _trackingCamera;
		private readonly bool _isFirst;

		private readonly VAction<GameSettings, bool> _change = new();

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
		private GameSettings(bool isLoad, int maxScore, bool isHexagonShow, bool trackingCamera, bool isFirst = false)
		{
			_isLoad = isLoad;
			_maxScore = maxScore;
			_hexagonShow = new(isHexagonShow);
			_trackingCamera = new(trackingCamera);
			_isFirst = isFirst;

			_hexagonShow.Subscribe(OnChangedReactiveValue, false);
			_trackingCamera.Subscribe(OnChangedReactiveValue, false);
		}

		[Impl(256)] public Subscription Subscribe(Action<GameSettings, bool> action, bool instantGetValue = true) => _change.Add(action, this, instantGetValue, instantGetValue);

		[Impl(256)]
		public void Start()
		{
			_isLoad = true;
			_change.Invoke(this, false);
		}

		[Impl(256)]
		public void Reset(int score)
		{
			_isLoad = false;
			_maxScore = MathI.Max(score, _maxScore);

			_change.Invoke(this, true);
			
		}

		private void OnChangedReactiveValue(bool value) => _change.Invoke(this, false);
	}
}
