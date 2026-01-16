using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public abstract class VToggleBase<TToggle> : VSelectable, IPointerClickHandler, ISubmitHandler  where TToggle : VToggleBase<TToggle>
	{
		[SerializeField] protected bool _isOn;
		[SerializeField] protected VToggleGroup<TToggle> _group;
		[SerializeField] protected UVAction<bool> _onValueChanged = new();

		protected readonly TToggle _this;

		public bool IsOn { [Impl(256)] get => _isOn; [Impl(256)] set => SetValue(value, true); }
		public bool SilentIsOn { [Impl(256)] get => _isOn; [Impl(256)] set => SetValue(value, false); }
		public VToggleGroup<TToggle> Group
		{
			[Impl(256)] get => _group;
			set
			{
				if (_group == value) return;

				if (_group != null)
					_group.UnregisterToggle(_this);

				_group = value;

				if (value != null && isActiveAndEnabled)
					value.RegisterToggle(_this);

				UpdateVisualInstant();
			}
		}

		protected VToggleBase() : base()
		{
			_this = (TToggle)this;
		}

		protected override void Start()
		{
			base.Start();

			_onValueChanged.Init(_isOn);
		}

		[Impl(256)] public Subscription AddListener(Action<bool> action, bool instantGetValue = true) => _onValueChanged.Add(action, _isOn, instantGetValue);
		[Impl(256)] public void RemoveListener(Action<bool> action) => _onValueChanged.Remove(action);

		protected abstract void UpdateVisual();
		protected abstract void UpdateVisualInstant();

		[Impl(256)] public void LeaveGroup()
		{
			if (_group != null)
				_group.UnregisterToggle(_this);
			_group = null;
		}

		internal void SetFromGroup(bool value)
		{
			if (_isOn == value) return;

			_isOn = value;
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				UpdateVisualInstant();
			else
#endif
				UpdateVisual();

			UISystemProfilerApi.AddMarker("VToggle.onValueChanged", _this);
			_onValueChanged.Invoke(_isOn);
		}

		protected void SetValue(bool value, bool sendCallback)
		{
			if (_isOn == value || (_group != null && !_group.CanSetValue(_this, value, sendCallback)))
				return;

			_isOn = value;

#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				UpdateVisualInstant();
			else
#endif
				UpdateVisual();

			if (sendCallback)
			{
				UISystemProfilerApi.AddMarker("VToggle.onValueChanged", _this);
				_onValueChanged.Invoke(_isOn);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (_group != null)
				_group.RegisterToggle(_this);

			UpdateVisualInstant();
		}

		protected override void OnDisable()
		{
#if UNITY_EDITOR
			if (_group != null)
			{
				if(Application.isPlaying)
					_group.DelayedUnregisterToggle(_this);
				else
					_group.UnregisterToggle(_this);
			}
#else
			if (_group != null)
				_group.DelayedUnregisterToggle(_this);
#endif
			base.OnDisable();
		}

		protected virtual void OnApplicationQuit()
		{
			_group = null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
				InternalToggle();
		}

		public void OnSubmit(BaseEventData eventData)
		{
			InternalToggle();
		}

		[Impl(256)]
		private void InternalToggle()
		{
			if (isActiveAndEnabled && IsInteractable())
				SetValue(!_isOn, true);
		}

#if UNITY_EDITOR
		private VToggleGroup<TToggle> _groupEditor;
		private bool _isOnEditor;

		protected override void OnValidate()
		{
			base.OnValidate();

		   OnValidateAsync();
		}

		protected virtual async void OnValidateAsync()
		{
			await System.Threading.Tasks.Task.Delay(2);

			if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode || this == null || !isActiveAndEnabled || UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
				return;

			var so = new UnityEditor.SerializedObject(this);

			if (_groupEditor != _group)
			{
				if (_groupEditor != null)
					_groupEditor.UnregisterToggle(_this);

				if (_group != null && isActiveAndEnabled)
					_group.RegisterToggle(_this);

				so.FindProperty(nameof(_group)).objectReferenceValue = _group;
				_groupEditor = _group;

				UpdateVisualInstant();
			}

			if (_isOnEditor != _isOn)
			{
				_isOn = _isOnEditor;
				SetValue(!_isOn, false);

				so.FindProperty(nameof(_isOn)).boolValue = _isOn;
				_isOnEditor = _isOn;
				UpdateVisualInstant();
			}
			so.ApplyModifiedProperties();
		}
#endif
	}
}
