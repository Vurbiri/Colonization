using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(CustomButton), true), CanEditMultipleObjects]
public class CustomButtonEditor : Editor
{
    private SerializedProperty _targetGraphicsProperty;

    //private SerializedProperty m_OnClickProperty;

    private SerializedProperty m_Script;

    private SerializedProperty m_InteractableProperty;

    private SerializedProperty m_TargetGraphicProperty;

    private SerializedProperty m_TransitionProperty;

    private SerializedProperty m_ColorBlockProperty;

    private SerializedProperty m_SpriteStateProperty;

    private SerializedProperty m_AnimTriggerProperty;

    private SerializedProperty m_NavigationProperty;

    private GUIContent m_VisualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

    private AnimBool m_ShowColorTint = new AnimBool();

    private AnimBool m_ShowSpriteTrasition = new AnimBool();

    private AnimBool m_ShowAnimTransition = new AnimBool();

    private static List<CustomButtonEditor> s_Editors = new List<CustomButtonEditor>();

    private static bool s_ShowNavigation = false;

    private static string s_ShowNavigationKey = "TestButtonEditor.ShowNavigation";

    private string[] m_PropertyPathToExcludeForChildClasses;

    private const float kArrowThickness = 2.5f;

    private const float kArrowHeadSize = 1.2f;

    protected virtual void OnEnable()
    {
        _targetGraphicsProperty = serializedObject.FindProperty("_targetGraphics");
        m_Script = base.serializedObject.FindProperty("m_Script");
        //m_OnClickProperty = base.serializedObject.FindProperty("m_OnClick");
        m_InteractableProperty = base.serializedObject.FindProperty("m_Interactable");
        m_TargetGraphicProperty = base.serializedObject.FindProperty("m_TargetGraphic");
        m_TransitionProperty = base.serializedObject.FindProperty("m_Transition");
        m_ColorBlockProperty = base.serializedObject.FindProperty("m_Colors");
        m_SpriteStateProperty = base.serializedObject.FindProperty("m_SpriteState");
        m_AnimTriggerProperty = base.serializedObject.FindProperty("m_AnimationTriggers");
        m_NavigationProperty = base.serializedObject.FindProperty("m_Navigation");
        m_PropertyPathToExcludeForChildClasses = new string[]{ m_Script.propertyPath, m_NavigationProperty.propertyPath, m_TransitionProperty.propertyPath, m_ColorBlockProperty.propertyPath, m_SpriteStateProperty.propertyPath, m_AnimTriggerProperty.propertyPath, m_InteractableProperty.propertyPath, m_TargetGraphicProperty.propertyPath, _targetGraphicsProperty.propertyPath };
        Selectable.Transition transition = GetTransition(m_TransitionProperty);
        m_ShowColorTint.value = transition == Selectable.Transition.ColorTint;
        m_ShowSpriteTrasition.value = transition == Selectable.Transition.SpriteSwap;
        m_ShowAnimTransition.value = transition == Selectable.Transition.Animation;
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

    private void RegisterStaticOnSceneGUI()
    {
        SceneView.duringSceneGui -= StaticOnSceneGUI;
        if (s_Editors.Count > 0)
        {
            SceneView.duringSceneGui += StaticOnSceneGUI;
        }
    }

    private static Selectable.Transition GetTransition(SerializedProperty transition)
    {
        return (Selectable.Transition)transition.enumValueIndex;
    }

    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_InteractableProperty);
        Selectable.Transition transition = GetTransition(m_TransitionProperty);
        Graphic graphic = m_TargetGraphicProperty.objectReferenceValue as Graphic;

        Animator animator = (base.target as Selectable).GetComponent<Animator>();
        m_ShowColorTint.target = !m_TransitionProperty.hasMultipleDifferentValues && transition == Selectable.Transition.ColorTint;
        m_ShowSpriteTrasition.target = !m_TransitionProperty.hasMultipleDifferentValues && transition == Selectable.Transition.SpriteSwap;
        m_ShowAnimTransition.target = !m_TransitionProperty.hasMultipleDifferentValues && transition == Selectable.Transition.Animation;
        EditorGUILayout.PropertyField(m_TransitionProperty);
        EditorGUI.indentLevel++;

        if (_targetGraphicsProperty.arraySize == 0)
            _targetGraphicsProperty.InsertArrayElementAtIndex(0);

        if (_targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue == null)
            _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue = graphic = (target as Selectable).GetComponent<Graphic>();

        if (graphic == null)
            graphic = _targetGraphicsProperty.GetArrayElementAtIndex(0).objectReferenceValue as Graphic;

        if (transition == Selectable.Transition.ColorTint)
            EditorGUILayout.PropertyField(_targetGraphicsProperty);
        else if(transition == Selectable.Transition.SpriteSwap)
            EditorGUILayout.PropertyField(m_TargetGraphicProperty);

        switch (transition)
        {
            case Selectable.Transition.ColorTint:
                if (graphic == null)
                {
                    EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", UnityEditor.MessageType.Warning);
                }

                break;
            case Selectable.Transition.SpriteSwap:
                if (graphic as Image == null)
                {
                    EditorGUILayout.HelpBox("You must have a Image target in order to use a sprite swap transition.", UnityEditor.MessageType.Warning);
                }

                break;
        }

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
                    AnimatorController animatorController = GenerateSelectableAnimatorContoller((base.target as Selectable).animationTriggers, base.target as Selectable);
                    if (animatorController != null)
                    {
                        if (animator == null)
                        {
                            animator = (base.target as Selectable).gameObject.AddComponent<Animator>();
                        }

                        AnimatorController.SetAnimatorController(animator, animatorController);
                    }
                }
            }
        }

        EditorGUILayout.EndFadeGroup();
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

        EditorGUILayout.Space();
        //EditorGUILayout.PropertyField(m_OnClickProperty);
        ChildClassPropertiesGUI();
        base.serializedObject.ApplyModifiedProperties();
    }

    private void ChildClassPropertiesGUI()
    {
        if (!IsDerivedSelectableEditor())
        {
            Editor.DrawPropertiesExcluding(base.serializedObject, m_PropertyPathToExcludeForChildClasses);
        }
    }

    private bool IsDerivedSelectableEditor()
    {
        return GetType() != typeof(CustomButton);
    }

    private static AnimatorController GenerateSelectableAnimatorContoller(AnimationTriggers animationTriggers, Selectable target)
    {
        if (target == null)
        {
            return null;
        }

        string saveControllerPath = GetSaveControllerPath(target);
        if (string.IsNullOrEmpty(saveControllerPath))
        {
            return null;
        }

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

    private static string GetSaveControllerPath(Selectable target)
    {
        string text = target.gameObject.name;
        string message = $"Create a new animator for the game object '{text}':";
        return EditorUtility.SaveFilePanelInProject("New Animation Contoller", text, "controller", message);
    }

    private static void SetUpCurves(AnimationClip highlightedClip, AnimationClip pressedClip, string animationPath)
    {
        string[] array = new string[3] { "m_LocalScale.x", "m_LocalScale.y", "m_LocalScale.z" };
        Keyframe[] keys = new Keyframe[3]
        {
            new Keyframe(0f, 1f),
            new Keyframe(0.5f, 1.1f),
            new Keyframe(1f, 1f)
        };
        AnimationCurve curve = new AnimationCurve(keys);
        string[] array2 = array;
        foreach (string inPropertyName in array2)
        {
            AnimationUtility.SetEditorCurve(highlightedClip, EditorCurveBinding.FloatCurve(animationPath, typeof(Transform), inPropertyName), curve);
        }

        Keyframe[] keys2 = new Keyframe[1]
        {
            new Keyframe(0f, 1.15f)
        };
        AnimationCurve curve2 = new AnimationCurve(keys2);
        string[] array3 = array;
        foreach (string inPropertyName2 in array3)
        {
            AnimationUtility.SetEditorCurve(pressedClip, EditorCurveBinding.FloatCurve(animationPath, typeof(Transform), inPropertyName2), curve2);
        }
    }

    private static string BuildAnimationPath(Selectable target)
    {
        Graphic targetGraphic = target.targetGraphic;
        if (targetGraphic == null)
        {
            return string.Empty;
        }

        GameObject gameObject = targetGraphic.gameObject;
        GameObject gameObject2 = target.gameObject;
        Stack<string> stack = new Stack<string>();
        while (gameObject2 != gameObject)
        {
            stack.Push(gameObject.name);
            if (gameObject.transform.parent == null)
            {
                return string.Empty;
            }

            gameObject = gameObject.transform.parent.gameObject;
        }

        StringBuilder stringBuilder = new StringBuilder();
        if (stack.Count > 0)
        {
            stringBuilder.Append(stack.Pop());
        }

        while (stack.Count > 0)
        {
            stringBuilder.Append("/").Append(stack.Pop());
        }

        return stringBuilder.ToString();
    }

    private static AnimationClip GenerateTriggerableTransition(string name, AnimatorController controller)
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

    private static void StaticOnSceneGUI(SceneView view)
    {
        if (!s_ShowNavigation)
        {
            return;
        }

        Selectable[] allSelectablesArray = Selectable.allSelectablesArray;
        foreach (Selectable selectable in allSelectablesArray)
        {
            if (StageUtility.IsGameObjectRenderedByCamera(selectable.gameObject, Camera.current))
            {
                DrawNavigationForSelectable(selectable);
            }
        }
    }

    private static void DrawNavigationForSelectable(Selectable sel)
    {
        if (!(sel == null))
        {
            Transform transform = sel.transform;
            bool flag = Selection.transforms.Any((Transform e) => e == transform);
            Handles.color = new Color(1f, 0.6f, 0.2f, flag ? 1f : 0.4f);
            DrawNavigationArrow(-Vector2.right, sel, sel.FindSelectableOnLeft());
            DrawNavigationArrow(Vector2.up, sel, sel.FindSelectableOnUp());
            Handles.color = new Color(1f, 0.9f, 0.1f, flag ? 1f : 0.4f);
            DrawNavigationArrow(Vector2.right, sel, sel.FindSelectableOnRight());
            DrawNavigationArrow(-Vector2.up, sel, sel.FindSelectableOnDown());
        }
    }

    private static void DrawNavigationArrow(Vector2 direction, Selectable fromObj, Selectable toObj)
    {
        if (!(fromObj == null) && !(toObj == null))
        {
            Transform transform = fromObj.transform;
            Transform transform2 = toObj.transform;
            Vector2 vector = new Vector2(direction.y, 0f - direction.x);
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
    }

    private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
    {
        if (rect == null)
        {
            return Vector3.zero;
        }

        if (dir != Vector2.zero)
        {
            dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        }

        dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
        return dir;
    }
}
