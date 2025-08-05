using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public abstract class APerkHint : AHintElement
    {
        [SerializeField, ReadOnly] protected string _key;
        [SerializeField, ReadOnly] protected string _cost;

        private Unsubscription _unsubscriber;

        public virtual void Init(Perk perk)
        {
            base.Init(GameContainer.UI.CanvasHint, 0.48f);
            _unsubscriber = Localization.Instance.Subscribe(SetTextAndCost);
        }

        public void Learn()
        {
            _unsubscriber ^= Localization.Instance.Subscribe(SetText);
            _cost = null;
        }

        protected abstract void SetTextAndCost(Localization localization);
        protected abstract void SetText(Localization localization);

        public void Dispose()
        {
            _unsubscriber.Unsubscribe();
        }

#if UNITY_EDITOR
        public void Init_Editor(Perk perk)
        {
            UnityEditor.SerializedObject so = new(this);
            so.FindProperty("_key").stringValue = perk.keyDescription;
            so.FindProperty("_cost").stringValue = $"<color=red>{perk.Cost}</color><space=0.1em><sprite={CurrencyId.Blood}>";
            so.ApplyModifiedProperties();

            gameObject.name = $"{perk.Id:D2}_{perk.keyDescription}";
        }
#endif
    }
}
