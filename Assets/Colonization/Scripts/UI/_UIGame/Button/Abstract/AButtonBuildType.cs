//Assets\Colonization\Scripts\UI\_UIGame\Button\Abstract\AButtonBuildType.cs
using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public abstract class AButtonBuildType<T> : AButtonBuild, IValueId<T> where T : AIdType<T>
    {
        [Space]
        [SerializeField] protected Files _lngFile = Files.Gameplay;
        [Space]
        [SerializeField] protected string _key;
        [Space]
        [SerializeField] protected Id<T> _id;

        protected ACurrencies _cost;
        protected IUnsubscriber _unsubscriber;
        protected string _caption;
        protected Players _players;
        protected GameObject _parentGO;
        
        public Id<T> Id => _id;

        public virtual void Init(ButtonSettings settings, ACurrencies cost, GameObject parent, Vector3 localPosition = default)
        {
            base.Init(localPosition, settings, OnClick);
            
            _players = settings.players;
            _cost = cost;
            _parentGO = parent;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        protected void SetText(Language localization) => _caption = localization.GetText(_lngFile, _key);

        protected abstract void OnClick();

        protected virtual void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
