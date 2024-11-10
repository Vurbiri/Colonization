using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.Localization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    using static CONST_UI;

    public class ButtonAttack : AHinting
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private Color32 _colorPlus = Color.green;
        [SerializeField] private Color32 _colorMinus = Color.red;

        private const string KEY_DAMAGE = "Damage", KEY_AP = "AP";
        private const int MAX_SIZE_HINT = 64;

        private GameObject _parentGO;
        private Language _language;
        private Actor _currentActor;
        private int _idAttack;

        private string _textDamage, _textAP;
        private string _hexColorPlus, _hexColorMinus, _hexColorDefault;

        public virtual void Init(Color color, GameObject parent, Language language)
        {
            Init(OnClick, false);

            _button.targetGraphic.color = color;
            _parentGO = parent;
            _language = language;
            _hexColorPlus = string.Format(TAG_COLOR_FORMAT_LITE, _colorPlus.ToHex());
            _hexColorMinus = string.Format(TAG_COLOR_FORMAT_LITE, _colorMinus.ToHex());
            _hexColorDefault = string.Format(TAG_COLOR_FORMAT_LITE, _hint.HintColor.ToHex());

            _language.Subscribe(SetTexts);
        }

        public void Setup(Actor actor, int idAttack, AttackSkillUI settings, Vector3 localPosition)
        {
            bool isUse = actor.ActionPoint >= settings.cost;

            _thisTransform.localPosition = localPosition;

            _currentActor = actor;
            _idAttack = idAttack;

            _buttonIcon.sprite = settings.sprite;
            _button.Interactable = isUse;

            int count = settings.effects.Length;

            StringBuilder sb = new(MAX_SIZE_HINT + count * MAX_SIZE_HINT);
            if(settings.effects != null && count > 0)
            {
                EffectSettingsUI effect;
                for (int i = 0; i < count; i++)
                {
                    effect = settings.effects[i];
                    if(string.IsNullOrEmpty(effect.keyDesc))
                        continue;

                    sb.Append(effect.isNegative ? _hexColorMinus : _hexColorPlus);
                    sb.Append(_language.GetTextFormat(_file, effect.keyDesc, effect.value, effect.duration));
                    sb.Append(NEW_LINE);
                }
                sb.Append(_hexColorDefault);
            }
            sb.AppendFormat(_textDamage, settings.percentDamage);
            sb.Append(NEW_LINE);
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
            _currentActor.Attack(_idAttack);
        }

        private void OnDestroy()
        {
            _language.Unsubscribe(SetTexts);
        }
    }
}
