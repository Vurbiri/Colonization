//Assets\Colonization\Scripts\Actors\Skin\Bar\EffectsUI\EffectsBar.cs
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization.Actors
{
    sealed public class EffectsBar : APooledObject<EffectsBar>
    {
        private const char CHAR = '-';

        private readonly Vector3 _startPosition = new(3.6f, 0.9f, 0f);
        private readonly Vector3 _offsetPosition = new(-0.7f, 0f, 0f);
        private readonly SpriteRenderer _sprite;
        private readonly TextMeshPro _durationTMP;

        private int Index { set => _transform.localPosition = _startPosition + _offsetPosition * value; }
        private int Duration { set => _durationTMP.text = new(CHAR, value); }

        private Unsubscribers _unsubscribers;

        public EffectsBar(EffectsBarFactory initObj, Action<EffectsBar, bool> callback) : base(initObj.gameObject, callback)
        {
            _startPosition = initObj.startPosition;
            _offsetPosition = initObj.offsetPosition;
            _sprite = initObj.GetComponent<SpriteRenderer>();
            _durationTMP = initObj.GetComponentInChildren<TextMeshPro>();

            _sprite.sortingOrder = initObj.orderLevel;
            _durationTMP.sortingOrder = initObj.orderLevel;

            Object.Destroy(initObj);
        }

        public void Init(ReactiveEffect effect, IReactiveItem<Actor> actor, IReadOnlyList<Sprite> sprites, TextColorSettings colors, int orderLevel)
        {
            _sprite.sortingOrder += orderLevel;
            _durationTMP.sortingOrder += orderLevel;

            _sprite.sprite = sprites[effect.TargetAbility];
            _sprite.color = colors.GetColor(effect.IsPositive);

            Index = effect.Index;
            Duration = effect.Duration;

            _unsubscribers += actor.Subscribe((item, type) => { if (type == TypeEvent.Remove) Destroy(); }, false);
            _unsubscribers += effect.Subscribe(OnChangeEffect, false);

            Enable();
        }

        private void Destroy()
        {
            _unsubscribers?.Unsubscribe();
            ToPool();
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
    }
}
