using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    public abstract class AButtonBuildType<T> : AButtonBuild, IValueId<T> where T : IdType<T>
    {
        [SerializeField] protected FileIdAndKey _getText;
        [SerializeField] protected Id<T> _id;

        protected ReadOnlyLiteCurrencies _cost;
        protected ReadOnlyCurrencies _cash;
        protected string _caption;
        protected AWorldMenu _parent;
        protected Crossroad _currentCrossroad;

        public Id<T> Id => _id;

        public virtual void Init(ReadOnlyLiteCurrencies cost, AWorldMenu parent)
        {
            base.InternalInit(OnClick, true);
            
            _cost = cost;
            _cash = GameContainer.Person.Resources;
            _parent = parent;
            
            Localization.Subscribe(SetText);
        }

        public virtual void Setup(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            SetTextHint(_caption, _cash, _cost);
        }

        protected void SetText(Localization localization) => _caption = localization.GetText(_getText.id, _getText.key);

        protected abstract void OnClick();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Unsubscribe(SetText);
        }
    }
}
