using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Image))]
    public class Shading : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private float _prePause = 0f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private Image _thisImage;

        private void Awake()
        {
            _thisImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            _thisImage.color = _defaultColor;
            Color targetColor = _defaultColor;
            targetColor.a = 0f;

            yield return new WaitForSecondsRealtime(_prePause);
            yield return _thisImage.Fade(targetColor, _fadeDuration, true);
            gameObject.SetActive(false);

        }
    }
}
