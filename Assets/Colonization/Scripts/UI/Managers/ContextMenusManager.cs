﻿using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class ContextMenusManager
    {
        [SerializeField] private RectTransform _canvasTransform;
        [SerializeField] private RectTransform _menusTransform;
        [Space]
        [SerializeField] private CrossroadInitMenu _initMenu;
        [SerializeField] private CrossroadMainMenu _crossroadMenu;
        [SerializeField] private CrossroadWarriorsMenu _recruitingMenu;
        [SerializeField] private CrossroadRoadsMenu _roadsMenu;
        [Space]
        [SerializeField] private WarriorsMenu _warriorsMenu;
        [Space]
        [SerializeField] private ButtonCancel _buttonCancel;

        private CameraTransform _cameraTransform;
        private bool _isPlayerTurn, _lookAtEnabled;
        private IMenu _currentOpenMenu;

        private GameEvents _game;
        private GameplayEventBus _eventBus;
        private Unsubscription _unsubscription;

        public void Init(ContextMenuSettings settings)
        {
            _cameraTransform = settings.cameraTransform;

            _buttonCancel  .Init(settings.hint).Add(OnActiveMenu);
            _crossroadMenu .Init(settings, _roadsMenu, _recruitingMenu).Add(OnActiveMenu);
            _recruitingMenu.Init(settings, _crossroadMenu).Add(OnActiveMenu);
            _roadsMenu     .Init(settings, _crossroadMenu).Add(OnActiveMenu);
            _warriorsMenu  .Init(settings).Add(OnActiveMenu);

            settings.cameraTransform.Subscribe(LookAtCamera);

            settings.game.Subscribe(GameModeId.EndTurn, OnEndTurn);
            settings.game.Subscribe(GameModeId.Play, OnGamePlay);

            settings.eventBus.EventUnselect.Add(OnClose);
            settings.eventBus.EventActorSelect.Add(OnSelectWarrior);

            if (settings.game.GameMode == GameModeId.Landing)
            {
                _game = settings.game;
                _game.Subscribe(GameModeId.Landing, OnInit);
                _game.Subscribe(GameModeId.EndLanding, OnEndInit);

                _eventBus = settings.eventBus;
                _initMenu.Init(settings).Add(OnActiveMenu);
                _unsubscription = _eventBus.EventCrossroadSelect.Add(OnInitCrossroad);
            }
            else
            {
                settings.eventBus.EventCrossroadSelect.Add(OnSelectCrossroad);
                _initMenu = null;
            }
        }

        private void OnSelectCrossroad(Crossroad crossroad)
        {
            if (_isPlayerTurn & crossroad.Interactable & _currentOpenMenu == null)
            {
                _cameraTransform.TransformToLocalPosition(_menusTransform, _canvasTransform, crossroad.Position);
                _crossroadMenu.Open(crossroad);
                _buttonCancel.Setup(crossroad);
            }
        }
        private void OnInitCrossroad(Crossroad crossroad)
        {
            if (_isPlayerTurn & crossroad.Interactable & _currentOpenMenu == null)
            {
                _cameraTransform.TransformToLocalPosition(_menusTransform, _canvasTransform, crossroad.Position);
                _initMenu.Open(crossroad);
                _buttonCancel.Setup(crossroad);
            }
        }

        private void OnSelectWarrior(Actor actor)
        {
            if (actor.Interactable & _currentOpenMenu == null)
            {
                _cameraTransform.TransformToLocalPosition(_menusTransform, _canvasTransform, actor.Position);
                _warriorsMenu.Open(actor);
                _buttonCancel.Setup(actor);
            }
        }

        private void OnClose(bool isEquals)
        {
            if (!isEquals & _currentOpenMenu != null)
                _currentOpenMenu.CloseInstant();
        }

        private void OnEndTurn(TurnQueue turnQueue, int dice)
        {
            _isPlayerTurn = false;
            OnClose(false);
        }
        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            _isPlayerTurn = turnQueue.IsPerson;
        }
        private void OnInit(TurnQueue turnQueue, int dice)
        {
            _isPlayerTurn = turnQueue.IsPerson;
        }

        private void OnEndInit(TurnQueue turnQueue, int dice)
        {
            _isPlayerTurn = false;

            _game.Unsubscribe(GameModeId.Landing, OnInit);
            _game.Unsubscribe(GameModeId.EndLanding, OnEndInit);
            _unsubscription.Unsubscribe();
            _eventBus.EventCrossroadSelect.Add(OnSelectCrossroad);

            _unsubscription = null; _game = null; _eventBus = null; _initMenu = null;
        }

        private void LookAtCamera(Transform cameraTransform)
        {
            if(_lookAtEnabled)
                _menusTransform.rotation = Quaternion.LookRotation(cameraTransform.forward);
        }

        private void OnActiveMenu(IMenu menu, bool value)
        {
            if (value)
            {
                _currentOpenMenu = menu;
                _lookAtEnabled = true;
            }
            else if (_currentOpenMenu == menu)
            {
                _currentOpenMenu = null;
                _lookAtEnabled = false;
            }
        }

#if UNITY_EDITOR
        [StartEditor]
        [SerializeField, Range(3f, 8f)] private float _buttonDistance = 5f;

        public void OnValidate()
        {
            if(Application.isPlaying) return;
            
            EUtility.SetObject(ref _canvasTransform, "CanvasWorld");

            _canvasTransform.SetChildren(ref _menusTransform, "ContextMenusWorld");

            _canvasTransform.SetChildren(ref _initMenu);
            _canvasTransform.SetChildren(ref _crossroadMenu);
            _canvasTransform.SetChildren(ref _recruitingMenu);
            _canvasTransform.SetChildren(ref _roadsMenu);
            _canvasTransform.SetChildren(ref _warriorsMenu);
            _canvasTransform.SetChildren(ref _buttonCancel);

            SetDistance();
        }

        private void SetDistance()
        {
            _initMenu.SetButtonPosition(_buttonDistance);
            _crossroadMenu.SetButtonPosition(_buttonDistance);
            _recruitingMenu.SetButtonPosition(_buttonDistance);
            _warriorsMenu.SetButtonPosition(_buttonDistance);

            _buttonCancel.transform.localScale = Vector3.zero;
        }
#endif
    }
}
