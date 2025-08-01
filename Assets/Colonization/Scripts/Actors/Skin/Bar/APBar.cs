using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class APBar : MonoBehaviour
	{
		private const char CHAR = '+';
		
		[SerializeField] private TextMeshPro _maxValueTMP;
        [SerializeField] private TextMeshPro _currentValueTMP;

        private Unsubscriptions _unsubscribers;

        public void Init(AbilitiesSet<ActorAbilityId> abilities, ReadOnlyIdArray<ActorAbilityId, Color> colors, int orderLevel)
        {
            _maxValueTMP.sortingOrder += orderLevel;
            _currentValueTMP.sortingOrder += orderLevel;

            _maxValueTMP.color = colors[ActorAbilityId.MaxAP];
            _currentValueTMP.color = colors[ActorAbilityId.CurrentAP];

            _unsubscribers += abilities[ActorAbilityId.MaxAP].Subscribe(SetMaxValue);
            _unsubscribers += abilities[ActorAbilityId.CurrentAP].Subscribe(SetCurrentValue);
        }

        private void SetMaxValue(int value) => _maxValueTMP.text = new(CHAR, value);
        private void SetCurrentValue(int value) => _currentValueTMP.text = new(CHAR, value);

        private void OnDestroy()
        {
            _unsubscribers?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_maxValueTMP == null || _currentValueTMP == null)
            {
                TextMeshPro[] TMPs = GetComponentsInChildren<TextMeshPro>();

                _maxValueTMP = TMPs[0];
                _currentValueTMP = TMPs[1];
            }
        }
#endif
	}
}
