using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.UTILITY_MENU_NAME + "FPS/Counter", VUI_CONST_ED.UTILITY_MENU_ORDER)]
    [RequireComponent(typeof(TextMeshProUGUI))]
#endif
    public class FPSCounter : MonoBehaviour
    {
        private const string TEXT = "FPS: {0,3}\nAvg: {1,4:N1}\nMax: {2,3}\nMin: {3,3}";

        [SerializeField] private float _updateInterval = 0.2f;
        [SerializeField] private int _cacheMaxSize = 128;
        [SerializeField] private FPSGraph _graph;

        private TextMeshProUGUI _thisTextFPS;
        private GameObject _graphObject;
        private float _time = 0f;
        private int _frames = 0;
        private Queue<int> _cache;

        private void Start()
        {
            _thisTextFPS = GetComponent<TextMeshProUGUI>();
            if (_graph != null)
            {
                _graphObject = _graph.gameObject;
                _cacheMaxSize = _graph.Width;
            }

            _cache = new(_cacheMaxSize);
        }

        public void SwitchGraphEnable()
        {
            if (_graphObject != null)
                _graphObject.SetActive(!_graphObject.activeSelf);
        }

        private void Update()
        {
            _time += Time.unscaledDeltaTime;
            ++_frames;

            if (_time < _updateInterval)
                return;

            int current = MathI.Round(_frames / _time);
            _time = 0f; _frames = 0;

            if (_cache.Count == _cacheMaxSize)
                _cache.Dequeue();
            _cache.Enqueue(current);

            int max = int.MinValue;
            int min = int.MaxValue;
            float avg = 0f;

            foreach (int fps in _cache)
            {
                avg += fps;
                max = MathI.Max(max, fps);
                min = MathI.Min(min, fps);
            }

            avg /= _cache.Count;
            _thisTextFPS.text = string.Format(TEXT, current, avg, max, min);

            if (_graphObject != null && _graphObject.activeSelf)
                _graph.UpdateTexture(_cache, avg, max, min);

        }
    }
}
