using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	[AddComponentMenu("UI/Effects/Gradient")]
	sealed public class GradientEffect : AMeshEffect
	{
		private const int HORIZONT = 0, VERTICAL = 1;

		[SerializeField] private Color _startColor = Color.white;
		[SerializeField] private Color _endColor = Color.black;
		[SerializeField] private bool _isVertical;

		public Color StartColor 
		{ 
			[Impl(256)] get => _startColor; 
			[Impl(256)] set
			{
				if(_startColor != value)
				{
					_startColor = value;
					_graphic.SetVerticesDirty();
				}
			}
		}

		public Color EndColor 
		{ 
			[Impl(256)] get => _endColor; 
			[Impl(256)] set
			{
				if(_endColor != value)
				{
					_endColor = value;
					_graphic.SetVerticesDirty();
				}
			}
		}

		public bool IsVertical
		{ 
			[Impl(256)] get => _isVertical; 
			[Impl(256)] set
			{
				if(_isVertical ^ value)
				{
					_isVertical = value;
					_graphic.SetVerticesDirty();
				}
			}
		}

        [Impl(256)]
        public void Set(Color start, Color end, bool isVertical = false)
		{
			_startColor = start; _endColor = end;
			_isVertical = isVertical; 
			_graphic.SetVerticesDirty();
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!isActiveAndEnabled)
				return;

			int axis = _isVertical ? VERTICAL : HORIZONT;
            int count = vh.currentIndexCount;
            List<UIVertex> vertexes = new(vh.currentIndexCount);
			vh.GetUIVertexStream(vertexes);

            MinMax extreme = MinMax.empty;
            for (int i = 0; i < count; ++i)
                extreme.Set(vertexes[i].position[axis]);

            float delta = extreme.Delta; UIVertex uiVertex;
            for (int i = 0; i < count; ++i)
            {
                uiVertex = vertexes[i];
                uiVertex.color = Color.Lerp(_startColor, _endColor, (vertexes[i].position[axis] - extreme.Min) / delta).ToColor32();
                vertexes[i] = uiVertex;
            }

            vh.Clear();
			vh.AddUIVertexTriangleStream(vertexes);
		}
	}
}
