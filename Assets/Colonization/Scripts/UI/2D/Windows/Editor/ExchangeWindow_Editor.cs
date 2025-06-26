using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class ExchangeWindow
	{
        [StartEditor]
        [SerializeField, ReadOnly] private RectTransform _mainContainer;
        [SerializeField, ReadOnly] private RectTransform _bankContainer;
        [SerializeField, ReadOnly] private RectTransform _playerContainer;
        [EndEditor] public bool endEditor;

        public void UpdateVisuals_Editor(float pixelsPerUnit, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.PanelBack.SetAlpha(1f);
            image.pixelsPerUnitMultiplier = pixelsPerUnit;
        }

        public void Setup_Editor()
        {
        }

        public void Create_Editor()
        {
        }

        public void Delete_Editor()
        {
        }

        private void OnValidate()
        {
            EUtility.SetComponent(ref _mainContainer, this);
            EUtility.SetChildren(ref _bankContainer, this, "Bank");
            EUtility.SetChildren(ref _playerContainer, this, "Player");
        }
    }
}
