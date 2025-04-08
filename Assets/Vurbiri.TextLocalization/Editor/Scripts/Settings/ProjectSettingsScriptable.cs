//Assets\Vurbiri.TextLocalization\Editor\Scripts\Settings\ProjectSettingsScriptable.cs
using UnityEditor;

namespace Vurbiri.TextLocalization.Editor
{
    using static CONST_L;

    public class ProjectSettingsScriptable : AGetOrCreateScriptableObject<ProjectSettingsScriptable>
    {
        public static ProjectSettingsScriptable GetOrCreateSelf() => GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH);
        public static SerializedObject GetSerializedSelf() => new(GetOrCreateSelf(PROJECT_SETTINGS_NAME, PROJECT_SETTINGS_PATH));

    }
}
