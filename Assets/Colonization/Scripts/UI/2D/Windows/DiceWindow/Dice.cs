using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
	public class Dice : MonoBehaviour
	{
		[SerializeField] private FloatRnd _time;

		private readonly WaitRealtime _waitTime = new();
		private RandomSequence _roll;

        private bool _isPlaying = false;
        private TextMeshProUGUI _label;
        private int _current;

        public void Init()
        {
            _label = GetComponentInChildren<TextMeshProUGUI>();
			_roll = new(CONST.DICE);
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
					_label.text = CONST.NUMBERS_STR[_current];
					yield return _waitTime.Restart(_time);
				}
			}
		}

		public int Stop()
		{
			_isPlaying = false;
			return _current;
		}
	}
}
