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

        private static SFXFactoriesStorage s_self;
        private static WarriorsSettingsScriptable s_warriorsSettings;
        private static DemonsSettingsScriptable s_demonsSettings;
        private static List<AActorSFX> s_actorSFXs;

        static SFXFactoriesStorage() => SetStaticField();

        public static void SetName_Ed(int nameIndex, string newName)
        {
            var oldName = names_ed[nameIndex];
            if (oldName == newName)
                return;

            names_ed[nameIndex] = newName;

            s_warriorsSettings.UpdateName_Ed(oldName, newName);
            s_demonsSettings.UpdateName_Ed(oldName, newName);
            foreach (var stt in s_actorSFXs)
                stt.UpdateSFX_Ed(oldName, newName);

            UnityEditor.AssetDatabase.SaveAssets();
            Debug.LogWarning($"[FactorySFX] Имя <b>\"{oldName}\"</b> заменено на <b>\"{newName}\"</b>.");
        }

        public static void UpdateS_Ed() => s_self.Update_Ed();
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

        private static void SetNames_Ed(List<ASFXFactory> factories)
        {
            names_ed = new string[factories.Count + 1];
            names_ed[0] = EmptySFX.NAME; ASFXFactory factory;

            bool isDirty = false;
            for (int i = 0, j = 1; i < factories.Count; i++, j++)
            {
                factory = factories[i];
                names_ed[j] = factory.Name;
                if (factory.index_ed != j)
                {
                    factory.index_ed = j;
                    UnityEditor.EditorUtility.SetDirty(factory);
                    isDirty = true;
                }
            }

            if (isDirty) UnityEditor.AssetDatabase.SaveAssets();
        }

        private async static void SetStaticField()
        {
            await System.Threading.Tasks.Task.Delay(2);
            if (!Application.isPlaying)
            {
                s_self = EUtility.FindAnyScriptable<SFXFactoriesStorage>();
                while (s_self == null)
                    s_self = EUtility.CreateScriptable<SFXFactoriesStorage>("FactoriesStorage", "Assets/Colonization/HitSFX");

                SetNames_Ed(EUtility.FindScriptables<ASFXFactory>());

                s_warriorsSettings = EUtility.FindAnyScriptable<WarriorsSettingsScriptable>();
                s_demonsSettings = EUtility.FindAnyScriptable<DemonsSettingsScriptable>();

                s_actorSFXs = EUtility.FindComponentsPrefabs<AActorSFX>();
            }
        }
#endif
    }
}
