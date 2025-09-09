using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract class ASFXFactory : ScriptableObject
    {
        [SerializeField, Delayed] private string _name;

        public string Name => _name;

        public abstract IHitSFX Create();



#if UNITY_EDITOR

        protected const string FILE_NAME = "ASFX_", MENU = "Vurbiri/ActorSFX/";
        protected const int ORDER = 49;

        [HideInInspector] public int index_ed = -1;

        public abstract TargetForSFX_Ed Target_Ed { get; }

        protected virtual void OnValidate()
        {
            if (!Application.isPlaying)
            {
                ValidateName_Ed();
            }
        }

        private async void ValidateName_Ed()
        {
            do
            {
                await System.Threading.Tasks.Task.Delay(2);
                if (Application.isPlaying) return;
            }
            while (SFXFactoriesStorage.names_ed == null);


            if (string.IsNullOrEmpty(_name))
            {
                _name = this.name.Delete("ASFX_");
                Debug.Log($"[FactorySFX] Задано имя <b>\"{_name}\"</b>.");
            }

            if (index_ed < 0)
                SFXFactoriesStorage.UpdateS_Ed();

            if (index_ed > 0)
            {
                var names = SFXFactoriesStorage.names_ed;
                for (int i = names.Length - 1, id = 1; i >= 0; i--)
                {
                    if (index_ed != i & _name == names[i])
                    {
                        Debug.LogWarning($"[FactorySFX] Имя <b>\"{names[i]}\"</b> занято.");
                        _name = name.Delete("ASFX_").Concat("_", (id++).ToString("D2"));
                        i = names.Length;
                    }
                }

                SFXFactoriesStorage.SetName_Ed(index_ed, _name);
            }
        }
#endif
    }
}
