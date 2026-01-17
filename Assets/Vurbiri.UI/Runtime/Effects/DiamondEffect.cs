using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    [AddComponentMenu("UI/Effects/Diamond")]
    sealed public class DiamondEffect : AMeshEffect
    {
        [SerializeField] private Color _leftTopColor = Color.white;
        [SerializeField] private Color _rightTopColor = Color.white;
        [SerializeField] private Color _leftBottomColor = Color.black;
        [SerializeField] private Color _rightBottomColor = Color.black;

        public Color LeftTopColor
        { 
			[Impl(256)] get => _leftTopColor; 
			[Impl(256)] set
			{
				if(_leftTopColor != value)
				{
                    _leftTopColor = value;
					_graphic.SetVerticesDirty();
				}
			}
		}
        public Color RightTopColor
        { 
			[Impl(256)] get => _rightTopColor; 
			[Impl(256)] set
			{
				if(_rightTopColor != value)
				{
                    _rightTopColor = value;
					_graphic.SetVerticesDirty();
				}
			}
		}
        public Color LeftBottomColor
        { 
			[Impl(256)] get => _leftBottomColor; 
			[Impl(256)] set
			{
				if(_leftBottomColor != value)
				{
                    _leftBottomColor = value;
					_graphic.SetVerticesDirty();
				}
			}
		}
        public Color RightBottomColor
        { 
			[Impl(256)] get => _rightBottomColor; 
			[Impl(256)] set
			{
				if(_rightBottomColor != value)
				{
                    _rightBottomColor = value;
					_graphic.SetVerticesDirty();
				}
			}
		}

        [Impl(256)] public void Set(Color leftTop, Color rightTop, Color leftBottom, Color rightBottom)
        {
            _leftTopColor = leftTop; _rightTopColor = rightTop;
            _leftBottomColor = leftBottom; _rightBottomColor = rightBottom;
            _graphic.SetVerticesDirty();
        }
        [Impl(256)] public void SetTop(Color leftTop, Color rightTop)
        {
            _leftTopColor = leftTop; _rightTopColor = rightTop;
            _graphic.SetVerticesDirty();
        }
        [Impl(256)] public void SetBottom(Color leftBottom, Color rightBottom)
        {
            _leftBottomColor = leftBottom; _rightBottomColor = rightBottom;
            _graphic.SetVerticesDirty();
        }
        [Impl(256)] public void SetLeft(Color leftTop, Color leftBottom)
        {
            _leftTopColor = leftTop; _leftBottomColor = leftBottom;
            _graphic.SetVerticesDirty();
        }
        [Impl(256)] public void SetRight(Color rightTop, Color rightBottom)
        {
            _rightTopColor = rightTop; _rightBottomColor = rightBottom;
            _graphic.SetVerticesDirty();
        }
        [Impl(256)] public void SetLeftTopAndRightBottom(Color leftTop, Color rightBottom)
        {
            _leftTopColor = leftTop; _rightBottomColor = rightBottom;
            _graphic.SetVerticesDirty();
        }
        [Impl(256)] public void SetRightTopAndLeftBottom(Color rightTop, Color leftBottom)
        {
             _rightTopColor = rightTop; _leftBottomColor = leftBottom; 
            _graphic.SetVerticesDirty();
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!isActiveAndEnabled)
                return;

            int count = vh.currentIndexCount;
            List<UIVertex> vertexes = new(count);
            vh.GetUIVertexStream(vertexes);

            Points points = new(vertexes);

            UIVertex uiVertex;
            for (int i = 0; i < count; ++i)
            {
                uiVertex = vertexes[i];
                uiVertex.color = Lerp(vertexes[i].position, points.min, points.size);
                vertexes[i] = uiVertex;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexes);
        }

        private Color32 Lerp(Vector3 position, Vector2 min, Vector2 size)
        {
            var x = (position.x - min.x) / size.x;
            var y = (position.y - min.y) / size.y;
            var top    = Color.Lerp(_leftTopColor, _rightTopColor, x);
            var bottom = Color.Lerp(_leftBottomColor, _rightBottomColor, x);
            return Color.Lerp(bottom, top, y).ToColor32();
        }

        // ************** Nested *********************
        private readonly struct Points
        {
            public readonly Vector2 min;
            public readonly Vector2 size;

            public Points(List<UIVertex> vertexes)
            {
                var xMinMax = MinMax.Empty;
                var yMinMax = MinMax.Empty;

                Vector3 current;
                for (int i = vertexes.Count - 1; i >= 0; --i)
                {
                    current = vertexes[i].position;
                    xMinMax.Set(current.x);
                    yMinMax.Set(current.y);
                }

                size.x = xMinMax.Delta;
                size.y = yMinMax.Delta;

                min.x = xMinMax.Min;
                min.y = yMinMax.Min;
            }
        }

#if UNITY_EDITOR
        public const string leftTopField = nameof(_leftTopColor);
        public const string rightTopField = nameof(_rightTopColor);
        public const string leftBottomField = nameof(_leftBottomColor);
        public const string rightBottomField = nameof(_rightBottomColor);
#endif
    }
}
