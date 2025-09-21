using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class Dice : MonoBehaviour
	{
		[SerializeField] private FloatRnd _time;
		
		private readonly WaitRealtime _waitTime = new();
		private readonly RandomSequence _roll = new(CONST.DICE);

        private bool _isPlaying = false;
        private string[] _numbers;
        private TextMeshProUGUI _label;
        private int _current;

        public void Init(string[] numbers)
        {
            _numbers = numbers;
            _label = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Run()
		{
            _isPlaying = true;
			StartCoroutine(Run_Cn());

			// Local
            IEnumerator Run_Cn()
			{
				while (_isPlaying)
				{
					_current = _roll.Next;
					_label.text = _numbers[_current];
					yield return _waitTime.Restart(_time);
				}
			}
		}

		public int Stop()
		{
			_isPlaying = false;
			return _current;
		}
		
#if UNITY_EDITOR
        private void OnValidate()
        {
			
        }
#endif
	}
}
