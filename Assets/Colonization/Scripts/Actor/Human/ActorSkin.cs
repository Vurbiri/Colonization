using UnityEngine;

namespace Vurbiri.Colonization
{
    public class ActorSkin : MonoBehaviour
    {
        private Animator _animator;
        private float _time = 12.5f;
        private readonly int _damageId = Animator.StringToHash("tDamage");
        private readonly int[] _ids 
            = new int[] { Animator.StringToHash("tIdleSwitch_01"), Animator.StringToHash("tIdleSwitch_02"), Animator.StringToHash("tIdleSwitch_03") };

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            _time -= Time.fixedDeltaTime;

            if (_time > 0)
                return;
            _time = Random.Range(10f, 20f);
            //_animator.SetTrigger(_damageId);
            _animator.SetTrigger(_ids.Rand());
        }
    }
}
