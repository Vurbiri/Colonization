using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Image))]
    public class LoadingIndicator : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)] private float _speed = 0.5f;
        
        private Image _thisImage;
        private float _fill;

        private void Awake()
        {
            _thisImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _thisImage.fillClockwise = true;
            _thisImage.fillAmount = _fill = 0f;
            _speed = Mathf.Abs(_speed);        
        }

        private void Update()
        {
            _thisImage.fillAmount = _fill += Time.unscaledDeltaTime * _speed;

            if (_fill <= 0f || _fill >= 1f)
            {
                _thisImage.fillClockwise = !_thisImage.fillClockwise;
                _speed *= -1f;
            }
        }
    }
}
