using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FPSGraph : MonoBehaviour
{
    [SerializeField] private int _width = 128;
    [SerializeField] private int _height = 64;
    [SerializeField] private FilterMode _filterMode = FilterMode.Bilinear;
    [Space]
    [SerializeField] private Color _colorBackground = Color.white;
    [SerializeField] private Color _colorGraph = Color.black;
    [SerializeField] private Color _colorAvg = Color.blue;
    [SerializeField] private Color _colorMax = Color.green;
    [SerializeField] private Color _colorMin = Color.red;

    private Texture2D _texture;

    public int Size => _width;

    private void Start()
    {
        _texture = new(_width, _height)
        {
            name = "FPSGraph",
            filterMode = _filterMode
        };

        for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
                _texture.SetPixel(i, j, _colorBackground);
        _texture.Apply();

        GetComponent<RawImage>().texture = _texture;
    }

    public void UpdateTexture(IReadOnlyCollection<int> values, float fpsAvg, int fpsMax, int fpsMin)
    {
        if (_texture == null)
            return;
        
        //ClearTexture();

        int count = values.Count;
        float scale = _height / (fpsMax * 1.15f);
        int x, y, yAvg = Mathf.RoundToInt(fpsAvg * scale), yMax = Mathf.RoundToInt(fpsMax * scale), yMin = Mathf.RoundToInt(fpsMin * scale);

        foreach (int value in values)
        {
            x = _width - count--;

            for (int j = 0; j < _height; j++)
                _texture.SetPixel(x, j, _colorBackground);

            y = Mathf.RoundToInt(value * scale);
            _texture.SetPixel(x, y, _colorGraph);
            _texture.SetPixel(x, yAvg, _colorAvg);
            _texture.SetPixel(x, yMax, _colorMax);
            _texture.SetPixel(x, yMin, _colorMin);
        }

        for (x = 0; x < count; x++)
        {
            for (int j = 0; j < _height; j++)
                _texture.SetPixel(x, j, _colorBackground);

            _texture.SetPixel(x, yAvg, _colorAvg);
            _texture.SetPixel(x, yMax, _colorMax);
            _texture.SetPixel(x, yMin, _colorMin);
        }

        _texture.Apply();
    }
}
