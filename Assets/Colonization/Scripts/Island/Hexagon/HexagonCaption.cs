using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class HexagonCaption : MonoBehaviour
    {
        private const float ANGLE_X = 90f;

        [SerializeField] private TextMeshPro _currencyText;
        [SerializeField] private Renderer _currencyTextRenderer;
        [Space]
        [SerializeField] private TextMeshPro _idText;
        [SerializeField] private Renderer _idTextRenderer;
        [Space]
        [SerializeField, MinMax(1f, 60f)] private WaitRealtime _timeShowNewId = 12.5f;
        [SerializeField, MinMax(1f, 60f)] private WaitRealtime _timeShowProfit = 25f;
        [SerializeField, Range(0.1f, 100f)] private float _fadeSpeed = 10f;

        private bool _isShow = true, _isEnable = true;
        private bool _showDistance, _showProfit, _showMode, _showNewId;
        private GameObject _thisGameObject;
        private Color _colorNormal, _colorProfit;
        private Transform _thisTransform, _cameraTransform;
        private Quaternion _lastCameraRotation;
        private string _defaultCurrencyText;
        private Coroutine _fadeCoroutine, _profitCoroutine, _newIdCoroutine;
        private Unsubscription _unsubscriber;

        private bool IsShow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _showDistance | _showProfit | _showMode | _showNewId & _isEnable;
        }

        public int Id { set => _idText.text = value.ToString(); }
        public Color IdColor
        {
            get => _idText.color;
            set => _idText.color = value;
        }

        public void Init(int id, IdFlags<CurrencyId> flags, Transform cameraTransform, ProjectColors colorSettings, GameplayEventBus eventBus)
        {
            _thisGameObject = gameObject;
            _thisTransform = transform;
            _cameraTransform = cameraTransform;
            _lastCameraRotation = Quaternion.identity;

            _colorNormal = colorSettings.TextDefault;
            _colorProfit = id != CONST.GATE_ID ? colorSettings.TextPositive : colorSettings.TextNegative;

            StringBuilder sb = new(TAG_SPRITE_LENGTH * CurrencyId.Count);

            for (int i = 0; i < CurrencyId.Count; i++)
                if (flags[i]) sb.AppendFormat(TAG_SPRITE, i);

            _currencyText.text = _defaultCurrencyText = sb.ToString();

            _idText.text = id.ToString();
            _idText.color = _colorNormal;

            SetActive();

            _unsubscriber = eventBus.EventHexagonShowDistance.Add(OnShowDistance);
            _unsubscriber = eventBus.EventHexagonShow.Add(OnCaptionEnable);
        }

        public void NewId(int id, Color color)
        {
            StopCoroutine(ref _profitCoroutine);

            _showProfit = false;
            _showNewId = true;

            _idText.text = id.ToString();
            _idText.color = color;

            SetActive();
            StartCoroutine(ref _newIdCoroutine, NewIdOff_Cn());

            #region Local: NewIdOff_Cn()
            //=================================
            IEnumerator NewIdOff_Cn()
            {
                yield return _timeShowNewId.Restart();
                _idText.color = _colorNormal;
                _showNewId = false;
                _newIdCoroutine = null;

                SetActive();
            }
            #endregion
        }

        public void Profit()
        {
            StopCoroutine(ref _newIdCoroutine);

            _showNewId = false;
            _showProfit = true;

            _idText.color = _colorProfit;
            SetActive();
            StartCoroutine(ref _profitCoroutine, ProfitOff_Cn());

            #region Local: ProfitOff_Cn()
            //=================================
            IEnumerator ProfitOff_Cn()
            {
                yield return _timeShowProfit.Restart();
                _showProfit = false;
                _profitCoroutine = null;
                SetActive();
            }
            #endregion

        }
        public void Profit(int currency)
        {
            _currencyText.text = string.Format(TAG_SPRITE, currency);
            Profit();
        }
        public void ResetProfit()
        {
            _showProfit = false;
            _currencyText.text = _defaultCurrencyText;
            _idText.color = _colorNormal;
            SetActive();
        }

        public void SetActive(bool isShow)
        {
            _showMode = isShow;
            SetActive();
        }

        private void SetActive()
        {
            if (IsShow) Show(); else Hide();
        }

        private void OnShowDistance(bool value)
        {
            _showDistance = value;
            SetActive();
        }
        private void OnCaptionEnable(bool value)
        {
            _isEnable = value;
            SetActive();
        }

        private void Show()
        {
            if (_isShow) return;
            
            _isShow = true;
            _thisGameObject.SetActive(true);
            StartCoroutine(ref _fadeCoroutine, Show_Cn());

            #region Local: Show_Cn()
            //=================================
            IEnumerator Show_Cn()
            {
                float scale = _thisTransform.localScale.x;

                while (scale < 1f)
                {
                    scale += Time.unscaledDeltaTime * _fadeSpeed;
                    _thisTransform.localScale = new Vector3(scale, scale, scale);
                    yield return null;
                }

                _thisTransform.localScale = Vector3.one;
                _fadeCoroutine = null;
            }
            #endregion
        }

        private void Hide()
        {
            if (!_isShow) return;

            _isShow = false;
            StartCoroutine(ref _fadeCoroutine, Hide_Cn());

            #region Local: Hide_Cn()
            //=================================
            IEnumerator Hide_Cn()
            {
                float scale = _thisTransform.localScale.x;

                while (scale > 0f)
                {
                    scale -= Time.unscaledDeltaTime * _fadeSpeed;
                    _thisTransform.localScale = new Vector3(scale, scale, scale);
                    yield return null;
                }

                _thisTransform.localScale = Vector3.zero;
                _fadeCoroutine = null;
                _thisGameObject.SetActive(false);
            }
            #endregion
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartCoroutine(ref Coroutine coroutine, IEnumerator rutune)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(rutune);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private void Update()
        {
            if ((_idTextRenderer.isVisible & _currencyTextRenderer.isVisible) && _lastCameraRotation != _cameraTransform.rotation)
            {
                _lastCameraRotation = _cameraTransform.rotation;
                _thisTransform.localRotation = Quaternion.Euler(ANGLE_X, _lastCameraRotation.eulerAngles.y, 0f);
            }
        }

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_currencyText == null || _currencyTextRenderer == null)
            {
                _currencyText = this.GetComponentInChildren<TextMeshPro>("Currency_Text(TMP)");
                _currencyTextRenderer = _currencyText.renderer;
            }
            if (_idText == null || _idTextRenderer == null)
            {
                _idText = this.GetComponentInChildren<TextMeshPro>("Id_Text(TMP)");
                _idTextRenderer = _idText.renderer;
            }

            transform.localRotation = Quaternion.Euler(ANGLE_X, 0f, 0f);
        }
#endif
    }
}
