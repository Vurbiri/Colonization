using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WindmillBladesRotation : MonoBehaviour
    {
        [SerializeField] private FloatRnd _speed = new(90f, 140f);

        private Quaternion _rotation;
        private Transform _thisTransform;
        
        private void Awake()
        {
            _thisTransform = GetComponent<Transform>();
            _rotation = Quaternion.Euler(0f, 0f, _speed * Time.fixedDeltaTime);
        }

        private void FixedUpdate()
        {
            _thisTransform.localRotation *= _rotation;
        }
    }
}
