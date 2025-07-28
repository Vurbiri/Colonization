using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    //[CreateAssetMenu(fileName = "FactoriesStorage", menuName = "Vurbiri/Colonization/ActorSFX/FactoriesStorage", order = 11)]
    public class SFXFactoriesStorage : ScriptableObject, IDisposable
    {
        [SerializeField] private ASFXFactory[] _factories;

        public void Dispose()
        {
            for(int i = _factories.Length - 1; i >= 0; i--)
                _factories[i].Dispose();

            Resources.UnloadAsset(this);
        }

#if UNITY_EDITOR
        public static string[] names_ed;

        public void Update_Editor()
        {
            var factories = EUtility.FindScriptables<ASFXFactory>();
            _factories = factories.ToArray();

            SetNames(factories);
        }

        static SFXFactoriesStorage() => SetNames();

        private void OnValidate()
        {
            if (_factories == null || _factories.Length == 0)
            {
                var factories = EUtility.FindScriptables<ASFXFactory>();
                _factories = factories.ToArray();
               
                SetNames(factories);
            }
        }

        private static void SetNames(List<ASFXFactory> factories)
        {
            names_ed = new string[factories.Count + 1];
            names_ed[0] = EmptySFX.NAME;
            for (int i = 0, j = 1; i < factories.Count; i++, j++)
            {
                names_ed[j] = factories[i].Name;
                factories[i].nameIndex = j;
            }
        }

        private async static void SetNames()
        {
            await System.Threading.Tasks.Task.Delay(5);
            SetNames(EUtility.FindScriptables<ASFXFactory>());
        }
#endif
    }
}
