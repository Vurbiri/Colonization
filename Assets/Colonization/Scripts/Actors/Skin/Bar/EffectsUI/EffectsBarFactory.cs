using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EffectsBarFactory : MonoBehaviour
	{
        [NonSerialized] private EffectsBar.Settings _settings;

        public EffectsBar Create(Transform repository, Action<EffectsBar, bool> callback) => new(Instantiate(this, repository), _settings, callback);

        public void CreateSettings(Vector2 startPosition, Vector2 offsetPosition, int maxIndex) => _settings = new(startPosition, offsetPosition, maxIndex);
    }
}
