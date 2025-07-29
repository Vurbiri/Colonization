using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public class CreatorSFXPool : IHitSFX
    {
        private readonly AMonoCreatorSFX _prefab;
        private readonly Stack<APooledSFX> _pool;

        public CreatorSFXPool(AMonoCreatorSFX prefab, int count)
        {
            _prefab = prefab;
            _pool = new(count);
            for (int i = 0; i < count; i++) 
                Create();
        }

        public IEnumerator Hit(IUserSFX user, ActorSkin target) => (_pool.Count > 0 ? _pool.Pop() : Create()).Hit(user, target);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private APooledSFX Create() => Object.Instantiate(_prefab, GameContainer.Container).Create(_pool.Push);
    }
}
