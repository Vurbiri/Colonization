//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\Abstract\ASkillState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public abstract partial class ASkillState : AActionState
        {
            protected readonly int _idAnimation;
            protected readonly IReadOnlyList<AEffect> _effects;
            protected readonly int _countEffects;

            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public ASkillState(Actor parent, IReadOnlyList<AEffect> effects, Settings settings, int id) : base(parent, settings.cost, TypeIdKey.Get<ASkillState>(id))
            {
                _idAnimation = settings.idAnimation;
                _effects = effects;
                _countEffects = _effects.Count;
                
                _waitTargetSkillAnimation = new(settings.damageTime);
                _waitEndSkillAnimation = new(settings.remainingTime);
            }

            public override void Enter()
            {
                _coroutineAction = _actor.StartCoroutine(Actions_Coroutine());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _actor.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }

                
            }

            protected void ToExit()
            {
                _coroutineAction = null;
                _fsm.ToDefaultState();
            }

            protected abstract IEnumerator Actions_Coroutine();

            protected virtual IEnumerator ApplySkill_Coroutine()
            {
                _skin.Skill(_idAnimation);
                yield return _waitTargetSkillAnimation;

                for (int i = 0; i < _countEffects; i++)
                    _effects[i].Apply(_actor, _actor);

                Pay();

                yield return _waitEndSkillAnimation;
            }

            #region Nested: Settings
            //*******************************************************
            [System.Serializable]
            public class Settings
            {
                public float damageTime;
                public float remainingTime;
                public int idAnimation;
                public int cost;
            }
            #endregion
        }
    }
}
