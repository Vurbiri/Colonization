//Assets\Colonization\Editor\Characteristic\Buffs\Abstract\ABuffsWindow.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Vurbiri;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public abstract class ABuffsWindow<T> : EditorWindow where T : BuffSettings, new()
    {
        [SerializeField] private ABuffsScriptable<T> _scriptable;

        private Vector2 _scrollPos;
        private GUIStyle _label;
        private readonly IdArray<ActorAbilityId, T> _settings = new(() => new());
        private List<T> _scriptableSettings;
        protected readonly List<int> _values = new(new int[] { -1, 0});
        protected readonly List<string> _names = new( new string[]{ "None", "Percent"});
        protected readonly HashSet<int> _excludeAbility = new(new int[] { ActorAbilityId.CurrentHP, ActorAbilityId.CurrentAP, ActorAbilityId.IsMove });

        protected virtual void OnEnable()
        {
            if (_scriptable == null)
                _scriptable = EUtility.FindAnyScriptable<ABuffsScriptable<T>>();

            _scriptableSettings = _scriptable.EditorOnlySettings ?? new();

            if (_scriptableSettings.Count == 0)
                return;

            T settings;
            for (int i = 0; i < _scriptableSettings.Count; i++)
            {
                settings = _scriptableSettings[i];
                _settings[settings.targetAbility] = settings;
            }

            _scriptableSettings.Clear();
        }

        private void OnGUI()
        {
            SetLabelStyle();

            BeginWindows();
            _scrollPos = BeginScrollView(_scrollPos);

            for (int i = 0; i < ActorAbilityId.Count; i++)
                DrawSettings(i, _settings[i]);

            EndScrollView();
            EndWindows();
        }

        private void DrawSettings(int id, T settings)
        {
            settings.targetAbility = id;
            if (_excludeAbility.Contains(id))
            {
                settings.typeModifier = -1;
                return;
            }

            BeginVertical(GUI.skin.window);

            LabelField(ActorAbilityId.Names[id], _label);
            Space();
            settings.typeModifier = IntPopup("Modifier", settings.typeModifier, _names.ToArray(), _values.ToArray());
            AnimBool animBool = new(settings.typeModifier >= 0);
            
            if (BeginFadeGroup(animBool.faded))
                DrawValues(settings);
            EndFadeGroup();

            Space(20);
            EndVertical();
        }

        protected virtual void DrawValues(T settings)
        {
            settings.value = IntSlider("Value", settings.value, 1, 25);
        }

        private void SetLabelStyle()
        {
            _label = new(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13
            };
            _label.normal.textColor = Color.cyan;
        }

        private void OnDisable()
        {
            for (int i = 0; i < ActorAbilityId.Count; i++)
            {
                if (_settings[i].typeModifier >= 0)
                    _scriptableSettings.Add(_settings[i]);
            }

            _scriptable.EditorOnlySettings = _scriptableSettings;
        }
    }
}
