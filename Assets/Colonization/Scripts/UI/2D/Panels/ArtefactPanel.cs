using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class ArtefactPanel : AHintElement, IDisposable
    {
        private const string NAME = "{0,-13}";

        [SerializeField] private FileIdAndKey _getText;
        [Space]
        [SerializeField, Range(1f, 10f)] private float _showSpeed = 5f;
        [SerializeField, Range(0.5f, 3f)] private float _showDuration = 1.75f;
        [SerializeField, Range(0.5f, 2.5f)] private float _hideSpeed = 0.75f;
        [Space]
        [SerializeField, ReadOnly] private Ability[] _abilities;
        [SerializeField, ReadOnly] private Level _level;

        private string _name;
        private Unsubscriptions _unsubscribers = new();
        private readonly Stack<WaitRealtime> _timers = new(4);

        // TEST
        public void Test()
        {
            Debug.Log("Удалить Тесты в ArtefactPanel");

            //_artefact.Next(UnityEngine.Random.Range(2, 10));

            var person = GameContainer.Players.Person;

            person.SpawnTest(WarriorId.Wizard, HEX.Left);
            person.SpawnTest(WarriorId.Wizard, HEX.LeftDown);
            person.SpawnTest(WarriorId.Militia, HEX.LeftUp);

            person.SpawnTest(WarriorId.Militia, 1);
            //GameContainer.Players.GetAI(PlayerId.AI_01).SpawnTest(WarriorId.Militia, 2);
            //GameContainer.Players.GetAI(PlayerId.AI_02).SpawnTest(WarriorId.Wizard, 2);

        }
        // TEST

        public void Init()
        {
            base.Init(GameContainer.UI.CanvasHint, 0.48f);

            var person = GameContainer.Players.Person;
            _level.Init(person.Artefact.MaxLevel);
            for (int i = _abilities.Length - 1; i >= 0; i--)
            {
                _abilities[i].Init(this);
                _timers.Push(new(_showDuration));
            }


            _unsubscribers += Localization.Instance.Subscribe(SetLocalizationText);
            _unsubscribers += person.Artefact.Subscribe(SetHintValues);

            _unsubscribers += person.GetAbility(HumanAbilityId.IsArtefact).Subscribe(value => gameObject.SetActive(value > 0));
        }

        private void SetLocalizationText(Localization localization)
        {
            int count = _abilities.Length;
            StringBuilder stringBuilder = new(22 * (count + 1));

            stringBuilder.AppendLine(_name = localization.GetText(_getText));
            stringBuilder.AppendLine();

            for (int i = _abilities.Length - 1; i >= 0; i--)
                _abilities[i].SetHintText(localization, stringBuilder);
            stringBuilder.AppendLine();
            _level.SetHintText(localization, stringBuilder);

            _hintText = stringBuilder.ToString();
        }

        private void SetHintValues(Artefact artefact)
        {
            int[] levels = artefact.Levels; int count = levels.Length;
            StringBuilder stringBuilder = new(22 * (count + 1));

            stringBuilder.AppendLine(_name);
            stringBuilder.AppendLine();

            for (int i = 0; i < count; i++)
                _abilities[i].SetHintValue(levels[i], stringBuilder);
            stringBuilder.AppendLine();
            _level.SetHintValue(artefact.Level, stringBuilder);
            
            _hintText = stringBuilder.ToString();
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

        #region Nested structs Level, Ability
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
                stringBuilder.AppendFormat(NAME, localization.GetText(LangFiles.Gameplay, _hintKey));
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
                parent.SetChildren(ref _levelTMP, "LevelTMP");
                _levelTMP.color = colors.TextDark;
            }
#endif
        }
        //*************************************************
        [Serializable]
        private struct Ability
        {
            private const string VALUE = "{0,3:+#;;0}%";

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
                stringBuilder.AppendFormat(NAME, localization.GetText(LangFiles.Actors, _hintKey));
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
                _hintKey = ActorAbilityId.Names_Ed[settings.targetAbility];
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

        [StartEditor]
        [SerializeField] private BuffsScriptable _settings;

        public RectTransform UpdateVisuals_Editor(float side, ProjectColors colors)
        {
            List<BuffSettings> settings = _settings.Settings;

            GetComponent<Image>().color = colors.PanelBack;

            _level.Init_Editor(this, colors);

            EUtility.SetArray(ref _abilities, settings.Count);
            for (int i = 0; i < settings.Count; i++)
                _abilities[i].Init_Editor(settings[i], this, colors);

            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = new(side, side);

            return thisRectTransform;
        }

        private void OnValidate()
        {
            EUtility.SetScriptable(ref _settings, "ArtefactSettings");
        }
#endif
    }
}
