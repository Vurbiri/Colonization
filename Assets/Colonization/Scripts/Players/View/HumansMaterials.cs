using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class HumansMaterials
    {
		private readonly HumanMaterials[] _humansVisual = new HumanMaterials[PlayerId.Count];

		public HumanMaterials this[int index] { [Impl(256)] get => _humansVisual[index]; }
		public HumanMaterials this[Id<PlayerId> id] { [Impl(256)] get => _humansVisual[id.Value]; }

		public HumansMaterials(PlayerColors colors, Material materialLit, Material materialUnlit, Material materialWarriors)
		{
			for (int i = 0; i < PlayerId.HumansCount; ++i)
                _humansVisual[i] = new(new(materialLit), new(materialUnlit), new(materialWarriors));

			colors.Subscribe(SetColors);
        }

        private void SetColors(PlayerColors colors)
		{
			for (int i = 0; i < PlayerId.HumansCount; ++i)
				_humansVisual[i].SetColor(colors[i]);
        }
    }
}
