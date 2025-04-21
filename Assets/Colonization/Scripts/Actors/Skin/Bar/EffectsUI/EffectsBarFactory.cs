//Assets\Colonization\Scripts\Actors\Skin\Bar\EffectsUI\EffectsBarFactory.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EffectsBarFactory : MonoBehaviour
	{
        public Vector3 startPosition = new(3.6f, 0.9f, 0f);
        [Space]
        public Vector3 offsetPosition = new(-0.7f, 0f, 0f);
        [Space]
        public int orderLevel = 0;

        public EffectsBar Create(Transform repository, Action<EffectsBar, bool> callback) => new(Instantiate(this, repository), callback);
    }
}
