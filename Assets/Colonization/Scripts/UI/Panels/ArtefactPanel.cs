using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class ArtefactPanel : AHintElement, IDisposable
    {
        private const string NAME = "{0,-13}";
        
        [SerializeField, ReadOnly] private Part[] _parts;
        [SerializeField, ReadOnly] private Level _level;

        private Unsubscriptions _unsubscribers = new();

        public void Init(Human player, CanvasHint hint)
        {
            base.Init(hint, 0.48f);

            _unsubscribers += Localization.Instance.Subscribe(SetLocalizationText);
            _unsubscribers += player.Artefact.Subscribe(SetHintValues);

            //_unsubscribers += player.GetAbility(HumanAbilityId.is)
            Debug.LogWarning("!!!  IsProfitAdv в HumanAbilityId а не в ActorAbilityId");
        }

        private void SetLocalizationText(Localization localization)
        {
            int count = _parts.Length;
            StringBuilder stringBuilder = new(20 * (count + 1));

            for (int i = _parts.Length - 1; i >= 0; i--)
                _parts[i].SetHintText(localization, stringBuilder);
            stringBuilder.AppendLine();
            _level.SetHintText(localization, stringBuilder);

            _text = stringBuilder.ToString();
        }

        private void SetHintValues(Artefact artefact)
        {
            int[] levels = artefact.Levels; int count = levels.Length;
            StringBuilder stringBuilder = new(20 * (count + 1));

            for (int i = 0; i < count; i++)
                _parts[i].SetHintValue(levels[i], stringBuilder);
            stringBuilder.AppendLine();
            _level.SetHintValue(artefact.Level, stringBuilder);
            
            _text = stringBuilder.ToString();
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }

        #region Nested structs Level, Part
        //*************************************************
        [Serializable]
        private struct Level
        {
            private const string VALUE = "{0}/";

            [SerializeField] private TextMeshProUGUI _levelTMP;
            [SerializeField] private string _hintKey;
            [SerializeField] private int _maxLevel;

            private string _hintText;
            private int _level;

            public void SetHintText(Localization localization, StringBuilder sb)
            {
                StringBuilder stringBuilder = new(20);
                stringBuilder.AppendFormat(NAME, localization.GetText(Files.Gameplay, _hintKey));
                stringBuilder.Append(VALUE);
                stringBuilder.Append(_maxLevel);

                _hintText = stringBuilder.ToString();

                sb.AppendFormat(_hintText, _level);
            }

            public void SetHintValue(int level, StringBuilder stringBuilder)
            {
                _level = level;
                _levelTMP.text = _level.ToString();

                stringBuilder.AppendFormat(_hintText, level);
            }

#if UNITY_EDITOR
            public void Init_Editor(int maxLevel, Component parent)
            {
                _maxLevel = maxLevel;
                _hintKey = "Level";
                if (_levelTMP == null)
                    _levelTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(parent, "LevelTMP");
            }
            public readonly Color TextColor_Editor { set => _levelTMP.color = value; }
#endif
        }
        //*************************************************
        [Serializable]
        private struct Part
        {
            private const string VALUE = "+{0}%";

            [SerializeField] private TextMeshProUGUI _valueTMP;
            [SerializeField] private string _hintKey;
            [SerializeField] private int _baseValue;

            private string _hintText;
            private int _value;

            public void SetHintText(Localization localization, StringBuilder sb)
            {
                StringBuilder stringBuilder = new(18);
                stringBuilder.AppendFormat(NAME, localization.GetText(Files.Actors, _hintKey));
                stringBuilder.Append(VALUE);

                _hintText = stringBuilder.ToString();

                sb.AppendFormat(_hintText, _value);
                sb.AppendLine();
            }

            public void SetHintValue(int level, StringBuilder stringBuilder)
            {
                _value = level * _baseValue;
                _valueTMP.text = _value.ToString();

                stringBuilder.AppendFormat(_hintText, _value);
                stringBuilder.AppendLine();
            }

#if UNITY_EDITOR
            public void Init_Editor(BuffSettings settings, Component parent)
            {
                _hintKey = ActorAbilityId.Names[settings.targetAbility];
                _baseValue = settings.value;
                if (settings.typeModifier == TypeModifierId.Addition && settings.targetAbility <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                    _baseValue >>= ActorAbilityId.SHIFT_ABILITY;

                if (_valueTMP == null)
                    _valueTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(parent, _hintKey.Concat("TMP"));
            }
            public readonly Color TextColor_Editor { set => _valueTMP.color = value; }
#endif
        }
        #endregion

#if UNITY_EDITOR

        public RectTransform UpdateVisuals_Editor(float side, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.PanelBack;

            _level.TextColor_Editor = colors.TextDark;

            for (int i = 0; i < _parts.Length; i++)
                _parts[i].TextColor_Editor = colors.PanelText;

            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = new(side, side);

            return thisRectTransform;
        }

        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField] private BuffsScriptable _settings;

        private void OnValidate()
        {
            EUtility.SetScriptable(ref _settings, "ArtefactSettings");
            List<BuffSettings> settings = _settings.Settings;

            _level.Init_Editor(_settings.MaxLevel, this);

            if (_parts == null || _parts.Length != settings.Count)
                _parts = new Part[settings.Count];

            for (int i = 0; i < settings.Count; i++)
                _parts[i].Init_Editor(settings[i], this);
        }
#endif

    }
}
