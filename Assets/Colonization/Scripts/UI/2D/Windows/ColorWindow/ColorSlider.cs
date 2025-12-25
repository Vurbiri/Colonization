using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
#if UNITY_EDITOR
    [RequireComponent(typeof(Image))]
#endif
    sealed public class ColorSlider : MonoBehaviour, IDragHandler, IPointerDownHandler, IInitializePotentialDragHandler
    {
        private static readonly int s_colorMinID, s_colorMaxID;

        [SerializeField] private int _component;
        [SerializeField] private int _axis;
        [SerializeField] private RectTransform _handle;

        private readonly VAction<int, float> _onValueChanged = new();
        private Material _material;
        private RectTransform _handleContainer;
        private float _value;
        
        private Vector2 _offset = Vector2.zero;

        public float Value { [Impl(256)] get => _value; }
        public int Component { [Impl(256)] get => _component; }
        public Event<int, float> OnValueChanged { [Impl(256)] get => _onValueChanged; }

        static ColorSlider()
        {
            s_colorMinID = Shader.PropertyToID("_ColorMin");
            s_colorMaxID = Shader.PropertyToID("_ColorMax");
        }

        public void Set(Color color)
        {
            Set(color[_component], false);
            _material.SetColor(s_colorMinID, color.SetOwnComponent(_component, 0f));
            _material.SetColor(s_colorMaxID, color.SetOwnComponent(_component, 1f));
        }

        private void Set(float value, bool sendCallback)
        {
            if (_value != value)
            {
                _value = value;

                UpdateVisuals();

                if (sendCallback)
                    _onValueChanged.Invoke(_component, _value);
            }
        }

        private void UpdateVisuals()
        {
            Vector2 anchorMin = Vector2.zero;
            Vector2 anchorMax = Vector2.one;
            anchorMin[_axis] = anchorMax[_axis] = _value;
            _handle.anchorMin = anchorMin;
            _handle.anchorMax = anchorMax;
        }

        private void UpdateDrag(PointerEventData eventData)
		{
            var clickRect = _handleContainer;
            if (clickRect.rect.size[_axis] > 0)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(clickRect, eventData.position, eventData.pressEventCamera, out Vector2 localCursor))
                {
                    localCursor -= clickRect.rect.position;
                    Set(Mathf.Clamp01((localCursor - _offset)[_axis] / clickRect.rect.size[_axis]), true);
                }
            }
        }

        #region Calls ..

        private void Awake()
        {
            _handleContainer = (RectTransform)_handle.parent;
            _material = GetComponent<Image>().material;
        }

        private void OnRectTransformDimensionsChange()
        {
            if (isActiveAndEnabled)
                UpdateVisuals();
        }

        [Impl(256)] private bool CanDrag(PointerEventData eventData) => eventData.button == PointerEventData.InputButton.Left && isActiveAndEnabled;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!CanDrag(eventData)) return;

            var screenPosition = eventData.pointerPressRaycast.screenPosition;

            _offset = Vector2.zero;
            if (RectTransformUtility.RectangleContainsScreenPoint(_handle, screenPosition, eventData.enterEventCamera)
            &&  RectTransformUtility.ScreenPointToLocalPointInRectangle(_handle, screenPosition, eventData.pressEventCamera, out Vector2 localMousePos))
            {
                _offset = localMousePos;
            }
            else
            {
                UpdateDrag(eventData);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (CanDrag(eventData))
                UpdateDrag(eventData);
        }
        public void OnInitializePotentialDrag(PointerEventData eventData) => eventData.useDragThreshold = false;
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _handle, "Handle");

            if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                return;
            
            const string SHADER_NAME = "UI/SimpleGradient";
            const int VERTICAL = 1;

            var image = GetComponent<Image>();
            var material = image.material;

            if (material == null || material.shader.name != SHADER_NAME)
            {
                var shader = Shader.Find(SHADER_NAME);
                material = new Material(shader);
                image.SetObjectField(material, "m_Material");
            }

            UnityEngine.Rendering.LocalKeyword keyVertical = new(material.shader, "IS_VERTICAL");
            bool isVertical = _axis == VERTICAL;
            if (isVertical != material.IsKeywordEnabled(keyVertical))
            {
                material.SetKeyword(keyVertical, isVertical);
                UnityEditor.EditorUtility.SetDirty(material);
            }

            this.GetComponentInChildren<Image>("Mark").SetColorField(Color.black.SetComponent(_component, 1f));

            material.SetColor(s_colorMinID, Color.white.SetComponent(_component, 0f));
            material.SetColor(s_colorMaxID, Color.white.SetComponent(_component, 1f));
        }

        public const string componentField = nameof(_component);
        public const string axisField = nameof(_axis);
        public const string handleField = nameof(_handle);
#endif
    }
}
