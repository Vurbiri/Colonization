using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AActorSFX : MonoBehaviour, IUserSFX
    {
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _durationDeath = 1f;
        [ReadOnly, SerializeField] protected HitsSFXSettings _scriptablesSFX = new();

        protected Transform _thisTransform;
        protected HitsSFX _hitsSFX;
        protected AudioSource _audioSource;

        public abstract Transform StartTransform { get; }
        public AudioSource AudioSource => _audioSource;

        protected virtual void Awake()
		{
            _thisTransform = transform;
            _audioSource = GetComponent<AudioSource>();

            _hitsSFX = _scriptablesSFX.GetHitsSFX();
            _scriptablesSFX = null;
        }

        public virtual void Impact(AudioClip clip) => _audioSource.PlayOneShot(clip);

        public virtual void Block(bool isActive) { }

        public virtual IEnumerator Hit(int idSkill, int idHit, ActorSkin target) => _hitsSFX[idSkill, idHit].Hit(this, target);

        public virtual void Death() { }

        public virtual IEnumerator Death_Cn()
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

        #region Nested: HitsSFX
        //***********************************
        protected class HitsSFX
        {
            private readonly IHitSFX[] _instances;
            private readonly int[][] _hits;

            public IHitSFX this[int x, int y] => _instances[_hits[x][y]];

            public HitsSFX(int[] countHits, List<ScriptableSFX> scriptables)
            {
                int count = countHits.Length, countIDs;
                _hits = new int[count][];
                for (int i = 0; i < count; i++)
                    _hits[i] = new int[countHits[i]];

                count = scriptables.Count;
                _instances = new IHitSFX[count];

                ScriptableSFX scriptable; List<ID> ids; ID id;
                for (int i = 0; i < count; i++)
                {
                    scriptable = scriptables[i];

                    _instances[i] = scriptable.Instantiate();

                    ids = scriptable.IDs;
                    countIDs = ids.Count;
                    for (int j = 0; j < countIDs; j++)
                    {
                        id = ids[j];
                        _hits[id.skill][id.hit] = i;
                    }
                }
            }
        }
        #endregion
        #region Nested: HitsSFXSettings
        //***********************************
        [Serializable]
        protected class HitsSFXSettings
        {
            [SerializeField] private List<ScriptableSFX> _scriptables;
            [SerializeField] private int[] _countHits;

            public HitsSFX GetHitsSFX() => new(_countHits, _scriptables);

#if UNITY_EDITOR
            public void SetCountSkills(int count)
            {
                _countHits = new int[count];
                _scriptables = new();
            }

            public void SetCountHits(int idSkill, int count)
            {
                _countHits[idSkill] = count;
            }

            public void Add(int idSkill, int idHit, ScriptableSFX scriptables)
            {
                if (_scriptables == null)
                    return;

                int index = _scriptables.IndexOf(scriptables);
                if(index == -1)
                    _scriptables.Add(scriptables);
                else
                    scriptables = _scriptables[index];

                scriptables.IDAdd(new(idSkill, idHit));
            }
#endif
        }
        #endregion
        #region Nested: ScriptableSFX
        //***********************************
        [Serializable]
        protected class ScriptableSFX : IEquatable<ScriptableSFX>
        {
            [SerializeField] private APrefabSFXFactory _sfx;
            [SerializeField] private List<ID> _ids = new();

            public List<ID> IDs => _ids;

            public ScriptableSFX(APrefabSFXFactory sfx)
            {
                _sfx = sfx;
            }

            public void IDAdd(ID id) => _ids.Add(id);

            public IHitSFX Instantiate()
            {
                if (_sfx == null)
                    return new EmptySFX();

                return _sfx.Create();
            }

            public bool Equals(ScriptableSFX other)
            {
                if(other is null) return false;

                return _sfx == other._sfx;
            }
            public override bool Equals(object obj) => Equals(obj as ScriptableSFX);
            public override int GetHashCode()=> _sfx.GetHashCode();

            public static bool operator ==(ScriptableSFX a, ScriptableSFX b) => (a is null & b is null) || (a is not null && a.Equals(b));
            public static bool operator !=(ScriptableSFX a, ScriptableSFX b) => !(a is null & b is null) && !(a is not null && a.Equals(b));
        }
        #endregion
        #region Nested: ID
        //***********************************
        [Serializable]
        protected class ID
        {
            public int skill;
            public int hit;

            public ID(int skillID, int hitID)
            {
                skill = skillID;
                hit = hitID;
            }
        }
        #endregion

#if UNITY_EDITOR

        public void SetCountSkillsSFX_EditorOnly(int count) => _scriptablesSFX.SetCountSkills(count);
        public void SetCountHitsSFX_EditorOnly(int idSkill, int count) => _scriptablesSFX.SetCountHits(idSkill, count);

        public void SetSkillSFX_EditorOnly(int idSkill, int idHit, APrefabSFXFactory sfx) => _scriptablesSFX.Add(idSkill, idHit, new(sfx));
#endif
    }
}
