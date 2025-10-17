using System.Collections;
using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Caster
        {
            private abstract class Cast
            {
                private readonly Caster _caster;
                private readonly int _type, _id, _weight;

                #region Parent Properties
                protected AIController Human { [Impl(256)] get => _caster._parent; }
                protected SpellBook SpellBook { [Impl(256)] get => _caster._parent._spellBook; }
                protected SpellBook.ASpell Spell { [Impl(256)] get => _caster._parent._spellBook[_type, _id]; }
                protected Currencies Resources { [Impl(256)] get => _caster._parent._resources; }
                protected int PlayerId { [Impl(256)] get => _caster._parent._id; }
                protected ReadOnlyAbilities<HumanAbilityId> Abilities { [Impl(256)] get => _caster._parent._abilities; }
                #endregion

                public int Type { [Impl(256)] get => _type; }
                public int Id { [Impl(256)] get => _id; }
                public int Weight { [Impl(256)] get => _weight; }

                [Impl(256)] protected Cast(Caster parent, int type, int id) : this(parent, type, id, s_weights[type][id]) { }
                [Impl(256)] protected Cast(Caster parent, int type, int id, int weight)
                {
                    _caster = parent;
                    _type = type; _id = id;
                    _weight = weight;
                }

                public abstract IEnumerator Casting_Cn();

                [Impl(256)] protected IEnumerator Casting_Cn(SpellParam spellParam)
                {
                    SpellBook.Cast(_type, _id, spellParam);

                    yield return SpellBook.WaitEndCasting;
                }
            }
        }
    }
}
