using UnityEditor;
using UnityEngine;

namespace Vurbiri.Localization
{
    using static CONST_L;

    public class ProjectSettingsScriptable : AGetOrCreateScriptableObject<ProjectSettingsScriptable>
    {
        [SerializeField] private SettingsScriptable _settings;

        public SettingsScriptable CurrentSettings { set => _settings = value; }

        public static SettingsScriptable GetCurrentSettings()
        {
            SettingsScriptable settings;
            using (var self = GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH))
            {
                settings = self._settings;
            }
            
            return settings;
        }

        public static ProjectSettingsScriptable GetOrCreateSelf() => GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH));
    }
}
