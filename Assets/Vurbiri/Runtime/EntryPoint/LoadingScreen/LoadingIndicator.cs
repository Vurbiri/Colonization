using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Image))]
    public class LoadingIndicator : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)] private float _speed = 0.5f;
        
        private Image _thisImage;

        private void Awake()
        {
            _thisImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            StartCoroutine(Turn_Coroutine());
        }

        private IEnumerator Turn_Coroutine()
        {
            float fill;

            while (true)
            {
                fill = 0f;
                _thisImage.fillClockwise = true;

                while (fill < 1f)
                {
                    _thisImage.fillAmount = fill += Time.unscaledDeltaTime * _speed;
                    yield return null;
                   
                }

                fill = 1f;
                _thisImage.fillClockwise = false;

                while (fill > 0f)
                {
                    _thisImage.fillAmount = fill -= Time.unscaledDeltaTime * _speed;
                    yield return null;
                }
            }
        }
    }
}