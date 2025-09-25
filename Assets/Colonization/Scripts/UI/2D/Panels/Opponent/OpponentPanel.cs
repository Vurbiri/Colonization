using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Reactive;
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
        [Space]
        [SerializeField] private Sprite _friend;
        [SerializeField] private Sprite _enemy;
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        private Subscription _unsub;
        private int _relation, _min, _max;

        public void Init(Vector3 offsetPopup, Diplomacy diplomacy)
        {
            _popup.Init(offsetPopup);
            base.InternalInit(GameContainer.UI.CanvasHint);

            _min = diplomacy.Min; _max = diplomacy.Max;

            diplomacy.Subscribe(OnDiplomacy, false);
            SetRelation(diplomacy.GetPersonRelation(_id));

            _icon.color = GameContainer.UI.PlayerColors[_id]; _icon = null;
            _unsub = GameContainer.UI.PlayerNames.Subscribe(_ => SetRelation(_relation), false);
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

        private void OnDestroy()
        {
            _unsub?.Dispose();
        }

#if UNITY_EDITOR

        public Vector2 UpdateVisuals_Editor(PlayerColors playerColors, float offset)
        {
            _icon.color = playerColors[_id];

            var rectTransform = (RectTransform)transform.parent;
            rectTransform.anchoredPosition = new(offset, 0f);

            return rectTransform.sizeDelta;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            
            this.SetChildren(ref _icon, "Icon");
            this.SetChildren(ref _diplomacy, "Relation");

            EUtility.SetAsset(ref _friend, "SP_IconFriend");
            EUtility.SetAsset(ref _enemy, "SP_IconEnemy");
        }
#endif
    }
}
