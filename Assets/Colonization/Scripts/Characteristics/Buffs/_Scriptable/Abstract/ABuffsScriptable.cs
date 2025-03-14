//Assets\Colonization\Scripts\Characteristics\Buffs\_Scriptable\Abstract\ABuffsScriptable.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuffsScriptable<T> : ScriptableObjectDisposable where T : BuffSettings
    {
        [SerializeField] private List<T> _settings;

        public IReadOnlyList<T> Settings => _settings;

#if UNITY_EDITOR
        public void SetValues_EditorOnly(IReadOnlyList<T> values)
        {
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
