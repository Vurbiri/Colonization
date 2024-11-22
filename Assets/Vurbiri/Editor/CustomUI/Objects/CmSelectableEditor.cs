//Assets\Vurbiri\Editor\CustomUI\Objects\CmSelectableEditor.cs
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

namespace VurbiriEditor.UI
{
    [CustomEditor(typeof(CmSelectable), true), CanEditMultipleObjects]
    public class CmSelectableEditor : Editor
    {
        protected SerializedProperty _interactableIconProperty;
        protected SerializedProperty _alfaColliderProperty;
        protected SerializedProperty _thresholdProperty;
        protected SerializedProperty _targetGraphicsProperty;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_InteractableProperty;
        protected SerializedProperty m_TransitionProperty;
        protected SerializedProperty m_ColorBlockProperty;
        protected SerializedProperty m_SpriteStateProperty;
        protected SerializedProperty m_AnimTriggerProperty;
        protected SerializedProperty m_NavigationProperty;

        protected GUIContent m_VisualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

        protected AnimBool m_ShowColorTint = new();
        protected AnimBool m_ShowSpriteTrasition = new();
        protected AnimBool m_ShowAnimTransition = new();

        protected static List<CmSelectableEditor> s_Editors = new();

        protected static bool s_ShowNavigation = false;

        protected static string s_ShowNavigationKey = "ASelectableCustomEditor.ShowNavigation";

        protected const float kArrowThickness = 2.5f;
        protected const float kArrowHeadSize = 1.2f;

        protected Selectable.Transition _transition;

        protected virtual void OnEnable()
        {
            _interactableIconProperty = serializedObject.FindProperty("_interactableIcon");
            _alfaColliderProperty = serializedObject.FindProperty("_alfaCollider");
            _thresholdProperty = serializedObject.FindProperty("_threshold");
            _targetGraphicsProperty = serializedObject.FindProperty("_targetGraphics");
            m_Script = serializedObject.FindProperty("m_Script");
            m_InteractableProperty = serializedObject.FindProperty("m_Interactable");
            m_TransitionProperty = serializedObject.FindProperty("m_Transition");
            m_ColorBlockProperty = serializedObject.FindProperty("m_Colors");
            m_SpriteStateProperty = serializedObject.FindProperty("m_SpriteState");
            m_AnimTriggerProperty = serializedObject.FindProperty("m_AnimationTriggers");
            m_NavigationProperty = serializedObject.FindProperty("m_Navigation");

            _transition = (Selectable.Transition)m_TransitionProperty.enumValueIndex;
            m_ShowColorTint.value = _transition == Selectable.Transition.ColorTint;
            m_ShowSpriteTrasition.value = _transition == Selectable.Transition.SpriteSwap;
            m_ShowAnimTransition.value = _transition == Selectable.Transition.Animation;

            m_ShowColorTint.valueChanged.AddListener(Repaint);
            m_ShowSpriteTrasition.valueChanged.AddListener(Repaint);
            s_Editors.Add(this);
            RegisterStaticOnSceneGUI();
            s_ShowNavigation = EditorPrefs.GetBool(s_ShowNavigationKey);
        }

        protected virtual void OnDisable()
        {
            m_ShowColorTint.valueChanged.RemoveListener(Repaint);
            m_ShowSpriteTrasition.valueChanged.RemoveListener(Repaint);
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
            _transition = (Selectable.Transition)m_TransitionProperty.enumValueIndex;

            serializedObject.Update();
            CustomStartPropertiesGUI();
            BeginPropertiesGUI();
            GraphicsPropertiesGUI();
            GroupBlocksPropertiesGUI();
            NavigationPropertiesGUI();
            CustomEndPropertiesGUI();
            serializedObject.ApplyModifiedProperties();
        }
        protected virtual void CustomStartPropertiesGUI()
        {

        }
        protected virtual void CustomEndPropertiesGUI()
        {

        }

        protected void BeginPropertiesGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_InteractableProperty);
            EditorGUILayout.PropertyField(_interactableIconProperty);
            GameObject go = _interactableIconProperty.objectReferenceValue as GameObject;
            if (go != null)
                go.SetActive(!m_InteractableProperty.boolValue);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_TransitionProperty);
            EditorGUI.indentLevel++;
        }

        protected virtual void GraphicsPropertiesGUI()
        {
            if (_targetGraphicsProperty.arraySize == 0)
                _targetGraphicsProperty.InsertArrayElementAtIndex(0);

            if (_targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue == null)
                _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue = (target as Selectable).GetComponent<Graphic>();

            Graphic graphic = _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue as Graphic;
            Image image = graphic as Image;

            if (_transition != Selectable.Transition.Animation && image != null && image.sprite != null && image.sprite.texture.isReadable)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_alfaColliderProperty);
                if (_alfaColliderProperty.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_thresholdProperty);
                    EditorGUI.indentLevel--;
                }
            }

            if (_transition == Selectable.Transition.ColorTint)
            {
                EditorGUILayout.PropertyField(_targetGraphicsProperty);
                if (graphic == null)
                    EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", UnityEditor.MessageType.Warning);
            }
            else if (_transition == Selectable.Transition.SpriteSwap)
            {
                EditorGUILayout.PropertyField(_targetGraphicsProperty.GetArrayElementAtIndex(0), new GUIContent("Target Graphic"));
                if (image == null)
                    EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", UnityEditor.MessageType.Warning);
            }
            EditorGUILayout.Space();
        }

        protected void GroupBlocksPropertiesGUI()
        {
            Animator animator = (target as Selectable).GetComponent<Animator>();
            m_ShowColorTint.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.ColorTint;
            m_ShowSpriteTrasition.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.SpriteSwap;
            m_ShowAnimTransition.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.Animation;

            if (EditorGUILayout.BeginFadeGroup(m_ShowColorTint.faded))
            {
                EditorGUILayout.PropertyField(m_ColorBlockProperty);
            }
            EditorGUILayout.EndFadeGroup();

            if (EditorGUILayout.BeginFadeGroup(m_ShowSpriteTrasition.faded))
            {
                EditorGUILayout.PropertyField(m_SpriteStateProperty);
            }
            EditorGUILayout.EndFadeGroup();

            if (EditorGUILayout.BeginFadeGroup(m_ShowAnimTransition.faded))
            {
                EditorGUILayout.PropertyField(m_AnimTriggerProperty);
                if (animator == null || animator.runtimeAnimatorController == null)
                {
                    Rect controlRect = EditorGUILayout.GetControlRect();
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
            EditorGUILayout.EndFadeGroup();
        }

        protected void NavigationPropertiesGUI()
        {
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_NavigationProperty);
            EditorGUI.BeginChangeCheck();
            Rect controlRect2 = EditorGUILayout.GetControlRect();
            controlRect2.xMin += EditorGUIUtility.labelWidth;
            s_ShowNavigation = GUI.Toggle(controlRect2, s_ShowNavigation, m_VisualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(s_ShowNavigationKey, s_ShowNavigation);
                SceneView.RepaintAll();
            }
        }

        protected static AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, Selectable target)
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
