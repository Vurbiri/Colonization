//Assets\Colonization\Scripts\Characteristics\Buffs\_Scriptable\Abstract\ABuffsScriptable.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuffsScriptable<T> : ScriptableObjectDisposable where T : BuffSettings
    {
        [SerializeField] private List<T> _settings = new();

        public IReadOnlyList<T> Settings => _settings;

#if UNITY_EDITOR
        public List<T> EditorOnlySettings { get => _settings; set => _settings = value; }
#endif
    }
}
