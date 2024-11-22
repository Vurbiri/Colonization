//Assets\Colonization\Scripts\Island\Hexagon\HexagonCaption.cs
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

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
        [SerializeField] private Color _colorNormal = Color.white;
        [SerializeField] private Color32 _colorProfit = Color.green;
        [Space]
        [SerializeField] private float _angleX = 90f;

        private Transform _thisTransform, _cameraTransform;
        private Quaternion _lastCameraRotation;
        private string _defaultCurrencyText;
        protected GameObject _thisGO;

        public void Init(int id, IReadOnlyList<Id<CurrencyId>> spritesIds)
        {
            _thisGO = gameObject;
            _thisTransform = transform;
            _cameraTransform = SceneObjects.Get<Camera>().transform;
            _lastCameraRotation = Quaternion.identity;

            StringBuilder sb = new(TAG_SPRITE_LENGTH * spritesIds.Count);

            foreach (var sid in spritesIds)
                sb.AppendFormat(TAG_SPRITE, sid);

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
            if (!_idTextRenderer.isVisible && !_currencyTextRenderer.isVisible && _lastCameraRotation == _cameraTransform.rotation) 
                return;

            _lastCameraRotation = _cameraTransform.rotation;
            _thisTransform.localRotation = Quaternion.Euler(_angleX, _lastCameraRotation.eulerAngles.y, 0f);
        }
    }
}
