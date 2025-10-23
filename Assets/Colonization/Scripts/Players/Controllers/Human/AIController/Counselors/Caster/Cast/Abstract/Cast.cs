using System.Collections;
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
                protected SpellParam _param;

                #region Parent Properties
                protected AIController Human { [Impl(256)] get => _caster._parent; }
                protected int HumanId { [Impl(256)] get => _param.playerId; }
                protected SpellBook SpellBook { [Impl(256)] get => _caster._parent._spellBook; }
                protected SpellBook.ASpell Spell { [Impl(256)] get => _caster._parent._spellBook[_type, _id]; }
                protected Currencies Resources { [Impl(256)] get => _caster._parent._resources; }
                protected PerkTree PerkTree { [Impl(256)] get => _caster.PerkTrees; }
                protected int Mana { [Impl(256)] get => _caster._parent._resources[CurrencyId.Mana]; }
                protected bool CanPay { [Impl(256)] get => _caster._parent._resources[CurrencyId.Mana] >= SpellBook.Cost[_type][_id]; }
                protected ReadOnlyAbilities<HumanAbilityId> Abilities { [Impl(256)] get => _caster._parent._abilities; }
                #endregion

                public int Type { [Impl(256)] get => _type; }
                public int Id { [Impl(256)] get => _id; }
                public int Weight { [Impl(256)] get => _weight; }

                [Impl(256)] protected Cast(Caster parent, int type, int id, bool lowWeight) : this(parent, type, id, s_weights[type][id] >> (lowWeight ? 3 : 0)) { }
                [Impl(256)] protected Cast(Caster parent, int type, int id) : this(parent, type, id, s_weights[type][id]) { }
                private Cast(Caster parent, int type, int id, int weights)
                {
                    _caster = parent;
                    _param.playerId = parent._parent._id;

                    _type = type; _id = id;
                    _weight = weights;

                    parent._leveling.Add(this);
                }

                public abstract IEnumerator TryCasting_Cn();

                sealed public override string ToString() => GetType().Name;

                protected IEnumerator Casting_Cn(int valueA = 0, int valueB = 0)
                {
                    _param.valueA = valueA; _param.valueB = valueB;
                    SpellBook.Cast(_type, _id, _param);

                    yield return SpellBook.WaitEndCasting;
                }

                protected IEnumerator CanPayOrExchange_Cn(Out<bool> output)
                {
                    output.Set(CanPay);
                    if (!output && Chance.Rolling(Resources.PercentAmountExCurrency(CurrencyId.Mana) - s_settings.percentAmountOffset))
                        yield return Human.Exchange_Cn(Spell.Cost, output);
                }
            }
        }
    }
}
