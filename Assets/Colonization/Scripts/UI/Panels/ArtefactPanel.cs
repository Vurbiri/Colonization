using System;
using System.Collections;
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

        [SerializeField, Range(1f, 10f)] private float _showSpeed = 5f;
        [SerializeField, Range(0.5f, 3f)] private float _showDuration = 1.75f;
        [SerializeField, Range(0.5f, 2.5f)] private float _hideSpeed = 0.75f;
        [Space]
        [SerializeField, ReadOnly] private Ability[] _abilities;
        [SerializeField, ReadOnly] private Level _level;

        private Unsubscriptions _unsubscribers = new();
        private readonly Stack<WaitRealtime> _timers = new(4);

        // TEST
        Artefact _artefact;
        public void Test() => _artefact.Next(UnityEngine.Random.Range(2, 10));
        // TEST

        public void Init(Human player, CanvasHint hint)
        {
            base.Init(hint, 0.48f);

            _level.Init(player.Artefact.MaxLevel);
            for (int i = _abilities.Length - 1; i >= 0; i--)
            {
                _abilities[i].Init(this);
                _timers.Push(new(_showDuration));
            }

            _unsubscribers += Localization.Instance.Subscribe(SetLocalizationText);
            _unsubscribers += player.Artefact.Subscribe(SetHintValues);

            _unsubscribers += player.GetAbility(HumanAbilityId.IsArtefact).Subscribe(value => gameObject.SetActive(value > 0));
            
            // TEST
            Debug.Log("Удалить Тесты в ArtefactPanel");
            _artefact = player.Artefact;
            // TEST
        }

        private void SetLocalizationText(Localization localization)
        {
            int count = _abilities.Length;
            StringBuilder stringBuilder = new(20 * (count + 1));

            for (int i = _abilities.Length - 1; i >= 0; i--)
                _abilities[i].SetHintText(localization, stringBuilder);
            stringBuilder.AppendLine();
            _level.SetHintText(localization, stringBuilder);

            _text = stringBuilder.ToString();
        }

        private void SetHintValues(Artefact artefact)
        {
            int[] levels = artefact.Levels; int count = levels.Length;
            StringBuilder stringBuilder = new(20 * (count + 1));

            for (int i = 0; i < count; i++)
                _abilities[i].SetHintValue(levels[i], stringBuilder);
            stringBuilder.AppendLine();
            _level.SetHintValue(artefact.Level, stringBuilder);
            
            _text = stringBuilder.ToString();
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }

        private void ShowProfit(TextMeshProUGUI tmp, int profit)
        {
            if(profit != 0)
            {
                tmp.text = profit.ToString();
                StartCoroutine(ShowProfit_Cn(tmp.canvasRenderer, _timers.Pop()));
            }
        }
        private IEnumerator ShowProfit_Cn(CanvasRenderer renderer, WaitRealtime waitRealtime)
        {
            float alpha = 0f;
            while (alpha < 1f)
            {
                alpha += Time.unscaledDeltaTime * _showSpeed;
                renderer.SetAlpha(alpha);
                yield return null;
            }
            renderer.SetAlpha(alpha = 1f);

            yield return waitRealtime.Restart();
            _timers.Push(waitRealtime);

            while (alpha > 0f)
            {
                alpha -= Time.unscaledDeltaTime * _hideSpeed;
                renderer.SetAlpha(alpha);
                yield return null;
            }
            renderer.SetAlpha(0f);
        }

        #region Nested structs Level, Part
        //*************************************************
        [Serializable]
        private struct Level
        {
            private const string VALUE = "{0}/";

            [SerializeField] private TextMeshProUGUI _levelTMP;
            [SerializeField] private string _hintKey;

            private string _hintText;
            private int _maxLevel;
            private int _level;

            public void Init(int maxLevel) => _maxLevel = maxLevel;

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
            public void Init_Editor(Component parent, ProjectColors colors)
            {
                _hintKey = "Level";
                if (_levelTMP == null)
                    _levelTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(parent, "LevelTMP");
                _levelTMP.color = colors.TextDark;
            }
#endif
        }
        //*************************************************
        [Serializable]
        private struct Ability
        {
            private const string VALUE = "+{0,2}%";

            [SerializeField] private TextMeshProUGUI _valueTMP;
            [SerializeField] private TextMeshProUGUI _valueDeltaTMP;
            [SerializeField] private string _hintKey;
            [SerializeField] private int _baseValue;

            private ArtefactPanel _parent;
            private string _hintText;
            private int _value;

            public void Init(ArtefactPanel parent)
            {
                _parent = parent;
                _valueDeltaTMP.canvasRenderer.SetAlpha(0.0f);
            }

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
                int newValue = level * _baseValue;
                _valueTMP.text = newValue.ToString();

                stringBuilder.AppendFormat(_hintText, newValue);
                stringBuilder.AppendLine();

                _parent.ShowProfit(_valueDeltaTMP, newValue - _value);
                _value = newValue;
            }

#if UNITY_EDITOR
            public void Init_Editor(BuffSettings settings, Component parent, ProjectColors colors)
            {
                _hintKey = ActorAbilityId.Names[settings.targetAbility];
                _baseValue = settings.value;
                if (settings.typeModifier == TypeModifierId.Addition && settings.targetAbility <= ActorAbilityId.MAX_ID_SHIFT_ABILITY)
                    _baseValue >>= ActorAbilityId.SHIFT_ABILITY;

                string name = _hintKey.Concat("TMP");
                if (_valueTMP == null || _valueTMP.gameObject.name != name)
                    _valueTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(parent, name);
                name = _hintKey.Concat("DeltaTMP");
                if (_valueDeltaTMP == null || _valueDeltaTMP.gameObject.name != name)
                    _valueDeltaTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(parent, name);

                _valueTMP.color = colors.PanelText;
                _valueDeltaTMP.color = colors.TextPositive;
            }
#endif
        }
        #endregion

#if UNITY_EDITOR

        public RectTransform UpdateVisuals_Editor(float side)
        {
            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = new(side, side);

            return thisRectTransform;
        }

        [Header("┌──────────── Editor ─────────────────────")]
        [SerializeField] private BuffsScriptable _settings;
        [SerializeField] private ColorSettingsScriptable _colorSettings;

        private void OnValidate()
        {
            EUtility.SetScriptable(ref _settings, "ArtefactSettings");
            EUtility.SetScriptable(ref _colorSettings);

            List<BuffSettings> settings = _settings.Settings;
            var colors = _colorSettings.Colors;

            GetComponent<Image>().color = colors.PanelBack;

            _level.Init_Editor(this, colors);

            if (_abilities == null || _abilities.Length != settings.Count)
                _abilities = new Ability[settings.Count];

            for (int i = 0; i < settings.Count; i++)
                _abilities[i].Init_Editor(settings[i], this, colors);
        }
#endif
    }
}
