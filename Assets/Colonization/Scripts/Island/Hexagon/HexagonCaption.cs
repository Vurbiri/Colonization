using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static Vurbiri.Colonization.UI.CONST_UI;

    public class HexagonCaption : MonoBehaviour
    {
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private Renderer _idTextRenderer;
        [Space]
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private Renderer _currencyTextRenderer;
        [Space]
        [SerializeField] private float _angleX = 90f;

        private Transform _thisTransform, _cameraTransform;
        private Quaternion _lastCameraRotation;

        public void Initialize(Transform cameraTransform, int id, IReadOnlyList<Id<CurrencyId>> spritesIds)
        {
            _thisTransform = transform;
            _cameraTransform = cameraTransform;
            _lastCameraRotation = Quaternion.identity;

            StringBuilder sb = new(TAG_SPRITE_LENGTH * spritesIds.Count);

            foreach (var sid in spritesIds)
                sb.AppendFormat(TAG_SPRITE, sid);

            _currencyText.text = sb.ToString();
            _idText.text = id.ToString();

        }

        private void Update()
        {
            if (!_idTextRenderer.isVisible && !_currencyTextRenderer.isVisible && _lastCameraRotation == _cameraTransform.rotation) 
                return;

            _lastCameraRotation = _cameraTransform.rotation;
            _thisTransform.localRotation = Quaternion.Euler(_angleX, _lastCameraRotation.eulerAngles.y, 0f);
        }

    }
}
