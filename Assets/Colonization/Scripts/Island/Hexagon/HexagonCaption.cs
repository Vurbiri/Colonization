//Assets\Colonization\Scripts\Island\Hexagon\HexagonCaption.cs
using System.Text;
using TMPro;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class HexagonCaption : MonoBehaviour
    {
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private Renderer _idTextRenderer;
        [Space]
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private Renderer _currencyTextRenderer;
        [Space]
        [SerializeField] private float _angleX = 90f;

        private Color _colorNormal, _colorProfit;
        private Transform _thisTransform, _cameraTransform;
        private Quaternion _lastCameraRotation;
        private string _defaultCurrencyText;
        protected GameObject _thisGO;

        public void Init(int id, IdFlags<CurrencyId> flags)
        {
            _thisGO = gameObject;
            _thisTransform = transform;
            _cameraTransform = SceneContainer.Get<Camera>().transform;
            _lastCameraRotation = Quaternion.identity;

            var colorSettings = SceneContainer.Get<ProjectColors>();
            _colorNormal = colorSettings.TextDefault;
            _colorProfit = colorSettings.TextPositive;

            StringBuilder sb = new(TAG_SPRITE_LENGTH * CurrencyId.Count);

            for (int i = 0; i < CurrencyId.Count; i++)
                if (flags[i]) sb.AppendFormat(TAG_SPRITE, i);

            _currencyText.text = _defaultCurrencyText = sb.ToString();

            _idText.text = id.ToString();
            _idText.color = _colorNormal;
         }

        public void Profit()
        {
            _idText.color = _colorProfit;
            _thisGO.SetActive(true);
        }

        public void Profit(int currency)
        {
            _currencyText.text = string.Format(TAG_SPRITE, currency);
            _idText.color = _colorProfit;
            _thisGO.SetActive(true);
        }

        public void ResetProfit(bool active)
        {
            _thisGO.SetActive(active);
            _currencyText.text = _defaultCurrencyText;
            _idText.color = _colorNormal;
        }

        public void SetActive(bool active) => _thisGO.SetActive(active);

        private void Update()
        {
            if (!_idTextRenderer.isVisible & !_currencyTextRenderer.isVisible & _lastCameraRotation == _cameraTransform.rotation) 
                return;

            _lastCameraRotation = _cameraTransform.rotation;
            _thisTransform.localRotation = Quaternion.Euler(_angleX, _lastCameraRotation.eulerAngles.y, 0f);
        }
    }
}
