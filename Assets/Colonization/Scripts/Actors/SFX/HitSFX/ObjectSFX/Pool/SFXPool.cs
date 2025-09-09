using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class SFXPool : IHitSFX
    {
        private readonly AMonoCreatorSFX _prefab;
        private readonly Stack<APooledSFX> _pool;

        public SFXPool(AMonoCreatorSFX prefab, int count)
        {
            _prefab = prefab;
            _pool = new(count);
            for (int i = 0; i < count; i++) 
                Create();
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target) => (_pool.Count > 0 ? _pool.Pop() : Create()).Hit(user, target);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private APooledSFX Create() => Object.Instantiate(_prefab, GameContainer.SharedContainer).Create(_pool.Push);
    }
}
