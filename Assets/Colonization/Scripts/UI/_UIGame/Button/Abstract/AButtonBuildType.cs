using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public abstract class AButtonBuildType<T> : AButtonBuild, IValueId<T> where T : AIdType<T>
    {
        [Space]
        [SerializeField] protected string _key;
        [Space]
        [SerializeField] protected Id<T> _id;

        protected ACurrencies _cost;
        protected Unsubscriber<Language> _unsubscriber;
        protected string _caption;

        public Id<T> Id => _id;

        protected void SetText(Language localization) => _caption = localization.GetText(_file, _key);

        protected virtual void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

        
    }
}
