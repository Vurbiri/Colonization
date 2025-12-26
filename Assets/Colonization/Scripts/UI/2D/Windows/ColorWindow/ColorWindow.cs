using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Colonization.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class ColorWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		private const int COUNT = 3;

		[SerializeField] protected Switcher _switcher;
		[SerializeField] protected RectTransform _captionRect;

		private readonly VAction<Color> _onColorChanged = new();
		private RectTransform _thisRect;
		private ColorSlider[] _sliders;
		private Color _color;
		private bool _isDrag;
		private Vector2 _cursorStartPosition, _thisStartPosition;
		private int _openId;

		private void Start()
		{
			_switcher.Init(this, true);
			_switcher.onClose.Add(_onColorChanged.Clear);

			GetComponentInChildren<SimpleButton>().AddListener(_switcher.Close);

			var sliders = GetComponentsInChildren<ColorSlider>();
			for (int i = 0; i < COUNT; ++i)
				sliders[i].OnValueChanged.Add(OnComponentChange);

			_sliders = sliders;
			_thisRect = GetComponent<RectTransform>();
		}
				
		[Impl(256)] public void Close() => _switcher.Close();
		[Impl(256)] public void Close(RectTransform transform)
		{
			if (_openId == transform.GetHashCode())
				_switcher.Close();
		} 

		public Event<Color> Open(RectTransform transform, in Color color)
		{
			_onColorChanged.Clear();

			_openId = transform.GetHashCode();
			_thisRect.position = transform.GetTopWorldPosition();

			_color = color;
			for (int i = 0; i < COUNT; ++i)
				_sliders[i].Set(color);

			_switcher.Open();
			return _onColorChanged;
		}

		private void OnComponentChange(int component, float value)
		{
			_color.SetOwnComponent(component, value);

			for (int i = 0; i < COUNT; ++i)
				_sliders[i].Set(component, _color);

			_onColorChanged.Invoke(_color);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (_isDrag = RectTransformUtility.RectangleContainsScreenPoint(_captionRect, eventData.pressPosition, eventData.pressEventCamera))
			{
				_cursorStartPosition = eventData.pressPosition;
				_thisStartPosition = _thisRect.position;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (eventData.dragging && _isDrag)
			{
				_thisRect.position = _thisStartPosition + (eventData.position - _cursorStartPosition);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_isDrag)
			{
				var bl = _thisRect.GetBottomLeftWorldPosition();
				var tr = _captionRect.GetTopRightWorldPosition();

				if (bl.x < -10f || bl.y < -10f || tr.x > Screen.width || tr.y > Screen.height)
					_thisRect.position = _thisStartPosition;

				_isDrag = false;
			}
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			this.SetChildren(ref _captionRect, "Caption");
			
			_switcher?.OnValidate(this);
		}
#endif
	}
}
