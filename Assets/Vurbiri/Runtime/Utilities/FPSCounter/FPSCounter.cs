//Assets\Vurbiri\Runtime\Utilities\FPSCounter\FPSCounter.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float _updateInterval = 0.2f;
    [SerializeField] private int _cacheMaxSize = 128;
    [SerializeField] private FPSGraph _graph;

    private TMP_Text _thisTextFPS;
    private float _time = 0f;
    private int _frames = 0;
    private int _fps = 0, _fpsMax = int.MinValue, _fpsMin = int.MaxValue;
    private float _fpsAvg = 0f;
    private Queue<int> _cache;

    private const string TEXT = "FPS: {0}\nAvg: {1:N1}\nMax: {2}\nMin: {3}";

    private void Start()
    {
        _thisTextFPS = GetComponent<TMP_Text>();
        if (_graph != null && _graph.gameObject.activeSelf)
            _cacheMaxSize = _graph.Size;

        _cache = new(_cacheMaxSize);
    }

    private void Update()
    {
        _time += Time.unscaledDeltaTime;
        ++_frames;

        if (_time < _updateInterval)
            return;

        _fps = Mathf.RoundToInt(_frames / _time);
        _time = 0f; _frames = 0;

        if (_cache.Count == _cacheMaxSize)
            _cache.Dequeue();
        _cache.Enqueue(_fps);

        _fpsMax = int.MinValue;
        _fpsMin = int.MaxValue;
        _fpsAvg = 0f;

        foreach (int fps in _cache)
        {
            _fpsAvg += fps;
            _fpsMax = Mathf.Max(_fpsMax, fps);
            _fpsMin = Mathf.Min(_fpsMin, fps);
        }

        _fpsAvg /= _cache.Count;
        _thisTextFPS.text = string.Format(TEXT, _fps, _fpsAvg, _fpsMax, _fpsMin);

        if (_graph != null && _graph.gameObject.activeSelf)
            _graph.UpdateTexture(_cache, _fpsAvg, _fpsMax, _fpsMin);

    }
}
