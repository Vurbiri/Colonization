using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Vurbiri.Localization
{
    using static CONST_L;

    public class ProjectSettingsScriptable : AGetOrCreateScriptableObject<ProjectSettingsScriptable>
    {
        [SerializeField] private SettingsScriptable _settings;

#if UNITY_EDITOR
        public SettingsScriptable CurrentSettings { set => _settings = value; }

        public static ProjectSettingsScriptable GetOrCreateSelf() => GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH));

        public static SettingsScriptable GetCurrentSettings()
        {
            using var self = GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH);
            return self._settings;
        }
#else
        public static SettingsScriptable GetCurrentSettings()
        {
            using var self = Resources.Load<ProjectSettingsScriptable>(PROJECT_SETTINGS_NAME);
            return self._settings;
        }
#endif

    }
}
