using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class SFXFactoriesStorage : ScriptableObject
    {
        [SerializeField] private ASFXFactory[] _factories;

        public SFXStorage Create()
        {
            int count = _factories.Length; ASFXFactory factory;
            Dictionary<string, IHitSFX> SFXs = new(count + 1);
            for (int i = 0; i < count; i++)
            {
                factory = _factories[i];
                SFXs.Add(factory.Name, factory.Create());
                Resources.UnloadAsset(factory);
            }
            SFXs.Add(EmptySFX.NAME, new EmptySFX());

            Resources.UnloadAsset(this);

            return new(SFXs);
        }

#if UNITY_EDITOR
        public static string[] names_ed;

        static SFXFactoriesStorage() => SetNames_Ed();

        public static void UpdateS_Ed()
        {
            var self = EUtility.FindAnyScriptable<SFXFactoriesStorage>();
            while (self == null)
                self = EUtility.CreateScriptable<SFXFactoriesStorage>("FactoriesStorage", "Assets/Colonization/HitSFX");
               
            self.Update_Ed();
        }
        public void Update_Ed()
        {
            if (!Application.isPlaying)
            {
                var factories = EUtility.FindScriptables<ASFXFactory>();
                _factories = factories.ToArray();

                SetNames_Ed(factories);
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying || _factories == null || _factories.Length == 0)
            {
                var factories = EUtility.FindScriptables<ASFXFactory>();
                _factories = factories.ToArray();
               
                SetNames_Ed(factories);
            }
        }

        private async static void SetNames_Ed()
        {
            await System.Threading.Tasks.Task.Delay(3);
            if (!Application.isPlaying)
                SetNames_Ed(EUtility.FindScriptables<ASFXFactory>());
        }
        private static void SetNames_Ed(List<ASFXFactory> factories)
        {
            names_ed = new string[factories.Count + 1];
            names_ed[0] = EmptySFX.NAME; ASFXFactory factory;
            for (int i = 0, j = 1; i < factories.Count; i++, j++)
            {
                factory = factories[i];
                names_ed[j] = factory.Name;
                if (factory.nameIndex_ed != j)
                {
                    factory.nameIndex_ed = j;
                    UnityEditor.EditorUtility.SetDirty(factory);
                }
            }
        }
#endif
    }
}
