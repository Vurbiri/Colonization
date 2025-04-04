//Assets\Vurbiri.UI\Editor\Editors\CmSelectableEditor.cs
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(VSelectable), true), CanEditMultipleObjects]
    public class VSelectableEditor : Editor
    {
        protected SerializedProperty _interactableIconProperty;
        protected SerializedProperty _alfaColliderProperty;
        protected SerializedProperty _thresholdProperty;
        protected SerializedProperty _targetGraphicsProperty;
        protected SerializedProperty m_TargetGraphicProperty;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_InteractableProperty;
        protected SerializedProperty m_TransitionProperty;
        protected SerializedProperty m_ColorBlockProperty;
        protected SerializedProperty m_SpriteStateProperty;
        protected SerializedProperty m_AnimTriggerProperty;
        protected SerializedProperty m_NavigationProperty;

        private readonly GUIContent m_VisualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

        protected readonly AnimBool m_ShowColorTint = new();
        protected readonly AnimBool m_ShowSpriteTransition = new();
        protected readonly AnimBool m_ShowAnimTransition = new();
        private readonly AnimBool _showAlfaCollider = new();
        private readonly AnimBool _showThreshold = new();

        private static readonly List<VSelectableEditor> s_Editors = new();

        protected static bool s_ShowNavigation = false;
        protected static string s_ShowNavigationKey = "ASelectableCustomEditor.ShowNavigation";

        protected const float kArrowThickness = 2.5f;
        protected const float kArrowHeadSize = 1.2f;

        protected VSelectable _vSelectable;
        protected Selectable.Transition _transition;

        protected virtual void OnEnable()
        {
            _vSelectable = target as VSelectable;

            _interactableIconProperty = serializedObject.FindProperty("_interactableIcon");
            _alfaColliderProperty = serializedObject.FindProperty("_alfaCollider");
            _thresholdProperty = serializedObject.FindProperty("_threshold");
            _targetGraphicsProperty = serializedObject.FindProperty("_targetGraphics");
            m_TargetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");
            m_Script = serializedObject.FindProperty("m_Script");
            m_InteractableProperty = serializedObject.FindProperty("m_Interactable");
            m_TransitionProperty = serializedObject.FindProperty("m_Transition");
            m_ColorBlockProperty = serializedObject.FindProperty("m_Colors");
            m_SpriteStateProperty = serializedObject.FindProperty("m_SpriteState");
            m_AnimTriggerProperty = serializedObject.FindProperty("m_AnimationTriggers");
            m_NavigationProperty = serializedObject.FindProperty("m_Navigation");

            m_ShowColorTint.value = _transition == Selectable.Transition.ColorTint;
            m_ShowSpriteTransition.value = _transition == Selectable.Transition.SpriteSwap;
            m_ShowAnimTransition.value = _transition == Selectable.Transition.Animation;
            _showAlfaCollider.value = false;
            _showThreshold.value = _alfaColliderProperty.boolValue;

            m_ShowColorTint.valueChanged.AddListener(Repaint);
            m_ShowSpriteTransition.valueChanged.AddListener(Repaint);
            _showAlfaCollider.valueChanged.AddListener(Repaint);
            _showThreshold.valueChanged.AddListener(Repaint);

            s_Editors.Add(this);
            RegisterStaticOnSceneGUI();
            s_ShowNavigation = EditorPrefs.GetBool(s_ShowNavigationKey);

            _transition = (Selectable.Transition)m_TransitionProperty.enumValueIndex;
        }

        protected virtual void OnDisable()
        {
            m_ShowColorTint.valueChanged.RemoveListener(Repaint);
            m_ShowSpriteTransition.valueChanged.RemoveListener(Repaint);
            _showAlfaCollider.valueChanged.RemoveListener(Repaint);
            _showThreshold.valueChanged.RemoveListener(Repaint);
            s_Editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }

        protected void RegisterStaticOnSceneGUI()
        {
            SceneView.duringSceneGui -= StaticOnSceneGUI;
            if (s_Editors.Count > 0)
                SceneView.duringSceneGui += StaticOnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CustomStartPropertiesGUI();
            InteractablePropertiesGUI();
            AlfaColliderPropertiesGUI();
            CustomMiddlePropertiesGUI();
            GraphicsAndGroupBlocksPropertiesGUI();
            NavigationPropertiesGUI();
            CustomEndPropertiesGUI();
            serializedObject.ApplyModifiedProperties();
        }
        protected virtual void CustomStartPropertiesGUI()
        {

        }
        protected virtual void CustomMiddlePropertiesGUI()
        {

        }
        protected virtual void CustomEndPropertiesGUI()
        {

        }

        private void InteractablePropertiesGUI()
        {
            Space();
            EditorGUI.BeginChangeCheck();
            PropertyField(m_InteractableProperty);
            PropertyField(_interactableIconProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_vSelectable.gameObject.scene);

                Graphic icon = _interactableIconProperty.objectReferenceValue as Graphic;
                if (icon != null) icon.canvasRenderer.SetAlpha(m_InteractableProperty.boolValue ? 0f : 1f);
            }
            Space();
        }

        private void AlfaColliderPropertiesGUI()
        {
            Image image = m_TargetGraphicProperty.objectReferenceValue as Image;

            _showAlfaCollider.target = _transition != Selectable.Transition.Animation && image != null && image.sprite != null && image.sprite.texture.isReadable;

            if (BeginFadeGroup(_showAlfaCollider.faded))
            {
                Space();
                PropertyField(_alfaColliderProperty);
                _showThreshold.target = _alfaColliderProperty.boolValue;
                if (BeginFadeGroup(_showThreshold.faded))
                {
                    EditorGUI.indentLevel++;
                    PropertyField(_thresholdProperty);
                    EditorGUI.indentLevel--;
                }
                EndFadeGroup();
                Space();
            }
            else
            {
                _alfaColliderProperty.boolValue = false;
            }
            EndFadeGroup();
        }

        private void GraphicsAndGroupBlocksPropertiesGUI()
        {
            if (!Application.isPlaying)
            {
                if (_targetGraphicsProperty.arraySize == 0) _targetGraphicsProperty.InsertArrayElementAtIndex(0);
                if(_targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue == null)
                    _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue = _vSelectable.GetComponent<Graphic>();

                m_TargetGraphicProperty.objectReferenceValue = _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue;
            }

            PropertyField(m_TransitionProperty);
            _transition = (Selectable.Transition)m_TransitionProperty.enumValueIndex;

            m_ShowColorTint.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.ColorTint;
            m_ShowSpriteTransition.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.SpriteSwap;
            m_ShowAnimTransition.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.Animation;

            Graphic graphic = m_TargetGraphicProperty.objectReferenceValue as Graphic;

            EditorGUI.indentLevel++;
            // ========= ColorTint =================================
            if (BeginFadeGroup(m_ShowColorTint.faded))
            {
                PropertyField(_targetGraphicsProperty);
                if (graphic == null)
                    HelpBox("You must have a Graphics target in order to use a color transition.", UnityEditor.MessageType.Warning);
                
                PropertyField(m_ColorBlockProperty);
            }
            EndFadeGroup();
            // ========= SpriteSwap =================================
            if (BeginFadeGroup(m_ShowSpriteTransition.faded))
            {
                Image image = graphic as Image;
                PropertyField(m_TargetGraphicProperty);
                if (!Application.isPlaying)
                    _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue = m_TargetGraphicProperty.objectReferenceValue;
                if (image == null)
                    HelpBox("You must have a Image target in order to use a sprite swap transition.", UnityEditor.MessageType.Warning);

                Space();
                PropertyField(m_SpriteStateProperty);
            }
            EndFadeGroup();
            // ========= Animation =================================
            if (BeginFadeGroup(m_ShowAnimTransition.faded))
            {
                Animator animator = (target as Selectable).GetComponent<Animator>();
                PropertyField(m_AnimTriggerProperty);
                if (animator == null || animator.runtimeAnimatorController == null)
                {
                    Rect controlRect = GetControlRect();
                    controlRect.xMin += EditorGUIUtility.labelWidth;
                    if (GUI.Button(controlRect, "Auto Generate Animation", EditorStyles.miniButton))
                    {
                        AnimatorController animatorController = GenerateSelectableAnimatorContoller((target as Selectable).animationTriggers, target as Selectable);
                        if (animatorController != null)
                        {
                            if (animator == null)
                            {
                                animator = (target as Selectable).gameObject.AddComponent<Animator>();
                            }

                            AnimatorController.SetAnimatorController(animator, animatorController);
                        }
                    }
                }
            }
            EndFadeGroup();

            EditorGUI.indentLevel--;
        }

        private void NavigationPropertiesGUI()
        {
            Space();
            PropertyField(m_NavigationProperty);
            EditorGUI.BeginChangeCheck();
            Rect controlRect2 = GetControlRect();
            controlRect2.xMin += EditorGUIUtility.labelWidth;
            s_ShowNavigation = GUI.Toggle(controlRect2, s_ShowNavigation, m_VisualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(s_ShowNavigationKey, s_ShowNavigation);
                SceneView.RepaintAll();
            }
        }

        private static AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, Selectable target)
        {
            if (target == null)
                return null;

            string saveControllerPath = GetSaveControllerPath(target);
            if (string.IsNullOrEmpty(saveControllerPath))
                return null;

            string text = (string.IsNullOrEmpty(animationTriggers.normalTrigger) ? "Normal" : animationTriggers.normalTrigger);
            string text2 = (string.IsNullOrEmpty(animationTriggers.highlightedTrigger) ? "Highlighted" : animationTriggers.highlightedTrigger);
            string text3 = (string.IsNullOrEmpty(animationTriggers.pressedTrigger) ? "Pressed" : animationTriggers.pressedTrigger);
            string text4 = (string.IsNullOrEmpty(animationTriggers.selectedTrigger) ? "Selected" : animationTriggers.selectedTrigger);
            string text5 = (string.IsNullOrEmpty(animationTriggers.disabledTrigger) ? "Disabled" : animationTriggers.disabledTrigger);
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(saveControllerPath);
            GenerateTriggerableTransition(text, animatorController);
            GenerateTriggerableTransition(text2, animatorController);
            GenerateTriggerableTransition(text3, animatorController);
            GenerateTriggerableTransition(text4, animatorController);
            GenerateTriggerableTransition(text5, animatorController);
            AssetDatabase.ImportAsset(saveControllerPath);
            return animatorController;
        }

        protected static string GetSaveControllerPath(Selectable target)
        {
            string text = target.gameObject.name;
            string message = $"Create a new animator for the game object '{text}':";
            return EditorUtility.SaveFilePanelInProject("New Animation Contoller", text, "controller", message);
        }

        protected static void SetUpCurves(AnimationClip highlightedClip, AnimationClip pressedClip, string animationPath)
        {
            string[] array = new string[3] { "m_LocalScale.x", "m_LocalScale.y", "m_LocalScale.z" };
            Keyframe[] keys = new Keyframe[3] { new(0f, 1f), new(0.5f, 1.1f), new(1f, 1f) };
            AnimationCurve curve = new(keys);
            string[] array2 = array;
            foreach (string inPropertyName in array2)
                AnimationUtility.SetEditorCurve(highlightedClip, EditorCurveBinding.FloatCurve(animationPath, typeof(Transform), inPropertyName), curve);
            Keyframe[] keys2 = new Keyframe[1] { new(0f, 1.15f) };
            AnimationCurve curve2 = new(keys2);
            string[] array3 = array;
            foreach (string inPropertyName2 in array3)
                AnimationUtility.SetEditorCurve(pressedClip, EditorCurveBinding.FloatCurve(animationPath, typeof(Transform), inPropertyName2), curve2);
        }

        protected static string BuildAnimationPath(Selectable target)
        {
            Graphic targetGraphic = target.targetGraphic;
            if (targetGraphic == null)
                return string.Empty;

            GameObject gameObject = targetGraphic.gameObject;
            GameObject gameObject2 = target.gameObject;
            Stack<string> stack = new();
            while (gameObject2 != gameObject)
            {
                stack.Push(gameObject.name);
                if (gameObject.transform.parent == null)
                    return string.Empty;

                gameObject = gameObject.transform.parent.gameObject;
            }

            StringBuilder stringBuilder = new();
            if (stack.Count > 0)
                stringBuilder.Append(stack.Pop());

            while (stack.Count > 0)
                stringBuilder.Append("/").Append(stack.Pop());

            return stringBuilder.ToString();
        }

        protected static AnimationClip GenerateTriggerableTransition(string name, AnimatorController controller)
        {
            AnimationClip animationClip = AnimatorController.AllocateAnimatorClip(name);
            AssetDatabase.AddObjectToAsset(animationClip, controller);
            AnimatorState destinationState = controller.AddMotion(animationClip);
            controller.AddParameter(name, AnimatorControllerParameterType.Trigger);
            AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            AnimatorStateTransition animatorStateTransition = stateMachine.AddAnyStateTransition(destinationState);
            animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0f, name);
            return animationClip;
        }

        protected static void StaticOnSceneGUI(SceneView view)
        {
            if (!s_ShowNavigation)
                return;

            Selectable[] allSelectablesArray = Selectable.allSelectablesArray;
            foreach (Selectable selectable in allSelectablesArray)
            {
                if (StageUtility.IsGameObjectRenderedByCamera(selectable.gameObject, Camera.current))
                {
                    DrawNavigationForSelectable(selectable);
                }
            }
        }

        protected static void DrawNavigationForSelectable(Selectable sel)
        {
            if (sel == null)
                return;

            Transform transform = sel.transform;
            bool flag = Selection.transforms.Any((Transform e) => e == transform);
            Handles.color = new Color(1f, 0.6f, 0.2f, flag ? 1f : 0.4f);
            DrawNavigationArrow(-Vector2.right, sel, sel.FindSelectableOnLeft());
            DrawNavigationArrow(Vector2.up, sel, sel.FindSelectableOnUp());
            Handles.color = new Color(1f, 0.9f, 0.1f, flag ? 1f : 0.4f);
            DrawNavigationArrow(Vector2.right, sel, sel.FindSelectableOnRight());
            DrawNavigationArrow(-Vector2.up, sel, sel.FindSelectableOnDown());
        }

        protected static void DrawNavigationArrow(Vector2 direction, Selectable fromObj, Selectable toObj)
        {
            if (fromObj == null || toObj == null)
                return;

            Transform transform = fromObj.transform;
            Transform transform2 = toObj.transform;
            Vector2 vector = new(direction.y, 0f - direction.x);
            Vector3 vector2 = transform.TransformPoint(GetPointOnRectEdge(transform as RectTransform, direction));
            Vector3 vector3 = transform2.TransformPoint(GetPointOnRectEdge(transform2 as RectTransform, -direction));
            float num = HandleUtility.GetHandleSize(vector2) * 0.05f;
            float num2 = HandleUtility.GetHandleSize(vector3) * 0.05f;
            vector2 += transform.TransformDirection(vector) * num;
            vector3 += transform2.TransformDirection(vector) * num2;
            float num3 = Vector3.Distance(vector2, vector3);
            Vector3 vector4 = transform.rotation * direction * num3 * 0.3f;
            Vector3 vector5 = transform2.rotation * -direction * num3 * 0.3f;
            Handles.DrawBezier(vector2, vector3, vector2 + vector4, vector3 + vector5, Handles.color, null, 2.5f);
            Handles.DrawAAPolyLine(2.5f, vector3, vector3 + transform2.rotation * (-direction - vector) * num2 * 1.2f);
            Handles.DrawAAPolyLine(2.5f, vector3, vector3 + transform2.rotation * (-direction + vector) * num2 * 1.2f);
        }

        protected static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null)
                return Vector3.zero;

            if (dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }

    }
}
