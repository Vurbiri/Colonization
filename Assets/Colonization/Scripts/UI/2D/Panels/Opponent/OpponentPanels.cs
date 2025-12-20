using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class OpponentPanels : MonoBehaviour
	{
        [SerializeField] private Direction2 _directionPopup;
		[Space]
        [SerializeField] private OpponentPanel[] _panels;
		
		public void Init()
		{
            Vector3 direction = _directionPopup;
			Diplomacy diplomacy = GameContainer.Diplomacy;

            for (int i = 0; i < PlayerId.AICount; i++)
				_panels[i].Init(direction, diplomacy);

            Destroy(this);
		}

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField, Range(-5f, -1f)] private float _offsetPanel = -2.5f;
        [Space]
        [SerializeField, HideInInspector] private UnityEngine.UI.Image _mainImage;

        public RectTransform UpdateVisuals_Ed(float pixelsPerUnit, SceneColorsEd colors, Vector2 padding)
        {
            Color color = colors.panelBack;

            _mainImage.color = color;
            _mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;

            Vector2 size = Vector2.zero;
            for (int i = 0; i < PlayerId.AICount; i++)
                size = _panels[i].UpdateVisuals_Editor(_offsetPanel * (i + 1));

            var thisTransform = (RectTransform)transform;

            thisTransform.sizeDelta = new(size.x * 2f, size.y);
            thisTransform.anchoredPosition = new(padding.x, -padding.y);

            return thisTransform;
        }

        private void OnValidate()
        {
			this.SetChildrens(ref _panels, PlayerId.AICount);
            this.SetComponent(ref _mainImage);
        }
#endif
	}
}
