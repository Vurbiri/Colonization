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

        private int _relation = int.MaxValue;

        public void Init()
        {
            base.InternalInit(GameContainer.UI.CanvasHint);
            GameContainer.Diplomacy.Subscribe(SetRelation);

            _icon.color = GameContainer.UI.PlayerColors[_id]; _icon = null;
        }

        public void SetRelation(Diplomacy diplomacy)
        {
            int relation = diplomacy.GetPersonRelation(_id);

            if (_relation != relation)
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
