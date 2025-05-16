//Assets\Vurbiri.International\Editor\Scripts\LanguageType\LanguageTypesEditor.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;
using static Vurbiri.Storage;

namespace Vurbiri.International.Editor
{
    [CustomEditor(typeof(LanguageTypesScriptable), true)]
    internal class LanguageTypesEditor : AEditorGetVE<LanguageTypesEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;
        [SerializeField] private VisualTreeAsset _treeAssetItem;

        public override VisualElement CreateInspectorGUI()
        {
            var language = LanguageTypesScriptable.GetOrCreateSelf();

            var allLang = LoadObjectFromResourceJson<LanguageType[]>(CONST.LANG_LIST);
            Dictionary<SystemLanguage, LanguageType> langs = new(allLang.Length);
            foreach (var lang in allLang)
                langs.Add(lang.Id, lang);

            VisualElement root = _treeAssetList.CloneTree();

            root.Q<Label>("Label").text = CONST.PROJECT_TYPES_LABEL;

            var notEdit = root.Q<Toggle>("Readonly");
            bool auto = notEdit.value;
            notEdit.RegisterValueChangedCallback(evt => auto = evt.newValue);

            var list = root.Q<ListView>("Types");
            list.makeItem = MakeItem;

            root.Q<Button>("Load").clicked += language.Load;
            root.Q<Button>("Save").clicked += language.Save;

            return root;

            #region Local: LoadLangs(), MakeItem(), OnItemIndexChanged(...)
            //=================================
            VisualElement MakeItem()
            {
                VisualElement item = _treeAssetItem.CloneTree();

                var code = item.Q<TextField>("Code");
                var folder = item.Q<TextField>("Folder");
                var name = item.Q<TextField>("Name");

                SetEnabled(!notEdit.value);
                notEdit.RegisterValueChangedCallback(evt => SetEnabled(!evt.newValue));

                var id = item.Q<EnumField>("Id");
                id.RegisterValueChangedCallback(evt => OnCodeChanged((SystemLanguage)evt.newValue));

                return item;

                #region Local: SetEnabled(...), OnCodeChanged(...)
                //---------------------------------------
                void SetEnabled(bool active)
                {
                    code.SetEnabled(active);
                    folder.SetEnabled(active);
                    name.SetEnabled(active);
                }
                //---------------------------------------
                void OnCodeChanged(SystemLanguage id)
                {
                    if (auto && langs.TryGetValue(id, out LanguageType lang))
                    {
                        code.value = lang.Code;
                        folder.value = lang.Folder;
                        name.value = lang.Name;
                    }
                }
                #endregion
            }
            //=================================
            #endregion
        }

        #region Nested: Lang
        //***********************************
        private class Lang
        {
            public readonly SystemLanguage Id;
            public readonly string Code;
            public readonly string Folder;
            public readonly string Name;

            [JsonConstructor]
            public Lang(SystemLanguage id, string code, string folder, string name)
            {
                Id = id;
                Code = code;
                Folder = folder;
                Name = name;
            }
        }
        #endregion
    }
}
