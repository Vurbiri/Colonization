//Assets\Colonization\Scripts\Actors\Skin\Bar\APBar.cs
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class APBar : MonoBehaviour
	{
		private const char CHAR = '+';
		
		[SerializeField] private TextMeshPro _maxValueTMP;
        [SerializeField] private TextMeshPro _currentValueTMP;

        private Unsubscribers _unsubscribers;

        public void Init(IReadOnlyAbilities<ActorAbilityId> abilities, int orderLevel)
        {
            _maxValueTMP.sortingOrder += orderLevel;
            _currentValueTMP.sortingOrder += orderLevel;

            _unsubscribers += abilities[ActorAbilityId.MaxAP].Subscribe(value => _maxValueTMP.text = new(CHAR, value));
            _unsubscribers += abilities[ActorAbilityId.CurrentAP].Subscribe(value => _currentValueTMP.text = new(CHAR, value));
        }

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
