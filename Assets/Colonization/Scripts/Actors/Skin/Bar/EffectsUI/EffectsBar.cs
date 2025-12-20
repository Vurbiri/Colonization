using System;
using TMPro;
using UnityEngine;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization.UI
{
    sealed public class EffectsBar : APooledObject<EffectsBar>
    {
        private const char CHAR = '-';

        private readonly Settings _settings;
        private readonly SpriteRenderer _sprite;
        private readonly TextMeshPro _durationTMP;

        private int Duration { [Impl(256)] set => _durationTMP.text = new(CHAR, value); }
        private int Index { [Impl(256)] set => _transform.localPosition = _settings.GetPosition(value); }

        private Subscription _subscription;

        public EffectsBar(Component initObj, Settings settings, Action<EffectsBar, bool> callback) : base(initObj.gameObject, callback)
        {
            _settings = settings;

            _sprite = initObj.GetComponent<SpriteRenderer>();
            _durationTMP = initObj.GetComponentInChildren<TextMeshPro>();

            Object.Destroy(initObj);
        }

        public void Init(ReactiveEffect effect, Actor actor, int orderLevel)
        {
            _sprite.sortingOrder = orderLevel;
            _durationTMP.sortingOrder = orderLevel;

            _sprite.sprite = GameContainer.UI.SpritesOfAbilities[effect.TargetAbility];
            _sprite.color = GameContainer.UI.Colors.GetColor(effect.IsPositive);

            Index = effect.Index;
            Duration = effect.Duration;

            _subscription += actor.Subscribe(OnRemoveActor, false);
            _subscription += effect.Subscribe(OnChangeEffect, false);

            Enable();
        }

        [Impl(256)] private void Destroy()
        {
            _subscription?.Dispose();
            ToPool();
        }

        private void OnRemoveActor(Actor actor, TypeEvent type)
        {
            if (type == TypeEvent.Remove)
                Destroy();
        }
        private void OnChangeEffect(ReactiveEffect effect, TypeEvent operation)
        {
            switch (operation)
            {
                case TypeEvent.Reindex:
                    Index = effect.Index;
                    return;
                case TypeEvent.Change:
                    Duration = effect.Duration;
                    return;
                case TypeEvent.Remove:
                    Destroy();
                    return;
            }
        }

        #region Nested: Settings
        //***********************************
        public class Settings
        {
            private readonly Vector2 _start;
            private readonly Vector2 _offset;
            private readonly int _maxIndex;

            [Impl(256)]
            public Settings(Vector2 startPosition, Vector2 offsetPosition, int maxIndex)
            {
                _start = startPosition;
                _offset = offsetPosition;
                _maxIndex = maxIndex;
            }

            [Impl(256)]
            public Vector3 GetPosition(int index)
            {
                int x = index % _maxIndex, y = index / _maxIndex;
                return new(_start.x + _offset.x * x, _start.y + _offset.y * y, 0f);
            }
        }
        //***********************************
        #endregion
    }
}
