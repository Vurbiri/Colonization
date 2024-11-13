namespace Vurbiri.Colonization.UI
{
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;
    using Vurbiri.Colonization.Actors;
    using Vurbiri.Localization;
    using Vurbiri.UI;
    using static CONST_UI;
    using static CONST_UI_LNG_KEYS;

    public class ButtonAttack : AHinting
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private Color32 _colorPlus = Color.green;
        [SerializeField] private Color32 _colorMinus = Color.red;

        private const int MAX_SIZE_HINT = 64;

        private GameObject _parentGO;
        private Language _language;
        private Actor _currentActor;
        private int _idAttack;

        private string _textDamage, _textAP;
        private string _hexColorPlus, _hexColorMinus;

        public virtual void Init(Color color, GameObject parent, Language language)
        {
            Init(OnClick, false);

            _button.targetGraphic.color = color;
            _parentGO = parent;
            _language = language;
            _hexColorPlus = string.Format(TAG_COLOR_FORMAT_LITE, _colorPlus.ToHex());
            _hexColorMinus = string.Format(TAG_COLOR_FORMAT_LITE, _colorMinus.ToHex());

            _language.Subscribe(SetTexts);
        }

        public void Setup(Actor actor, int idAttack, SkillUI settings, Vector3 localPosition)
        {
            bool isUse = actor.ActionPoint >= settings.cost;

            _thisTransform.localPosition = localPosition;

            _currentActor = actor;
            _idAttack = idAttack;

            _buttonIcon.sprite = settings.sprite;
            _button.Interactable = isUse;

            int count = settings.effects.Length;

            StringBuilder sb = new(MAX_SIZE_HINT + count * MAX_SIZE_HINT);
            sb.Append(_language.GetText(_file, settings.keyName));
            if (settings.effects != null && count > 0)
            {
                EffectSettingsUI effect;
                for (int i = 0; i < count; i++)
                {
                    effect = settings.effects[i];
                    if(string.IsNullOrEmpty(effect.keyDesc))
                        continue;

                    sb.Append(effect.isNegative ? _hexColorMinus : _hexColorPlus);
                    if(effect.duration > 0)
                        sb.Append(_language.GetTextFormat(_file, effect.keyDesc, effect.value, effect.duration));
                    else
                        sb.Append(_language.GetTextFormat(_file, effect.keyDesc, effect.value));
                }
                sb.Append(TAG_COLOR_OFF);
            }
            sb.AppendFormat(_textDamage, settings.percentDamage);
            sb.Append(isUse ? _hexColorPlus : _hexColorMinus);
            sb.AppendFormat(_textAP, settings.cost);

            _text = sb.ToString();
            _thisGO.SetActive(true);
        }

        public void Disable() => _thisGO.SetActive(false);

        private void SetTexts(Language localization)
        {
            _textDamage = localization.GetText(_file, KEY_DAMAGE);
            _textAP = localization.GetText(_file, KEY_AP);
        }

        private void OnClick()
        {
            _parentGO.SetActive(false);
            _currentActor.Skill(_idAttack);
        }

        private void OnDestroy()
        {
            _language.Unsubscribe(SetTexts);
        }
    }
}
