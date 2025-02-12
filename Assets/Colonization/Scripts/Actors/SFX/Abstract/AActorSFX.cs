//Assets\Colonization\Scripts\Actors\SFX\Abstract\AActorSFX.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AActorSFX : MonoBehaviour, IActorSFX
    {
        [SerializeField] protected Transform _rightHand;
        [Space]
        [SerializeField] protected float _heightDeath = -3.5f;
        [SerializeField] protected float _durationDeath = 1f;
        [HideInInspector, SerializeField] protected HitsSFXSettings _scriptablesSFX = new();
        [HideInInspector, SerializeField] bool[] _isReactToSkills;

        protected Transform _thisTransform;
        protected HitsSFX _hitsSFX;

        public Transform Main => _thisTransform;
        public Transform RightHand => _rightHand;
        public AudioSource AudioSource { get; private set; }

        protected virtual void Awake()
		{
            _thisTransform = transform;
            AudioSource = GetComponent<AudioSource>();

            _hitsSFX = _scriptablesSFX.GetHitsSFX(this);
            _scriptablesSFX = null;
        }

        public virtual void Block(bool isActive) { }

        public bool IsTargetReact(int index) => _isReactToSkills[index];
        public virtual void Hit(int idSkill, int idHit, ActorSkin target)
        {
            var delay = _hitsSFX[idSkill, idHit].Hit(target);
            if (_isReactToSkills[idSkill])
                target.React(delay);
        }

        public virtual void Death() { }

        public virtual IEnumerator Death_Coroutine()
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

            public HitsSFX(IReadOnlyList<int> countHits, IReadOnlyList<ScriptableSFX> scriptables, IActorSFX parent)
            {
                int count = countHits.Count, countIDs;
                _hits = new int[count][];
                for (int i = 0; i < count; i++)
                    _hits[i] = new int[countHits[i]];

                count = scriptables.Count;
                _instances = new IHitSFX[count];

                ScriptableSFX scriptable; IReadOnlyList<ID> ids; ID id;
                for (int i = 0; i < count; i++)
                {
                    scriptable = scriptables[i];

                    _instances[i] = scriptable.Instantiate(parent);

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

            public HitsSFX GetHitsSFX(IActorSFX parent) => new(_countHits, _scriptables, parent);

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
            [SerializeField] private AHitScriptableSFX _sfx;
            [SerializeField] private List<ID> _ids = new();

            public IReadOnlyList<ID> IDs => _ids;

            public ScriptableSFX(AHitScriptableSFX sfx)
            {
                _sfx = sfx;
            }

            public void IDAdd(ID id) => _ids.Add(id);

            public IHitSFX Instantiate(IActorSFX parent)
            {
                if (_sfx == null)
                    return new HitEmptySFX();

                return _sfx.Create(parent);
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

        protected virtual void OnValidate()
        {
            if (_rightHand == null)
                _rightHand = GetComponentsInChildren<Transform>().Where(t => t.gameObject.name == "RightHand").First();
        }

        public void SetCountSkillsSFX(int count)
        {
            _scriptablesSFX.SetCountSkills(count);

            _isReactToSkills ??= new bool[count];
            if (_isReactToSkills.Length != count)
                Array.Resize(ref _isReactToSkills, count);
        }
        public void SetCountHitsSFX(int idSkill, int count) => _scriptablesSFX.SetCountHits(idSkill, count);

        public void SetReactToSkills(bool isReactToSkill, int id) => _isReactToSkills[id] = isReactToSkill;
        public void SetSkillSFX(int idSkill, int idHit, AHitScriptableSFX sfx) => _scriptablesSFX.Add(idSkill, idHit, new(sfx));
#endif
    }
}
