using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
#if UNITY_EDITOR
	[RequireComponent(typeof(Image))]
#endif
	public class ColorInputField : MonoBehaviour, IPointerDownHandler
	{
		private ColorWindow _window;
		private RectTransform _thisRect;
		private Image _image;
		private System.Action<Color> _setColor;

		public Color32 Color { [Impl(256)] get => _image.color.ToColor32(); }

		public void Init(Color color, ColorWindow window)
		{
			_window = window;
			_thisRect = GetComponent<RectTransform>();
			
			_image = GetComponent<Image>();
			_image.color = color;

			_setColor = _image.GetSetor<Color>(nameof(Image.color));
		}

		public void SetColor(Color color)
		{
			_image.color = color;
			_window.Close(_thisRect);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_window.Open(_thisRect, _image.color).Add(_setColor);
		}

	}
}
