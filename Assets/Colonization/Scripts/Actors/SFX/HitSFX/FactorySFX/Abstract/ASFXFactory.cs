using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class ASFXFactory : ScriptableObjectDisposable
    {
        [SerializeField] private string _name;

        public string Name => _name;

        public abstract IHitSFX Create();

#if UNITY_EDITOR

        [HideInInspector] public int nameIndex = -1;

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(_name))
            {
                var name = this.name.Delete("ASFX_");
                _name = name;
            }

            if (nameIndex > 0)
            {
                var names = SFXFactoriesStorage.names_ed;
                names[nameIndex] = string.Empty;
                for (int i = names.Length - 1; i >= 0; i--)
                {
                    if (_name == names[i])
                    {
                        Debug.LogWarning($"[FactorySFX] Имя <b>{names[i]}</b> занято.");
                        _name = name.Delete("ASFX_").Concat("_", nameIndex.ToString("D2"));
                        i = names.Length;
                    }
                }

                SFXFactoriesStorage.names_ed[nameIndex] = _name;
            }
        }
#endif
    }
}
