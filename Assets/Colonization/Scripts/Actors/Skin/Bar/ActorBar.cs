using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Actors.UI
{
	public class ActorBar : MonoBehaviour
    {
        private static short s_orderLevel = short.MinValue;
        private static readonly short s_incOrderLevel = 7;

        [SerializeField] private Settings _settings;

        private readonly List<IDisposable> _bars = new(7);

        private int _selfOrderLevel;
        private Actor _actor;
        private Transform _thisTransform, _actorTransform;
        private GameObject _thisGameObject;
        private Vector3 _cameraForward;
        private bool _isActive;
        private SpriteRenderer _renderer;
        private Unsubscription _unsubscriber;

        private void Awake()
        {
            _actor = GetComponentInParent<Actor>();
            _actorTransform = _actor.transform;
            _thisTransform = transform;
            _thisGameObject = gameObject;

            _settings.Init(this, _actor);

            _actor.Skin.EventStart += OnStart;

            _settings = null;
            _thisGameObject.SetActive(_isActive = false);
        }

        private void Update()
        {
            if (_actorTransform.hasChanged && _renderer.isVisible)
                _thisTransform.rotation = Quaternion.LookRotation(_cameraForward, Vector3.up);
        }

        private void OnChangeCamera(Transform transform)
        {
            bool isActive = transform.position.y < CameraController.heightShow;
            if (_isActive != isActive)
                _thisGameObject.SetActive(_isActive = isActive);

            _cameraForward = transform.forward;
        }

        private void OnAddEffect(ReactiveEffect effect, TypeEvent type)
        {
            if (type == TypeEvent.Add | type == TypeEvent.Subscribe)
                GameContainer.UI.EffectsBar.Get(_thisTransform).Init(effect, _actor, _selfOrderLevel);
        }
        private void OnRemoveActor(Actor actor, TypeEvent type)
        {
            if (type == TypeEvent.Remove)
                Destroy(_thisGameObject);
        }

        private void OnDestroy()
        {
            _unsubscriber?.Dispose();

            for (int i = _bars.Count - 1; i >= 0; i--)
                _bars[i].Dispose();
            _bars.Clear();
        }

        private void Add(IDisposable item) => _bars.Add(item);

        private void OnStart()
        {
            //_isActive = GameContainer.CameraTransform.CameraPosition.y < CameraController.heightShow;
            //_thisGameObject.SetActive(_isActive);
            _actor.Skin.EventStart -= OnStart;

            _unsubscriber += GameContainer.CameraTransform.Subscribe(OnChangeCamera);
            _unsubscriber += _actor.Subscribe(OnRemoveActor, false);
            _unsubscriber += _actor.Effects.Subscribe(OnAddEffect);
        }

        #region Nested: Settings
        //***********************************
        [Serializable] private class Settings
        {
            [SerializeField] private float _offset = 1.75f;
            [Space]
            [SerializeField] private HPBarFactory _hpBar;
            [SerializeField] private APBarFactory _apBar;
            [SerializeField] private SpriteRenderer _moveSprite;
            [SerializeField] private IdSet<ActorAbilityId, ValueBarFactory> _valueBars;
            [Space]
            [SerializeField] private PopupWidget3D _popup;

            public void Init(ActorBar bar, Actor actor)
            {
                var abilities = actor.Abilities;
                var abilitiesColors = GameContainer.UI.Colors.Ability;

                bar._renderer = _hpBar.Renderer;
                bar._thisTransform.localPosition = new(0f, actor.Skin.Size.y + _offset, 0f);

                if (s_orderLevel >= short.MaxValue - (s_incOrderLevel << 1))
                    s_orderLevel = short.MinValue;
                bar._selfOrderLevel = s_orderLevel += s_incOrderLevel;

                _popup.Init(s_orderLevel);

                bar.Add(_hpBar.Get(abilities, abilitiesColors, _popup, s_orderLevel));
                bar.Add(_apBar.Get(abilities, abilitiesColors, s_orderLevel));
                bar.Add(new MoveBar(_moveSprite, abilities, abilitiesColors, s_orderLevel));

                foreach (var value in _valueBars)
                    bar.Add(value.Get(abilities, abilitiesColors, _popup, s_orderLevel));
            }

#if UNITY_EDITOR
            public void OnValidate(MonoBehaviour parent)
            {
                parent.SetChildren(ref _hpBar);
                parent.SetChildren(ref _apBar);
                parent.SetChildren(ref _popup);
                parent.SetChildren(ref _moveSprite, "Move");

                _valueBars ??= new();
                if (_valueBars.Fullness < 2)
                    _valueBars.ReplaceRange(parent.GetComponentsInChildren<ValueBarFactory>());
            }
#endif
        }
        //***********************************
        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
                _settings.OnValidate(this);
        }
#endif
    }
}
