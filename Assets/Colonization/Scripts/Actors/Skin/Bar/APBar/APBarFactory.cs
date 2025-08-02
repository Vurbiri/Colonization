using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors.UI
{
	public class APBarFactory : MonoBehaviour
	{
        [SerializeField] private TextMeshPro _maxValueTMP;
        [SerializeField] private TextMeshPro _currentValueTMP;

        public APBar Get(AbilitiesSet<ActorAbilityId> abilities, ReadOnlyIdArray<ActorAbilityId, Color> colors, int orderLevel)
        {
            _maxValueTMP.sortingOrder += orderLevel;
            _currentValueTMP.sortingOrder += orderLevel;

            _maxValueTMP.color = colors[ActorAbilityId.MaxAP];
            _currentValueTMP.color = colors[ActorAbilityId.CurrentAP];

            Destroy(this);

            return new(_maxValueTMP, _currentValueTMP, abilities);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _maxValueTMP);
            this.SetChildren(ref _currentValueTMP, "CurrentAP_TMP");
        }
#endif
	}
}
