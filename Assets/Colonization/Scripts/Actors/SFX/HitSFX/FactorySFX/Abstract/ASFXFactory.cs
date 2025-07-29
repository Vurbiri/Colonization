using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class ASFXFactory : ScriptableObject
    {
        [SerializeField] private string _name;

        public string Name => _name;

        public abstract IHitSFX Create();

#if UNITY_EDITOR

        [HideInInspector] public int nameIndex_ed = -1;

        protected virtual void OnValidate()
        {
            if(Application.isPlaying) return;
            
            if (string.IsNullOrEmpty(_name))
            {
                var name = this.name.Delete("ASFX_");
                _name = name;
                Debug.Log($"[FactorySFX] Задано имя <b>\"{_name}\"</b>.");
            }

            ValidateName_Ed();
        }

        private async void ValidateName_Ed()
        {
            do
                await System.Threading.Tasks.Task.Delay(1);
            while (SFXFactoriesStorage.names_ed == null) ;

            if (nameIndex_ed < 0)
            {
                SFXFactoriesStorage.UpdateS_Ed();
            }

            if (nameIndex_ed > 0)
            {
                var names = SFXFactoriesStorage.names_ed;
                var oldName = names[nameIndex_ed];

                if (oldName != _name)
                {
                    names[nameIndex_ed] = string.Empty;

                    for (int i = names.Length - 1; i >= 0; i--)
                    {
                        if (_name == names[i])
                        {
                            Debug.LogWarning($"[FactorySFX] Имя <b>\"{names[i]}\"</b> занято.");
                            _name = name.Delete("ASFX_").Concat("_", nameIndex_ed.ToString("D2"));
                            i = names.Length;
                        }
                    }

                    Debug.LogWarning($"[FactorySFX] Имя <b>\"{oldName}\"</b> заменено на <b>\"{_name}\"</b>.");
                    SFXFactoriesStorage.names_ed[nameIndex_ed] = _name;
                }
            }
        }
#endif
    }
}
