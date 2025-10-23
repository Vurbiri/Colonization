using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public abstract class APerkHint : AHintElement
    {
        [SerializeField, ReadOnly] protected string _key;
        [SerializeField, ReadOnly, Multiline] protected string _cost;

        private Subscription _subscription;

        public virtual void Init(Perk perk)
        {
            base.InternalInit(GameContainer.UI.CanvasHint, 0.48f);
            _subscription = Localization.Instance.Subscribe(SetTextAndCost);
        }

        public void Learn()
        {
            _subscription ^= Localization.Instance.Subscribe(SetText);
            _cost = null;
        }

        protected abstract void SetTextAndCost(Localization localization);
        protected abstract void SetText(Localization localization);

        public void Dispose()
        {
            _subscription.Dispose();
        }

#if UNITY_EDITOR
        public void Init_Editor(Perk perk)
        {
            UnityEditor.SerializedObject so = new(this);
            so.FindProperty("_key").stringValue = perk.keyDescription;
            so.FindProperty("_cost").stringValue = $"\n{CONST_UI.SEPARATOR}\n<color=red>{perk.Cost}</color><space=0.1em><sprite={CurrencyId.Blood}>";
            so.ApplyModifiedProperties();

            gameObject.name = $"{perk.Id:D2}_{perk.keyDescription}";
        }
#endif
    }
}
