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

        private const string FIELD_LABEL = "Label", FIELD_CLIP = "clip", FIELD_TOTAL_T = "totalTime", FIELD_DAMAGES_T = "hitTimes";
        private const string FIELD_RANGE = "range", FIELD_DISTANCE = "distance";
        private const string BUTTON = "Select";

        public override VisualElement CreateInspectorGUI()
        {
            var root = _treeAnimationClipSettingsScriptable.CloneTree();

            root.Q<Label>(FIELD_LABEL).text = target.name;

            var clipUXML = root.Q<ObjectField>(FIELD_CLIP);
            clipUXML.RegisterCallback<ChangeEvent<Object>>(evt => Setup(evt.newValue as AnimationClip));
            root.Q<ListView>(FIELD_DAMAGES_T).makeItem = Make;

            Setup(clipUXML.value as AnimationClip);

            return root;

            #region Local: Setup(..), Make(..)
            //=================================
            void Setup(AnimationClip clip)
            {
                var totalTimeUXML = root.Q<FloatField>(FIELD_TOTAL_T);
                var damageTimeUXML = root.Q<ListView>(FIELD_DAMAGES_T);
                var rangeUXML = root.Q<Slider>(FIELD_RANGE);
                var distanceUXML = root.Q<Slider>(FIELD_DISTANCE);
                var button = root.Q<Button>(BUTTON);

                bool isShow = clip != null;

                totalTimeUXML.visible = isShow;
                damageTimeUXML.visible = isShow;
                rangeUXML.visible = isShow;
                button.visible = isShow;

                if (!isShow) return;

                totalTimeUXML.value = clip.length;

                button.clicked -= OnClick;
                button.clicked += OnClick;

                void OnClick() => Selection.activeObject = clip;
            }
            //=================================
            VisualElement Make() => new Slider
            {
                showInputField = true,
                highValue = 100f
            };
            #endregion
        }
    }
}
