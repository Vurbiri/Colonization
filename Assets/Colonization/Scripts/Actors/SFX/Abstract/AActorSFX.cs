//Assets\Colonization\Scripts\Actors\SFX\Abstract\AActorSFX.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AActorSFX : MonoBehaviour, IActorSFX
    {
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _durationDeath = 1f;
        [HideInInspector, SerializeField] protected SkillsSFXSettings _prefabsSFX = new();

        protected Transform _thisTransform;
        protected SkillsSFX _skills;

        public Transform Container => _thisTransform;
        public AudioSource AudioSource { get; private set; }

        protected virtual void Awake()
		{
            _thisTransform = transform;
            AudioSource = GetComponent<AudioSource>();

            _skills = _prefabsSFX.GetSkillsSFX(this);
            _prefabsSFX = null;
        }

        public virtual void Skill(int id, Transform target) 
        {
           _skills[id].Run(target);
        }
        public virtual void Hit(int id, int index) 
        {
            _skills[id].Hint(index);
        }

        public IEnumerator Death_Coroutine()
        {
            Vector3 position = _thisTransform.localPosition;
            float speed = _heightDeath / _durationDeath;
            while (position.y > _heightDeath)
            {
                yield return null;
                position.y += speed * Time.deltaTime;
                _thisTransform.localPosition = position;
            }
        }

        #region Nested: SkillsSFX
        //***********************************
        [Serializable]
        protected class SkillsSFX
        {
            private readonly ISkillSFX[] _instances;
            private readonly int[] _skills;

            public ISkillSFX this[int index] => _instances[_skills[index]];

            public SkillsSFX(int countSkills, IReadOnlyList<ScriptableSFX> prefabs, IActorSFX parent)
            {
                _skills = new int[countSkills];

                int countInstances = prefabs.Count, countIds;
                _instances = new ISkillSFX[countInstances];

                ScriptableSFX prefab; IReadOnlyList<int> ids;
                for (int i = 0; i < countInstances; i++)
                {
                    prefab = prefabs[i];
                    _instances[i] = prefab.Instantiate(parent);

                    ids = prefab.IDs;
                    countIds = prefab.IDs.Count;
                    for (int j = 0; j < countIds; j++)
                        _skills[ids[j]] = i;
                }
            }
        }
        #endregion
        #region Nested: SkillsSFXSettings
        //***********************************
        [Serializable]
        protected class SkillsSFXSettings
        {
            [SerializeField] private List<ScriptableSFX> _scriptables;
            [SerializeField] private int _count;

            public SkillsSFX GetSkillsSFX(IActorSFX parent) => new(_count, _scriptables, parent);

#if UNITY_EDITOR
            public void SetCount(int count)
            {
                _count = count;
                _scriptables = new(count);
            }

            public void AddPrefab(int id, ScriptableSFX scriptables)
            {
                if (_scriptables == null)
                    return;

                int index = _scriptables.IndexOf(scriptables);
                if(index == -1)
                    _scriptables.Add(scriptables);
                else
                    scriptables = _scriptables[index];

                scriptables.IdAdd(id);
            }
#endif
        }
        #endregion
        #region Nested: ScriptableSFX
        //***********************************
        [Serializable]
        protected class ScriptableSFX : IEquatable<ScriptableSFX>
        {
            [SerializeField] private AScriptableSFX _sfx;
            [SerializeField] private float _duration;
            [SerializeField] private List<int> _ids = new();

            public IReadOnlyList<int> IDs => _ids;

            public ScriptableSFX(AScriptableSFX sfx, float duration)
            {
                _sfx = sfx;
                _duration = sfx == null ? 0f : duration;
            }

            public void IdAdd(int id) { _ids.Add(id); }

            public ISkillSFX Instantiate(IActorSFX parent)
            {
                if (_sfx == null)
                    return new SkillEmptySFX();

                return _sfx.Create(parent, _duration);
            }

            public bool Equals(ScriptableSFX other)
            {
                if(other is null) return false;
                if(_sfx == null & other._sfx == null) return true;
                return _sfx == other._sfx & Mathf.Approximately(_duration, other._duration);
            }
            public override bool Equals(object obj) => Equals(obj as ScriptableSFX);
            public override int GetHashCode()=> HashCode.Combine(_sfx.GetHashCode(), _duration.GetHashCode());

            public static bool operator ==(ScriptableSFX a, ScriptableSFX b) => (a is null & b is null) || (a is not null && a.Equals(b));
            public static bool operator !=(ScriptableSFX a, ScriptableSFX b) => !(a is null & b is null) && !(a is not null && a.Equals(b));
        }
        #endregion

#if UNITY_EDITOR
        public void SetCountSkillsSFX(int count) => _prefabsSFX.SetCount(count);
        public void SetSkillSFX(AScriptableSFX sfx, float duration, int id) => _prefabsSFX.AddPrefab(id, new(sfx, duration));
#endif
    }
}
