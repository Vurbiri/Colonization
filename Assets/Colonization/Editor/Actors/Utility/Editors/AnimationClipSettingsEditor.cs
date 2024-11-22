//Assets\Colonization\Editor\Actors\Utility\Editors\AnimationClipSettingsEditor.cs
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    [CustomEditor(typeof(AnimationClipSettingsScriptable), true)]
    public class AnimationClipSettingsEditor : AEditorGetVE<AnimationClipSettingsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAnimationClipSettingsScriptable;

        private const string FIELD_LABEL = "Label", FIELD_CLIP = "clip", FIELD_TOTAL_T = "totalTime", FIELD_DAMAGE_T = "damageTime", FIELD_RANGE = "range";

        public override VisualElement CreateInspectorGUI()
        {
            var root = _treeAnimationClipSettingsScriptable.CloneTree();

            root.Q<Label>(FIELD_LABEL).text = target.name;

            var clipUXML = root.Q<ObjectField>(FIELD_CLIP);
            clipUXML.RegisterCallback<ChangeEvent<Object>>(evt => Setup(evt.newValue as AnimationClip));

            Setup(clipUXML.value as AnimationClip);

            return root;

            #region Local: Setup(..)
            //=================================
            void Setup(AnimationClip clip)
            {
                var totalTimeUXML = root.Q<FloatField>(FIELD_TOTAL_T);
                var damageTimeUXML = root.Q<Slider>(FIELD_DAMAGE_T);
                var rangeUXML = root.Q<Slider>(FIELD_RANGE);

                bool isShow = clip != null;

                totalTimeUXML.visible = isShow;
                damageTimeUXML.visible = isShow;
                rangeUXML.visible = isShow;

                if (!isShow)
                    return;

                float time = clip.length;// - 0.1f;
                totalTimeUXML.value = time;
                damageTimeUXML.highValue = time;
            }
            #endregion
        }
    }
}
