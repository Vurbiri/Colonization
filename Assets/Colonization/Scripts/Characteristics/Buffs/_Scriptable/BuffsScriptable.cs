using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public class BuffsScriptable : ScriptableObject
    {
        [SerializeField] private int _maxLevel;
        [SerializeField] private List<BuffSettings> _settings;

        public int MaxLevel => _maxLevel;
        public List<BuffSettings> Settings => _settings;

#if UNITY_EDITOR
        public void SetValues_Ed(int maxLevel, IReadOnlyList<BuffSettings> values)
        {
            _maxLevel = maxLevel;

            _settings = new();
            for (int i = 0; i < ActorAbilityId.Count; i++)
            {
                if (values[i].typeModifier >= 0)
                    _settings.Add(values[i]);
            }
        }

        public void Sort_Ed()
        {
            for (int i = 0, index; i < _settings.Count;)
            {
                index = AbilityToIndex(_settings[i].targetAbility, i);
                if (index != i)
                    (_settings[i], _settings[index]) = (_settings[index], _settings[i]);
                else
                    i++;
            }

            // Local
            static int AbilityToIndex(int ability, int index) => ability switch
            {
                ActorAbilityId.Defense   => 0,
                ActorAbilityId.HPPerTurn => 1,
                ActorAbilityId.Attack    => 2,
                ActorAbilityId.Pierce    => 3,
                _ => index
            };
        }
#endif
    }
}
