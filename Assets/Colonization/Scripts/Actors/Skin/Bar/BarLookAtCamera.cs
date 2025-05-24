using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.Actors
{
    public class BarLookAtCamera : MonoBehaviour
	{
        private Transform _cameraTransform, _thisTransform;
        private Vector3 _lastCameraPosition = -Vector3.up, _up = Vector3.up;
        private Quaternion _lastRotation;
        private IRendererVisible[] _renderers;
        private int _renderersCount;

        public void Init(params IRendererVisible[] renderers)
		{
            _cameraTransform = SceneContainer.Get<CameraController>().MainCamera.transform;
            _thisTransform = transform;

            _renderers = renderers;
            _renderersCount = renderers.Length;
        }

        private void Update()
        {
            bool isVisible = false;
            for(int i = 0; i < _renderersCount; i++)
            {
                if(isVisible = _renderers[i].IsVisible)
                    break;
            }

            if (!isVisible || (_lastCameraPosition == _cameraTransform.position && _lastRotation == transform.rotation))
                return;

            _lastCameraPosition = _cameraTransform.position;
            _thisTransform.rotation = _lastRotation = Quaternion.LookRotation(_cameraTransform.forward, _up);
        }
    }
}
