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

        private static SFXFactoriesStorage s_self_ed;
        private static WarriorsSettingsScriptable s_warriorsSettings_ed;
        private static DemonsSettingsScriptable s_demonsSettings_ed;

        static SFXFactoriesStorage() => SetStaticField_Ed();

        public static void SetName_Ed(int nameIndex, string newName)
        {
            var oldName = names_ed[nameIndex];
            if (oldName == newName)
                return;

            names_ed[nameIndex] = newName;

            s_warriorsSettings_ed.UpdateName_Ed(oldName, newName);
            s_demonsSettings_ed.UpdateName_Ed(oldName, newName);

            UnityEditor.AssetDatabase.SaveAssets();
            Debug.LogWarning($"[FactorySFX] Имя <b>\"{oldName}\"</b> заменено на <b>\"{newName}\"</b>.");
        }

        public static void UpdateS_Ed()
        {
            while (s_self_ed == null)
                s_self_ed = EUtility.FindAnyScriptable<SFXFactoriesStorage>();

            if (!Application.isPlaying)
                s_self_ed.Update_Ed();
        }
        public void Update_Ed()
        {
            var factories = EUtility.FindScriptables<ASFXFactory>();
            bool isDirty = false;

            EUtility.SetScriptable(ref s_warriorsSettings_ed);
            EUtility.SetScriptable(ref s_demonsSettings_ed);

            names_ed = new string[factories.Count + 1];
            names_ed[0] = EmptySFX.NAME; 

            for (int i = 0, j = 1; i < factories.Count; i++, j++)
            {
                ASFXFactory factory = factories[i];
                names_ed[j] = factory.Name;
                if (factory.index_ed != j)
                {
                    factory.index_ed = j;
                    UnityEditor.EditorUtility.SetDirty(factory);
                    isDirty = true;
                }
            }

            if (isDirty || _factories == null || _factories.Length != factories.Count)
            {
                _factories = factories.ToArray();
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
                UpdateAsync_Ed();
        }

        private async void UpdateAsync_Ed()
        {
            await System.Threading.Tasks.Task.Delay(1);
            if (!Application.isPlaying)
                UpdateS_Ed();
        }

        private async static void SetStaticField_Ed()
        {
            await System.Threading.Tasks.Task.Delay(2);
            if (!Application.isPlaying)
            {
                s_self_ed = EUtility.FindAnyScriptable<SFXFactoriesStorage>();
                while (s_self_ed == null)
                    s_self_ed = EUtility.CreateScriptable<SFXFactoriesStorage>("FactoriesStorage", "Assets/Colonization/HitSFX");

                UpdateS_Ed();
            }
        }
#endif
    }
}
