using UnityEngine;

namespace Vurbiri.Colonization
{
    public class HumansMaterials
    {
        private readonly HumanMaterials[] _humansVisual = new HumanMaterials[PlayerId.Count];

        public HumanMaterials this[int index] => _humansVisual[index];
        public HumanMaterials this[Id<PlayerId> id] => _humansVisual[id.Value];

        public HumansMaterials(PlayerColors colors, Material materialLit, Material materialUnlit, Material materialWarriors)
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                _humansVisual[i] = new(new(materialLit), new(materialUnlit), new(materialWarriors));
                colors.Subscribe(i, _humansVisual[i].SetColor);
            }
        }
    }
}
