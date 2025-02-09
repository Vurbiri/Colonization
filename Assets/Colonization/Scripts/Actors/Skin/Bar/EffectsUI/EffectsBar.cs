//Assets\Colonization\Scripts\Actors\Skin\Bar\EffectsUI\EffectsBar.cs
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Actors
{
    public class EffectsBar : APooledObject<EffectsBar>
    {
        private const char CHAR = '-';

        [SerializeField] private Vector3 _startPosition = new(3.6f, 0.9f, 0f);
        [Space]
        [SerializeField] private Vector3 _offsetPosition = new(-0.7f, 0f, 0f);
        [SerializeField] private int _orderLevel = 0;
        [Space]
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TextMeshPro _durationTMP;

        private int Index { set => _thisTransform.localPosition = _startPosition + _offsetPosition * value; }
        private int Duration { set => _durationTMP.text = new(CHAR, value); }

        private Unsubscribers _unsubscribers;

        public void Init(ReactiveEffect effect, IReactiveElement<Actor> actor, IReadOnlyList<Sprite> sprites, SettingsTextColor colors, int orderLevel)
        {
            _sprite.sortingOrder = _orderLevel + orderLevel;
            _durationTMP.sortingOrder = _orderLevel + orderLevel;

            _sprite.sprite = sprites[effect.TargetAbility];
            _sprite.color = effect.IsNegative ? colors.ColorNegative : colors.ColorPositive;

            Index = effect.Index;
            Duration = effect.Duration;

            _unsubscribers += actor.Subscribe((item, type) => { if (type == TypeEvent.Remove) Destroy(); }, false);
            _unsubscribers += effect.Subscribe(OnChangeEffect, false);

            SetActive(true);
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();

            if (_durationTMP == null)
                _durationTMP = GetComponentInChildren<TextMeshPro>();
        }
#endif
    }
}
