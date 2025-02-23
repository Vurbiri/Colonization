//Assets\Vurbiri.TextLocalization\Editor\Scripts\LanguageType\LanguageTypesEditor.cs
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;
using static Vurbiri.Storage;

namespace Vurbiri.TextLocalization.Editor
{
    using static CONST;

    [CustomEditor(typeof(LanguageTypesScriptable), true)]
    internal class LanguageTypesEditor : AEditorGetVE<LanguageTypesEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAssetList;
        [SerializeField] private VisualTreeAsset _treeAssetItem;

        public override VisualElement CreateInspectorGUI()
        {
            var language = LanguageTypesScriptable.GetOrCreateSelf();

            List<string> choices = null;
            Dictionary<string, Lang> langs = null;

            LoadLangs();

            VisualElement root = _treeAssetList.CloneTree();

            var auto = root.Q<Toggle>("Auto");

            var list = root.Q<ListView>("Types");
            list.makeItem = MakeItem;

            root.Q<Button>("Load").clicked += language.Load;
            root.Q<Button>("Save").clicked += language.Save;

            return root;

            #region Local: LoadLangs(), MakeItem(), OnItemIndexChanged(...)
            //=================================
            void LoadLangs()
            {
                if (!LoadObjectFromResourceJson(LANGS, out Lang[] arr))
                    return;

                int count = arr.Length;

                choices = new(count);
                langs = new(count);

                foreach (var lang in arr)
                {
                    choices.Add(lang.Code);
                    langs.Add(lang.Code, lang);
                }
            }
            //=================================
            VisualElement MakeItem()
            {
                VisualElement item = _treeAssetItem.CloneTree();

                bool enabled = false;

                var id = item.Q<IntegerField>("Id");
                var folder = item.Q<TextField>("Folder");
                var name = item.Q<TextField>("Name");

                id.SetEnabled(false);
                SetEnabled(!auto.value);
                auto.RegisterValueChangedCallback(evt => SetEnabled(!evt.newValue));

                var code = item.Q<DropdownField>("Code");
                code.choices = choices;
                code.RegisterValueChangedCallback(OnCodeChanged);

                return item;

                #region Local: SetEnabled(...), OnCodeChanged(...)
                //---------------------------------------
                void SetEnabled(bool e)
                {
                    enabled = e;

                    folder.SetEnabled(enabled);
                    name.SetEnabled(enabled);
                }
                //---------------------------------------
                void OnCodeChanged(ChangeEvent<string> changeEvent)
                {
                    if (!enabled && langs.TryGetValue(changeEvent.newValue, out Lang lang))
                    {
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
            public readonly string Code;
            public readonly string Folder;
            public readonly string Name;

            [JsonConstructor]
            public Lang(string code, string folder, string name)
            {
                Code = code;
                Folder = folder;
                Name = name;
            }
        }
        #endregion
    }
}
