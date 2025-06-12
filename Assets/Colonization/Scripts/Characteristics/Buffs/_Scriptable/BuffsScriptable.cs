using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    //[CreateAssetMenu(fileName = "Buffs", menuName = "Vurbiri/Colonization/Buffs/Buffs", order = 51)]
    sealed public class BuffsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private int _maxLevel;
        [SerializeField] private List<BuffSettings> _settings;

        public int MaxLevel => _maxLevel;
        public List<BuffSettings> Settings => _settings;

#if UNITY_EDITOR
        public void SetValues_EditorOnly(int maxLevel, IReadOnlyList<BuffSettings> values)
        {
            _maxLevel = maxLevel;

            _settings = new();
            for (int i = 0; i < ActorAbilityId.Count; i++)
            {
                if (values[i].typeModifier >= 0)
                    _settings.Add(values[i]);
            }
        }
#endif
    }
}
