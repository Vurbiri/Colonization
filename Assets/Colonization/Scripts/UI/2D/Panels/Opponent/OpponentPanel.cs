using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class OpponentPanel : AHintElement 
	{
        private const string LINE = "|";

        [SerializeField, Id(typeof(PlayerId))] private int _id;
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _diplomacy;
        [Space]
        [SerializeField] private Sprite _friend;
        [SerializeField] private Sprite _enemy;
        [Space]
        [SerializeField] private PopupTextWidgetUI _popup;

        private int _relation;

        public void Init(Vector3 offsetPopup, Diplomacy diplomacy)
        {
            _popup.Init(offsetPopup);
            base.InternalInit(GameContainer.UI.CanvasHint);

            diplomacy.Subscribe(OnDiplomacy, false);
            SetRelation(diplomacy.GetPersonRelation(_id), diplomacy);

            _icon.color = GameContainer.UI.PlayerColors[_id]; _icon = null;
        }

        private void OnDiplomacy(Diplomacy diplomacy)
        {
            int relation = diplomacy.GetPersonRelation(_id);

            if (_relation != relation)
            {
                _popup.ForceRun(relation - _relation);
                SetRelation(relation, diplomacy);
            }
        }

        private void SetRelation(int relation, Diplomacy diplomacy)
        {
            _relation = relation;

            StringBuilder sb = new(64);
            sb.AppendLine(GameContainer.UI.PlayerNames[_id]);
            sb.AppendLine(CONST_UI.SEPARATOR);

            if (relation > 0)
            {
                sb.Append(GameContainer.UI.Colors.TextPositiveTag);
                sb.Append(relation); sb.Append(LINE); sb.Append(diplomacy.Max);
                _diplomacy.sprite = _friend;
            }
            else
            {
                sb.Append(GameContainer.UI.Colors.TextNegativeTag);
                sb.Append(relation - 1); sb.Append(LINE); sb.Append(diplomacy.Min);
                _diplomacy.sprite = _enemy;
            }

            _hintText = sb.ToString();
        }

#if UNITY_EDITOR
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
