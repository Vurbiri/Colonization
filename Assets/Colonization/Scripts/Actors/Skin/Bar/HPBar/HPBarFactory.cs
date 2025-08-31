using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Actors.UI
{
	public class HPBarFactory : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer _backgroundBar;
        [SerializeField] private SpriteRenderer _barSprite;
        [SerializeField] private SpriteRenderer _hpSprite;
        [SerializeField] private TextMeshPro _maxValueTMP;
        [SerializeField] private TextMeshPro _currentValueTMP;

        public SpriteRenderer Renderer => _backgroundBar;

        public HPBar Get(ReadOnlyAbilities<ActorAbilityId> abilities, ReadOnlyIdArray<ActorAbilityId, Color> colors, PopupWidget3D popup, int orderLevel)
        {
            Vector2 size = new(HPBar.SP_WIDTH, HPBar.SP_HIGHT);

            _barSprite.size = size;
            _backgroundBar.size = size;

            _backgroundBar.color = colors[ActorAbilityId.MaxHP];
            _hpSprite.color = colors[ActorAbilityId.CurrentHP]; ;

            _backgroundBar.sortingOrder += orderLevel;
            _barSprite.sortingOrder += orderLevel;
            _hpSprite.sortingOrder += orderLevel;
            _maxValueTMP.sortingOrder += orderLevel;
            _currentValueTMP.sortingOrder += orderLevel;

            Destroy(this);

            return new(_barSprite, _maxValueTMP, _currentValueTMP, popup, _hpSprite.sprite, abilities);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _backgroundBar);
            this.SetChildren(ref _barSprite, "HP_Bar");
            this.SetChildren(ref _hpSprite, "HP_Sprite");
            this.SetChildren(ref _maxValueTMP, "HPMaxValue_TMP");
            this.SetChildren(ref _currentValueTMP, "HPCurrentValue_TMP");
        }
#endif
	}
}
