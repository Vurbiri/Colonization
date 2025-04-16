//Assets\Colonization\Scripts\UI\_UIGame\Button\Abstract\AButtonBuildType.cs
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    public abstract class AButtonBuildType<T> : AButtonBuild, IValueId<T> where T : IdType<T>
    {
        [Space]
        [SerializeField] protected Files _lngFile = Files.Gameplay;
        [SerializeField] protected string _key;
        [Space]
        [SerializeField] protected Id<T> _id;

        protected ACurrencies _cost;
        protected ACurrencies _cash;
        protected Unsubscriber _unsubscriber;
        protected string _caption;
        protected Human _player;
        protected AWorldMenu _parent;
        protected Crossroad _currentCrossroad;

        public Id<T> Id => _id;

        public virtual void Init(ButtonSettings settings, ACurrencies cost, AWorldMenu parent, Vector3 localPosition = default)
        {
            base.Init(localPosition, settings, OnClick);
            
            _player = settings.player;
            _cost = cost;
            _cash = _player.Resources;
            _parent = parent;
            _unsubscriber = SceneContainer.Get<Localization>().Subscribe(SetText);
        }

        public virtual void Setup(Crossroad crossroad)
        {
            _currentCrossroad = crossroad;
            SetTextHint(_caption, _cash, _cost);
        }

        protected void SetText(Localization localization) => _caption = localization.GetText(_lngFile, _key);

        protected abstract void OnClick();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscriber?.Unsubscribe();
        }
    }
}
