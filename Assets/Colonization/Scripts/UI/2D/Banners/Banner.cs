using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vurbiri
{
    //[RequireComponent(typeof(Image), typeof(HorizontalLayoutGroup), typeof(Outline))]
    public class Banner //: PooledObject<Banner>
    {
        [SerializeField] private TextMeshProUGUI _text;

        private Banners _banners;

        private Image _image;
        private Coroutine _coroutine;
        private bool _isThrough;

        public void Init()
        {

            _banners = Banners.Instance;
            //_image = GetComponent<Image>();
            //HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();

            float size = _banners.FontSize;
            _text.fontSize = size;
            size /= 2f;
            //GetComponent<Outline>().effectDistance = Vector2.one * size;
            //layoutGroup.spacing = size;
            //int iSize = Mathf.RoundToInt(size);
            //layoutGroup.padding.left = iSize;
            //layoutGroup.padding.right = iSize;
            //layoutGroup.padding.top = iSize;
            //layoutGroup.padding.bottom = iSize;
        }

        public void Setup(string message, MessageType messageType, float time, bool isThrough)
        {
            _isThrough = isThrough;
            _text.text = message;
            _image.color = _banners.Colors[(int)messageType];

            //_gameObject.SetActive(true);
            //_coroutine = StartCoroutine(TimeShow());
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            //IEnumerator TimeShow()
            //{
            //    yield return new WaitForSecondsRealtime(time);
            //    ToPool();
            //}
        }

        //public override void ToPool(bool worldPositionStays = true)
        //{
        //    SceneManager.sceneUnloaded -= OnSceneUnloaded;
        //    if (_coroutine != null)
        //    {
        //        StopCoroutine(_coroutine);
        //        _coroutine = null;
        //    }
        //    base.ToPool(worldPositionStays);
        //}

        private void OnSceneUnloaded(Scene scene)
        {
            if (_isThrough)
                return;

            //ToPool();
        }
    }
}
