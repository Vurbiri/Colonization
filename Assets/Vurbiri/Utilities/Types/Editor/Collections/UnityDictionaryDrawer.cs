using UnityEditor;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryDrawer : AValueDrawer
    {
        protected override string NameValue => "_values";
    }
}
