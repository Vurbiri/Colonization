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
        private const string NAME = "Toggle", RESOURCE = "VToggle";
        private const string MENU = VUI_CONST_EDITOR.NAME_CREATE_MENU + NAME;

        private const float MIN_DURATION = 0f, MAX_DURATION = 1f;

        private static readonly string isOnName = "Is On";
        private static readonly string isFadeLabels = "Checkmark Fade Duration";
        private static readonly string[] switchingLabels = { "On|Off checkmark", "Switch checkmarks", "Color checkmark" };
        private static readonly string[] checkmarkOnNames = { "Checkmark", "Checkmark On", "Checkmark" };
        private static readonly string checkmarkOffName = "Checkmark Off";
        private static readonly string colorOnName = "Color On", colorOffName = "Color Off";
        private static readonly string groupName = "Group";

        private SerializedProperty _switchingTypeProperty;
        private SerializedProperty _onValueChangedProperty;

        private readonly AnimBool _showSwitchType = new(), _showColorType = new();
       
        private VToggle _toggle;
        bool _isOn;
        private SwitchingType _switchingType;
        private int _switchingTypeIndex;
        private float _duration;
        private Graphic _checkmarkOn, _checkmarkOff;
        private Color _colorOn, _colorOff;
        private VToggleGroup _group;

        protected override void OnEnable()
        {
            _toggle = target as VToggle;
            _switchingType = _toggle.Switching;
            _switchingTypeIndex = (int)_switchingType;

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
            _isOn = Toggle(isOnName, _toggle.IsOn);
            if (isChange |= EndChangeCheck()) 
                _toggle.IsOn = _isOn;
            //============================================================
            BeginChangeCheck();
            _duration = Slider(isFadeLabels, _toggle.CheckmarkFadeDuration, MIN_DURATION, MAX_DURATION);
            if (isChange |= EndChangeCheck()) 
                _toggle.CheckmarkFadeDuration = _duration;
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
            _checkmarkOn = VEditorGUILayout.ObjectField(checkmarkOnNames[_switchingTypeIndex], _toggle.CheckmarkOn);
            if (isChange |= EndChangeCheck()) 
                _toggle.CheckmarkOn = _checkmarkOn;
            //============================================================
            if (BeginFadeGroup(_showSwitchType.faded))
            {
                BeginChangeCheck();
                _checkmarkOff = VEditorGUILayout.ObjectField(checkmarkOffName, _toggle.CheckmarkOff);
                if (isChange |= EndChangeCheck()) 
                    _toggle.CheckmarkOff = _checkmarkOff;
            }
            EndFadeGroup();
            //============================================================
            if (BeginFadeGroup(_showColorType.faded))
            {
                BeginChangeCheck();
                _colorOn = ColorField(colorOnName, _toggle.ColorOn);
                _colorOff = ColorField(colorOffName, _toggle.ColorOff);
                if (isChange |= EndChangeCheck()) 
                    _toggle.SetColors(_colorOn, _colorOff);
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

        [MenuItem(MENU, false, VUI_CONST_EDITOR.MENU_PRIORITY)]
        public static void CreateFromMenu(MenuCommand command) => Utility.CreateFromResources(RESOURCE, NAME, command.context as GameObject);
    }
}
