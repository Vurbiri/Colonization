using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class HumanMaterials
	{
		private readonly Material _materialLit;
		private readonly Material _materialUnlit;
		private readonly Material _materialWarriors;
		private Color _color;

		public Material Lit { [Impl(256)] get => _materialLit; }
		public Material Unlit { [Impl(256)] get => _materialUnlit; }
		public Material Warriors { [Impl(256)] get => _materialWarriors; }
		public Color Color { [Impl(256)] get => _color; }

		public HumanMaterials(Material materialLit, Material materialUnlit, Material materialWarriors)
		{
			_materialLit = materialLit;
			_materialUnlit = materialUnlit;
			_materialWarriors = materialWarriors;
		}

		public void SetColor(Color color)
		{
			_color = color;
			_materialLit.color = color;
			_materialUnlit.color = color;
			_materialWarriors.color = color;
		}
	}
}
