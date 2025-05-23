//Assets\Colonization\Editor\Characteristic\Buffs\Abstract\ABuffsWindow.cs
using System;
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
        private const int MIN_VALUE = 1, MAX_VALUE = 25;

        [SerializeField] private ABuffsScriptable<T> _scriptable;

        private Vector2 _scrollPos;
        private bool _isSave;
        private readonly IdArray<ActorAbilityId, T> _settings = new(() => new());
        private readonly IdArray<ActorAbilityId, AnimBool> _showSettings = new(() => new());
        protected readonly List<int> _values = new(new int[] { -1, 0});
        protected readonly List<string> _names = new( new string[]{ "None", "Percent"});
        protected readonly HashSet<int> _excludeAbility = new(new int[] { ActorAbilityId.CurrentHP, ActorAbilityId.CurrentAP, ActorAbilityId.IsMove });

        protected virtual void OnEnable()
        {
            _isSave = false;

            if (_scriptable == null)
                _scriptable = EUtility.FindAnyScriptable<ABuffsScriptable<T>>();

            var scriptableSettings = _scriptable.Settings;

            if (scriptableSettings == null || scriptableSettings.Count == 0)
                return;

            T settings;
            for (int i = 0; i < scriptableSettings.Count; i++)
            {
                settings = scriptableSettings[i];
                _settings[settings.targetAbility] = settings;
            }

            for (int i = 0; i < ActorAbilityId.Count; i++)
            {
                _showSettings[i].value = _settings[i].typeModifier >= 0;
                _showSettings[i].valueChanged.AddListener(Repaint);
            }
        }

        private void OnGUI()
        {
            string[] names = _names.ToArray();
            int[] values = _values.ToArray();

            BeginWindows();
            _scrollPos = BeginScrollView(_scrollPos);

            DrawSave();

            for (int i = 0; i < ActorAbilityId.Count; i++)
                _isSave |= DrawSettings(i, _settings[i], _showSettings[i]);

            EndScrollView();
            EndWindows();

            #region Local: DrawSave(), DrawSettings(..)
            //=================================
            void DrawSave()
            {
                Space(10);
                BeginHorizontal(GUI.skin.button);
                Space(15);
                _isSave = ToggleLeft("Save", _isSave);
                EndHorizontal();
                Space(8);
            }
            //=================================
            bool DrawSettings(int id, T settings, AnimBool showSetting)
            {
                settings.targetAbility = id;
                if (_excludeAbility.Contains(id))
                {
                    settings.typeModifier = -1;
                    return false;
                }

                int oldTypeModifier = settings.typeModifier;
                
                BeginVertical(GUI.skin.box);
                Space();

                LabelField(ActorAbilityId.Names[id], STYLES.H2);
                Space();
                settings.typeModifier = IntPopup("Modifier", oldTypeModifier, names, values);
                showSetting.target = settings.typeModifier >= 0;
                bool isSave = oldTypeModifier != settings.typeModifier;

                if (BeginFadeGroup(showSetting.faded))
                    isSave |= DrawValues(settings);
                EndFadeGroup();

                Space();
                EndVertical();

                return isSave;
            }
            #endregion
        }

        protected virtual bool DrawValues(T settings)
        {
            int oldValue = settings.value;

            settings.value = Math.Clamp(settings.value, MIN_VALUE, MAX_VALUE);
            settings.value = IntSlider("Value", settings.value, MIN_VALUE, MAX_VALUE);

            return oldValue != settings.value;
        }

        private void OnDisable()
        {
            for (int i = 0; i < ActorAbilityId.Count; i++)
                _showSettings[i].valueChanged.RemoveListener(Repaint);

            if (!_isSave) return;
            
            _scriptable.SetValues_EditorOnly(_settings);

            new SerializedObject(_scriptable).Update();
            EditorUtility.SetDirty(_scriptable);
            AssetDatabase.SaveAssets();
        }
    }
}
