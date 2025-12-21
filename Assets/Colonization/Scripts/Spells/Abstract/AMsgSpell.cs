using System.Runtime.CompilerServices;
using Vurbiri.International;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
    {
        protected abstract class AMsgSpell : ASpell
        {
            private readonly string _msgKey;
            protected string _strMsg;

            protected AMsgSpell(int type, int id) : base(type, id)
            {
                _msgKey = string.Concat(s_keys[type][id], "Msg");
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected void SetMsg(Localization localization)
            {
                _strMsg = string.Concat(_strName, "\n \n", localization.GetText(FILE, _msgKey));
            }
        }
    }
}
