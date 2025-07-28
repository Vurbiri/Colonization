using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class MonoSFXPool : IHitSFX
    {
        private readonly AMonoSFX _prefab;
        private readonly Stack<IHitSFX> _pool;

        public MonoSFXPool(AMonoSFX prefab, int count = 1)
        {
            _prefab = prefab;
            _pool = new(count);
            for(int i = 0; i < count; i++)
                _pool.Push(Create());
        }

        public IEnumerator Hit(IUserSFX user, ActorSkin target) => (_pool.Count > 0 ? _pool.Pop() : Create()).Hit(user, target);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IHitSFX Create() => Object.Instantiate(_prefab, GameContainer.Repository).Init(OnDeactivate);

        private void OnDeactivate(AMonoSFX sfx)
        {
            sfx.SetParent(GameContainer.Repository);
            _pool.Push(sfx);
        }
    }
}
