//Assets\Colonization\Scripts\UI\_UIGame\Button\Abstract\AButtonBuildType.cs
using UnityEngine;
using Vurbiri.TextLocalization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public abstract class AButtonBuildType<T> : AButtonBuild, IValueId<T> where T : IdType<T>
    {
        [Space]
        [SerializeField] protected Files _lngFile = Files.Gameplay;
        [Space]
        [SerializeField] protected string _key;
        [Space]
        [SerializeField] protected Id<T> _id;

        protected ACurrencies _cost;
        protected ACurrencies _cash;
        protected Unsubscriber _unsubscriber;
        protected string _caption;
        protected Player _player;
        protected GameObject _parentGO;
        protected Crossroad _currentCrossroad;

        public Id<T> Id => _id;

        public virtual void Init(ButtonSettings settings, ACurrencies cost, GameObject parent, Vector3 localPosition = default)
        {
            base.Init(localPosition, settings, OnClick);
            
            _player = settings.player;
            _cost = cost;
            _cash = _player.Resources;
            _parentGO = parent;
            _unsubscriber = SceneServices.Get<Localization>().Subscribe(SetText);
        }

        public virtual void Setup(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            SetTextHint(_caption, _cash, _cost);
        }

        protected void SetText(Localization localization) => _caption = localization.GetText(_lngFile, _key);

        protected abstract void OnClick();

        protected virtual void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
