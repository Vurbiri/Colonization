//Assets\Vurbiri.UI\Editor\Editors\VToggleEditor.cs
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;
using static Vurbiri.UI.VToggle;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VToggle)), CanEditMultipleObjects]
    sealed public class VToggleEditor : VSelectableEditor
    {
        private const float MIN_DURATION = 0f, MAX_DURATION = 1f;
        
        private static readonly string[] switchingLabels = { "On|Off checkmark", "Switch checkmarks", "Color checkmark" };

        private static readonly string isOnName = "Is On";
        private static readonly string isFadeLabels = "Checkmark Fade Duration";
        private static readonly string[] checkmarkOnNames = { "Checkmark", "Checkmark On", "Checkmark" };
        private static readonly string checkmarkOffName = "Checkmark Off";
        private static readonly string colorOnName = "Color On", colorOffName = "Color Off";
        private static readonly string groupName = "Group";

        private SerializedProperty _switchingTypeProperty;
        private SerializedProperty _onValueChangedProperty;

        private readonly AnimBool _showSwitchType = new(), _showColorType = new();
       
        private VToggle _toggle;
        private SwitchingType _switchingType;
        private int _switchingTypeIndex;
        private VToggleGroup _group;

        protected override void OnEnable()
        {
            _toggle = target as VToggle;
            _switchingType = _toggle.Switching;
            _switchingTypeIndex = (int)_switchingType;
            _group = _toggle.Group;

            _switchingTypeProperty = serializedObject.FindProperty("_switchingType");
            _onValueChangedProperty = serializedObject.FindProperty("_onValueChanged");

            _showSwitchType.value = _switchingType == SwitchingType.SwitchCheckmark;
            _showColorType.value = _switchingType == SwitchingType.ColorCheckmark;

            _showSwitchType.valueChanged.AddListener(Repaint);
            _showColorType.valueChanged.AddListener(Repaint);

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _showSwitchType.valueChanged.RemoveListener(Repaint);
            _showColorType.valueChanged.RemoveListener(Repaint);
            base.OnDisable();
        }

        protected override void CustomMiddlePropertiesGUI()
        {
            serializedObject.ApplyModifiedProperties();

            bool isChange = false;

            BeginChangeCheck();
            bool isOn = Toggle(isOnName, _toggle.IsOn);
            if (isChange |= EndChangeCheck()) 
                _toggle.IsOn = isOn;
            //============================================================
            BeginChangeCheck();
            float duration = Slider(isFadeLabels, _toggle.CheckmarkFadeDuration, MIN_DURATION, MAX_DURATION);
            if (isChange |= EndChangeCheck()) 
                _toggle.CheckmarkFadeDuration = duration;
            //============================================================
            BeginChangeCheck();
            _switchingTypeIndex = Popup(_switchingTypeProperty.displayName, _switchingTypeIndex, switchingLabels);
            if (isChange |= EndChangeCheck())
            {
                _toggle.Switching = _switchingType = (SwitchingType)_switchingTypeIndex;
                _showSwitchType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _switchingType == SwitchingType.SwitchCheckmark;
                _showColorType.target = !_switchingTypeProperty.hasMultipleDifferentValues && _switchingType == SwitchingType.ColorCheckmark;
            }
            //============================================================
            indentLevel++;
            BeginChangeCheck();
            Graphic checkmarkOn = VEditorGUILayout.ObjectField(checkmarkOnNames[_switchingTypeIndex], _toggle.CheckmarkOn);
            if (isChange |= EndChangeCheck()) 
                _toggle.CheckmarkOn = checkmarkOn;
            //============================================================
            if (BeginFadeGroup(_showSwitchType.faded))
            {
                BeginChangeCheck();
                Graphic checkmarkOff = VEditorGUILayout.ObjectField(checkmarkOffName, _toggle.CheckmarkOff);
                if (isChange |= EndChangeCheck()) 
                    _toggle.CheckmarkOff = checkmarkOff;
            }
            EndFadeGroup();
            //============================================================
            if (BeginFadeGroup(_showColorType.faded))
            {
                BeginChangeCheck();
                Color colorOn = ColorField(colorOnName, _toggle.ColorOn);
                Color colorOff = ColorField(colorOffName, _toggle.ColorOff);
                if (isChange |= EndChangeCheck()) 
                    _toggle.SetColors(colorOn, colorOff);
            }
            EndFadeGroup();
            Space();
            indentLevel--;
            //============================================================
            BeginChangeCheck();
            _group = VEditorGUILayout.ObjectField(groupName, _toggle.Group);
            if (isChange |= EndChangeCheck())
            {
                _toggle.Group = _group;
                if (!Application.isPlaying && _group != null && _group.IsActiveToggle)
                    _toggle.IsOn = false;
            }
            Space();

            if (isChange & !Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(_toggle.gameObject.scene);

            serializedObject.Update();
        }

        protected override void CustomEndPropertiesGUI()
        {
            Space();
            PropertyField(_onValueChangedProperty);
        }

        [MenuItem("GameObject/UI Vurbiri/Toggle", false, VUI_CONST.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromPrefab("VToggle", "Toggle", command.context as GameObject);
    }
}
