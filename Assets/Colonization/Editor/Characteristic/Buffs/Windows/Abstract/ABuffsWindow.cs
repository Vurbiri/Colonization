using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Vurbiri;
using Vurbiri.Collections;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public abstract class ABuffsWindow : EditorWindow
    {
        private const int MIN_VALUE = 1, MAX_VALUE = 5;
        private const int MIN_LEVEL = 10, MAX_LEVEL = 100;

        protected static readonly Vector2 s_minSize = new(450f, 450f);

        [SerializeField] private BuffsScriptable _scriptable;

        private Vector2 _scrollPos;
        private bool _isSave, _isFixedMaxLevel;
        private int _maxLevel;
        private readonly IdArray<ActorAbilityId, BuffSettings> _settings = new(() => new());
        private readonly IdArray<ActorAbilityId, AnimBool> _showSettings = new(() => new());
        private readonly string[] _names = { "None", "Percent", "Add" };
        private readonly int[] _values = { -1, TypeModifierId.BasePercent, TypeModifierId.Addition };

        private string _advanceName;
        private int _advanceMax;

        protected readonly HashSet<int> _excludeAbility = new(new int[]
            { ActorAbilityId.CurrentHP, ActorAbilityId.CurrentAP, ActorAbilityId.IsMove });

        protected void Enable(string scriptableName, string advanceName, int advanceMax)
        {
            _advanceName = advanceName;
            _advanceMax = advanceMax;
            _isSave = false;

            if (_scriptable == null)
            {
                _scriptable = EUtility.FindAnyScriptable<BuffsScriptable>(scriptableName);
                if (_scriptable == null)
                    _scriptable = EUtility.CreateScriptable<BuffsScriptable>(scriptableName, "Assets/Colonization/Settings/Characteristics/Buffs/");
                else
                    Debug.LogWarning($"Set {scriptableName}");
            }

            _maxLevel = _scriptable.MaxLevel;

            var scriptableSettings = _scriptable.Settings;
            if (scriptableSettings == null || scriptableSettings.Count == 0)
                return;

            for (int i = 0; i < scriptableSettings.Count; ++i)
                _settings[scriptableSettings[i].targetAbility] = scriptableSettings[i];

            for (int i = 0; i < ActorAbilityId.Count; ++i)
            {
                _showSettings[i].value = _settings[i].typeModifier >= 0;
                _showSettings[i].valueChanged.AddListener(Repaint);
            }
        }

        private void OnGUI()
        {
            BeginWindows();
            _scrollPos = BeginScrollView(_scrollPos);
            {
                DrawSave();

                _isSave |= DrawLevel();

                for (int i = 0; i < ActorAbilityId.Count; ++i)
                    _isSave |= DrawSettings(i, _settings[i], _showSettings[i]);
            }
            EndScrollView();
            EndWindows();

            #region Local: DrawSave(), DrawSettings(..)
            //=================================
            void DrawSave()
            {
                Space(10);
                Rect rect = BeginVertical();
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    rect.width *= 0.5f; rect.x = rect.width - 10f;
                    _isSave = EditorGUI.ToggleLeft(rect, "Save", _isSave);
                }
                EndVertical();
                Space(rect.height + 8);
            }
            //=================================
            bool DrawLevel()
            {
                int oldLevel = _maxLevel;

                BeginVertical(GUI.skin.box);
                {
                    Space();
                    _maxLevel = DrawValue("Max Level", _maxLevel, MIN_LEVEL, MAX_LEVEL);
                    Space();
                }
                EndVertical();
                return oldLevel != _maxLevel;
            }
            //=================================
            bool DrawSettings(int id, BuffSettings settings, AnimBool showSetting)
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

                LabelField(ActorAbilityId.Names_Ed[id], STYLES.H2);
                Space();
                settings.typeModifier = IntPopup("Modifier", oldTypeModifier, _names, _values);
                showSetting.target = settings.typeModifier >= 0;
                bool isSave = oldTypeModifier != settings.typeModifier;

                if (BeginFadeGroup(showSetting.faded))
                    isSave |= DrawValues(settings);
                EndFadeGroup();

                Space();
                EndVertical();

                return isSave;
            }
            //=================================
            bool DrawValues(BuffSettings settings)
            {
                int oldValue = settings.value, oldAdvance = settings.advance;
                int shift = 0;
                if(settings.typeModifier == TypeModifierId.Addition && settings.targetAbility <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                    shift = ActorAbilityId.SHIFT_ABILITY;

                settings.value = DrawValue("Value", settings.value >> shift, MIN_VALUE, MAX_VALUE) << shift;
                settings.advance = DrawValue(_advanceName, settings.advance, MIN_VALUE, _advanceMax);

                return oldValue != settings.value | oldAdvance != settings.advance;
                
            }
            //=================================
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int DrawValue(string name, int value, int min, int max) => IntSlider(name, Math.Clamp(value, min, max), min, max);
            #endregion
        }

        protected void Disable(bool isSorting)
        {
            for (int i = 0; i < ActorAbilityId.Count; ++i)
                _showSettings[i].valueChanged.RemoveListener(Repaint);

            if (!_isSave) return;

            SerializedObject serializedObject = new(_scriptable);
            serializedObject.FindProperty("_maxLevel").intValue = _maxLevel;

            _scriptable.SetValues_Ed(_maxLevel, _settings);
            if (isSorting)
                _scriptable.Sort_Ed();

            new SerializedObject(_scriptable).Update();
            EditorUtility.SetDirty(_scriptable);
            AssetDatabase.SaveAssets();
        }
    }
}
