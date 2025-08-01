using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.Actors
{
    public class BarLookAtCamera : MonoBehaviour
	{
        private Transform _thisTransform, _actorTransform;
        private Vector3 _cameraForward;
        private bool _isActive;
        private SpriteRenderer _renderer;

        public void Init(Transform actorTransform, SpriteRenderer renderer)
		{
            _thisTransform = transform;
            _isActive = gameObject.activeSelf;
            _actorTransform = actorTransform;
            _renderer = renderer;

            GameContainer.CameraTransform.Subscribe(OnUpdate);
        }

        private void OnUpdate(Transform transform)
        {
            bool isActive = transform.position.y < CameraController.heightShow;
            if(_isActive != isActive)
                gameObject.SetActive(_isActive = isActive);

            _cameraForward = transform.forward;
        }

        private void Update()
        {
            if (_actorTransform.hasChanged && _renderer.isVisible)
                _thisTransform.rotation = Quaternion.LookRotation(_cameraForward, Vector3.up);
        }

        private void OnDestroy()
        {
            GameContainer.CameraTransform.Unsubscribe(OnUpdate);
        }
    }
}
