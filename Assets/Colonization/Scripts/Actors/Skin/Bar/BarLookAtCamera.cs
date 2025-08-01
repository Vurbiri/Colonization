using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.Actors
{
    public class BarLookAtCamera : MonoBehaviour
	{
        private Transform _thisTransform;
        private GameObject _thisGameObject;
        private bool _isActive = true;
        private SpriteRenderer _renderer;

        public void Init(SpriteRenderer renderer)
		{
            _thisTransform = transform;
            _thisGameObject = gameObject;
            _renderer = renderer;

            GameContainer.CameraTransform.Subscribe(OnUpdate);
            _thisTransform.rotation = Quaternion.LookRotation(GameContainer.CameraTransform.Transform.forward, Vector3.up);
        }

        private void OnUpdate(Transform transform)
        {
            bool isActive = transform.position.y < CameraController.heightShow;
            if(_isActive != isActive)
                _thisGameObject.SetActive(_isActive = isActive);

            if (isActive && _renderer.isVisible)
                _thisTransform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }

        private void OnDestroy()
        {
            GameContainer.CameraTransform.Unsubscribe(OnUpdate);
        }
    }
}
