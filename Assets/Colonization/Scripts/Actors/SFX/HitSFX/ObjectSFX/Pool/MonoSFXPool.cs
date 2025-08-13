using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class MonoSFXPool : IHitSFX
    {
        private readonly AMonoPooledSFX _prefab;
        private readonly Stack<AMonoPooledSFX> _pool;

        public MonoSFXPool(AMonoPooledSFX prefab, int count)
        {
            _prefab = prefab;
            _pool = new(count);
            for(int i = 0; i < count; i++) 
                Create();
        }

        public IEnumerator Hit(ISFXUser user, ActorSkin target) => (_pool.Count > 0 ? _pool.Pop() : Create()).Hit(user, target);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AMonoPooledSFX Create() => Object.Instantiate(_prefab, GameContainer.SharedContainer).Init(_pool.Push);
    }
}
