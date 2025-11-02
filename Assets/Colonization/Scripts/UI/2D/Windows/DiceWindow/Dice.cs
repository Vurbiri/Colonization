using System.Collections;
using TMPro;

namespace Vurbiri.Colonization
{
	public class Dice
	{
        private readonly WaitRealtime _waitTime = new();
        private readonly RandomSequence _roll = new(CONST.DICE);
        private readonly TextMeshProUGUI _label;

        private bool _isPlaying = false;
        private int _current;

        public Dice(TextMeshProUGUI label)
        {
            _label = label; 
        }

        public void Run(float time)
        {
            _isPlaying = true;
            _waitTime.Time = time;
            _label.StartCoroutine(Run_Cn());

            // === Local ===
            IEnumerator Run_Cn()
            {
                while (_isPlaying)
                {
                    _current = _roll.Next;
                    _label.text = _current.ToStr();
                    yield return _waitTime.Restart();
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
