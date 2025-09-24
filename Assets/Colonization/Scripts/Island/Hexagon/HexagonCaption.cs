using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    using static TAG;

    public class HexagonCaption : MonoBehaviour
    {
        private const float ANGLE_X = 90f;

        [SerializeField] private TextMeshPro _currencyText;
        [SerializeField] private Renderer _currencyTextRenderer;
        [Space]
        [SerializeField] private TextMeshPro _idText;
        [SerializeField] private Renderer _idTextRenderer;
        [Space]
        [SerializeField, MinMax(1f, 60f)] private WaitRealtime _timeShowProfit = 25f;
        [SerializeField, Range(0.1f, 100f)] private float _fadeSpeed = 10f;

        private readonly WaitRealtime _timeShowNewId = new(10f);

        private bool _isShow = true, _isEnable = true;
        private bool _showDistance, _showProfit, _showMode, _showNewId;
        private GameObject _thisGameObject;
        private Color _profitColor, _prevColor;
        private Transform _thisTransform;
        private float _lastAngle;
        private string _defaultCurrencyText;
        private Coroutine _fadeCoroutine, _profitCoroutine, _newIdCoroutine;
        private Unsubscription _unsubscriber;

        private bool IsShow
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (_showDistance | _showProfit | _showMode | _showNewId) & _isEnable;
        }

        public int Id { set => _idText.text = value.ToString(); }

        public void Init(int id, IdFlags<CurrencyId> flags)
        {
            _thisGameObject = gameObject;
            _thisTransform = transform;

            _prevColor = GameContainer.UI.Colors.TextDefault;
            _profitColor =  GameContainer.UI.Colors.GetTextColor(id != CONST.GATE_ID);

            StringBuilder sb = new(TAG_SPRITE_LENGTH * CurrencyId.Count);

            for (int i = 0; i < CurrencyId.Count; i++)
                if (flags[i]) sb.AppendFormat(SPRITE, i);

            _currencyText.text = _defaultCurrencyText = sb.ToString();

            _idText.text = id.ToString();
            _idText.color = GameContainer.UI.Colors.TextDefault;

            SetActive();

            _unsubscriber = GameContainer.CameraTransform.Subscribe(OnChangeCamera);
            _unsubscriber = GameContainer.EventBus.EventHexagonShow.Add(OnCaptionEnable);
        }

        public void NewId(int id, Color color, float showTime)
        {
            StopCoroutine(ref _profitCoroutine);

            _showProfit = false;
            _showNewId = true;

            _idText.text = id.ToString();
            _idText.color = color;

            _timeShowNewId.Restart(showTime);

            SetActive();
            StartCoroutine(ref _newIdCoroutine, NewIdOff_Cn());

            #region Local: NewIdOff_Cn()
            //=================================
            IEnumerator NewIdOff_Cn()
            {
                yield return _timeShowNewId;
                _showNewId = false;
                _newIdCoroutine = null;

                SetActive();
            }
            #endregion
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetColor(Color value)
        {
            _prevColor = _idText.color;
            _idText.color = value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetColor() => _idText.color = _prevColor;

        public void Profit()
        {
            StopCoroutine(ref _newIdCoroutine);

            _showNewId = false;
            _showProfit = true;

            _idText.color = _profitColor;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Profit(int currency)
        {
            _currencyText.text = string.Format(SPRITE, currency);
            Profit();
        }
        public void ResetProfit()
        {
            _showProfit = false;
            _currencyText.text = _defaultCurrencyText;
            _idText.color = GameContainer.UI.Colors.TextDefault;
            SetActive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetActive(bool isShow)
        {
            _showMode = isShow;
            SetActive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetActive()
        {
            if (IsShow) Show(); else Hide();
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
            _thisTransform.localRotation = Quaternion.Euler(ANGLE_X, _lastAngle, 0f);
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

        private void OnChangeCamera(Transform transform)
        {
            bool showDistance = transform.position.y > CameraController.heightShow;
            if (_showDistance != showDistance)
            {
                _showDistance = showDistance;
                SetActive();
            }

            _lastAngle = transform.eulerAngles.y;
            if (_isShow && (_idTextRenderer.isVisible & _currencyTextRenderer.isVisible))
                _thisTransform.localRotation = Quaternion.Euler(ANGLE_X, _lastAngle, 0f);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private void TextEnable(bool value)
        //{
        //    _currencyText.enabled = value;
        //    _idText.enabled = value;
        //}

        private void OnDestroy()
        {
            _unsubscriber?.Dispose();
        }

#if UNITY_EDITOR

        [StartEditor]
        [SerializeField] private TextMeshPro _keyText;

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

        public void SetKey_Ed(Key key)
        {
            _keyText.text = key.ToString();
            _keyText.enabled = false;
        }
        public void ShowKey_Ed()
        {
            OnCaptionEnable(_keyText.enabled);
            _keyText.enabled = !_keyText.enabled;
        }
#endif
    }
}
