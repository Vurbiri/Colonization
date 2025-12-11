using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class HexagonCaption
    {
        private const float ANGLE_X = 90f;

        [SerializeField] private Transform _thisTransform;
        [SerializeField] private GameObject _thisGameObject;
        [Space]
        [SerializeField] private TextMeshPro _currencyText;
        [SerializeField] private TextMeshPro _idText;
        [Space]
        [SerializeField, MinMax(1f, 30f)] private WaitRealtime _timeShowProfit; // = 20f;
        [SerializeField, MinMax(1f, 30f)] private WaitRealtime _timeShowNewId; // = 10f;
        [SerializeField, Range(0.1f, 20f)] private float _fadeSpeed; //= 8f;

        private MonoBehaviour _mono;
        private Visible _visible = new(true);
        private Color _profitColor, _prevColor;
        private float _lastAngle;
        private string _defaultCurrencyText;
        private Renderer _currencyTextRenderer, _idTextRenderer;
        private Coroutine _fadeCoroutine, _profitCoroutine, _newIdCoroutine;
        private Subscription _subscription;

        public void Init(int id, IdFlags<CurrencyId> flags, MonoBehaviour mono)
        {
            _currencyTextRenderer = _currencyText.GetComponent<Renderer>();
            _idTextRenderer = _idText.GetComponent<Renderer>();
            _mono = mono;

            _prevColor = GameContainer.UI.Colors.TextDefault;
            _profitColor =  GameContainer.UI.Colors.GetTextColor(id != HEX.GATE);

            StringBuilder sb = new(TAG.TAG_SPRITE_LENGTH * CurrencyId.Count);
            for (int i = 0; i < CurrencyId.Count; i++)
                if (flags[i]) sb.AppendFormat(TAG.SPRITE, i.ToStr());
            _currencyText.text = _defaultCurrencyText = sb.ToString();

            _idText.text = id.ToStr();
            _idText.color = GameContainer.UI.Colors.TextDefault;

            SetActive();

            _subscription  = GameContainer.CameraTransform.Subscribe(OnChangeCamera);
            _subscription += GameContainer.GameSettings.HexagonShow.Subscribe(OnCaptionEnable);
        }

        public void NewId(int id, Color color, float showTime)
        {
            StopCoroutine(ref _profitCoroutine);

            _visible.profit = false;
            _visible.newId = true;

            _idText.text = id.ToStr();
            _idText.color = color;

            _timeShowNewId.Restart(showTime);

            SetActive();
            StartCoroutine(ref _newIdCoroutine, NewIdOff_Cn());

            #region Local: NewIdOff_Cn()
            //=================================
            IEnumerator NewIdOff_Cn()
            {
                yield return _timeShowNewId;
                _visible.newId = false;
                _newIdCoroutine = null;

                SetActive();
            }
            #endregion
        }

        [Impl(256)] public void SetColor(Color value)
        {
            _prevColor = _idText.color;
            _idText.color = value;
        }
        [Impl(256)] public void ResetColor() => _idText.color = _prevColor;

        public void Profit()
        {
            StopCoroutine(ref _newIdCoroutine);

            _visible.newId = false;
            _visible.profit = true;

            _idText.color = _profitColor;
            SetActive();
            StartCoroutine(ref _profitCoroutine, ProfitOff_Cn());

            #region Local: ProfitOff_Cn()
            //=================================
            IEnumerator ProfitOff_Cn()
            {
                yield return _timeShowProfit.Restart();
                _visible.profit = false;
                _profitCoroutine = null;
                SetActive();
            }
            #endregion

        }
        [Impl(256)] public void Profit(int currency)
        {
            _currencyText.text = string.Format(TAG.SPRITE, currency.ToStr());
            Profit();
        }
        [Impl(256)] public void ResetProfit()
        {
            _visible.profit = false;
            _currencyText.text = _defaultCurrencyText;
            _idText.color = GameContainer.UI.Colors.TextDefault;
            SetActive();
        }

        [Impl(256)] public void SetActive(bool isActive)
        {
            _visible.mode = isActive;
            SetActive();
        }

        [Impl(256)] private void SetActive()
        {
            if (_visible) Show(); else Hide();
        }

        [Impl(256)] private void OnCaptionEnable(bool value)
        {
            _visible.enable = value;
            SetActive();
        }

        private void Show()
        {
            if (!_visible.current)
            {
                _visible.current = true;
                _thisTransform.localRotation = Quaternion.Euler(ANGLE_X, _lastAngle, 0f);
                _thisGameObject.SetActive(true);

                StartCoroutine(ref _fadeCoroutine, Show_Cn());
            }

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
            if (_visible.current)
            {
                _visible.current = false;
                StartCoroutine(ref _fadeCoroutine, Hide_Cn());
            }

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

        [Impl(256)] private void StartCoroutine(ref Coroutine coroutine, IEnumerator rutune)
        {
            if (coroutine != null)
                _mono.StopCoroutine(coroutine);

            coroutine = _mono.StartCoroutine(rutune);
        }
        [Impl(256)] private void StopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                _mono.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private void OnChangeCamera(Transform transform)
        {
            bool showDistance = transform.position.y > CameraController.heightShow;
            if (_visible.distance != showDistance)
            {
                _visible.distance = showDistance;
                SetActive();
            }

            _lastAngle = transform.eulerAngles.y;
            if (_visible.current && (_idTextRenderer.isVisible & _currencyTextRenderer.isVisible))
                _thisTransform.localRotation = Quaternion.Euler(ANGLE_X, _lastAngle, 0f);
        }

        public void OnDestroy()
        {
            _subscription?.Dispose();
        }

        // Nested
        private struct Visible
        {
            public bool enable;

            public bool mode;
            public bool profit;
            public bool distance;
            public bool newId;

            public bool current;

            public Visible(bool active)
            {
                current = enable = active;
                mode = profit = distance = newId = false;
            }

            [Impl(256)] public static implicit operator bool(Visible self) => (self.mode | self.profit | self.distance | self.newId) & self.enable;
        }

#if UNITY_EDITOR
        public void OnValidate(Component parent)
        {
            parent.SetChildren(ref _thisTransform, "Caption");
            parent.SetChildren(ref _currencyText, "Currency_Text(TMP)");
            parent.SetChildren(ref _idText, "Id_Text(TMP)");

            _thisGameObject = _thisTransform.gameObject;
            _thisTransform.localRotation = Quaternion.Euler(ANGLE_X, 0f, 0f);
        }
#endif
    }
}
