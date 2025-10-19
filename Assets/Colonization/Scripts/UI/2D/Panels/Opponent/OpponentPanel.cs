using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class OpponentPanel : AHintElement 
	{
        private const string SEPARATOR = "|";

        [SerializeField, Id(typeof(PlayerId))] private int _id;
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _diplomacy;
        [SerializeField] private Image _indicator;
        [SerializeField] private Image _indicatorTurn;
        [SerializeField, Range(0.1f, 2f)] private float _indicatorSpeed;
        [SerializeField, Range(1f, 8f)] private float _fadeSpeed = 5f;
        [Space]
        [SerializeField] private Sprite _friend;
        [SerializeField] private Sprite _enemy;
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        private Subscription _unsub;
        private int _relation, _min, _max;
        private bool _run;

        public void Init(Vector3 offsetPopup, Diplomacy diplomacy)
        {
            _popup.Init(offsetPopup);
            base.InternalInit(GameContainer.UI.CanvasHint);

            _min = diplomacy.Min; _max = diplomacy.Max;

            diplomacy.Subscribe(OnDiplomacy, false);
            SetRelation(diplomacy.GetPersonRelation(_id));

            _icon.color = GameContainer.UI.PlayerColors[_id]; _icon = null;
            _unsub = GameContainer.UI.PlayerNames.Subscribe(_ => SetRelation(_relation), false);

            _indicator.canvasRenderer.SetAlpha(0f); _indicatorTurn.canvasRenderer.SetAlpha(0f);
            GameContainer.Players.Humans[_id].Interactable.Subscribe(IndicatorTurn);
        }

        private void OnDiplomacy(Diplomacy diplomacy)
        {
            int relation = diplomacy.GetPersonRelation(_id);

            if (_relation != relation)
            {
                _popup.ForceRun(relation - _relation);
                SetRelation(relation);
            }
        }

        private void SetRelation(int relation)
        {
            _relation = relation;

            StringBuilder sb = new(64);
            sb.AppendLine(GameContainer.UI.PlayerNames[_id]);
            sb.AppendLine(CONST_UI.SEPARATOR);

            if (relation > 0)
            {
                sb.Append(GameContainer.UI.Colors.TextPositiveTag);
                sb.Append(relation); sb.Append(SEPARATOR); sb.Append(_max);
                _diplomacy.sprite = _friend;
            }
            else
            {
                sb.Append(GameContainer.UI.Colors.TextNegativeTag);
                sb.Append(relation - 1); sb.Append(SEPARATOR); sb.Append(_min);
                _diplomacy.sprite = _enemy;
            }
            
            _hintText = sb.ToString();
        }

        private void IndicatorTurn(bool run)
        {
            _run = run;
            if (run)
                StartCoroutine(IndicatorTurn_Cn());

            // ===== Local =====
            IEnumerator IndicatorTurn_Cn()
            {
                float start = 0f, end = 1f, progress, sign;
                _indicator.fillClockwise = true;
                _indicator.canvasRenderer.SetAlpha(1f);
                _indicatorTurn.canvasRenderer.SetAlpha(1f);

                while (_run)
                {
                    progress = 0f; sign = end - start;

                    do
                    {
                        progress += Time.unscaledDeltaTime * _indicatorSpeed;
                        _indicator.fillAmount = start + sign * progress;
                        yield return null;
                    }
                    while (_run & progress < 1f) ;

                    (start, end) = (end, start);
                    _indicator.fillClockwise = !_indicator.fillClockwise;
                }
                _indicator.canvasRenderer.SetAlpha(0f);
                _indicatorTurn.canvasRenderer.SetAlpha(0f);
            }
        }

        private void OnDestroy()
        {
            _unsub?.Dispose();
        }

#if UNITY_EDITOR

        public Vector2 UpdateVisuals_Editor(float offset)
        {
            var rectTransform = (RectTransform)transform.parent;
            rectTransform.anchoredPosition = new(offset, 0f);

            return rectTransform.sizeDelta;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            this.SetChildren(ref _icon, "Icon");
            this.SetChildren(ref _diplomacy, "Relation");
            this.SetChildren(ref _indicator, "Indicator");
            this.SetChildren(ref _indicatorTurn, "IndicatorTurn");

            EUtility.SetAsset(ref _friend, "SP_IconFriend");
            EUtility.SetAsset(ref _enemy, "SP_IconEnemy");
        }
#endif
    }
}
