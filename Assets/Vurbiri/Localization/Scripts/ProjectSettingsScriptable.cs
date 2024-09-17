using UnityEditor;
using UnityEngine;

namespace Vurbiri.Localization
{
    using static CONST;

    public class ProjectSettingsScriptable : AGetOrCreateScriptableObject<ProjectSettingsScriptable>
    {
        [SerializeField] private SettingsScriptable _settings;

        public SettingsScriptable CurrentSettings { get => _settings; set => _settings = value; }

        public static ProjectSettingsScriptable GetOrCreateSelf() => GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH));
    }
}
