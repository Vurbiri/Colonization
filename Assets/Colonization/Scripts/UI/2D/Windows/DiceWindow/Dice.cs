using System.Collections;
using TMPro;

namespace Vurbiri.Colonization
{
	public class Dice
	{
        private readonly WaitRealtime _waitTime = new();
        private readonly RandomSequence _roll = new(CONST.DICE);
        private readonly TextMeshProUGUI _label;

        private FloatRnd _time;
        private bool _isPlaying = false;
        private int _current;

        public Dice(TextMeshProUGUI label, FloatRnd time)
        {
            _label = label; _time = time;
        }

        public void Run()
        {
            _isPlaying = true;
            _label.StartCoroutine(Run_Cn());

            // === Local ===
            IEnumerator Run_Cn()
            {
                while (_isPlaying)
                {
                    _current = _roll.Next;
                    _label.text = _current.ToStr();
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
