//Assets\Vurbiri.UI\Editor\Editors\VSelectableEditor.cs
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
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
        private const string P_GRAPHIC = "_graphic";
        private static readonly GUIContent[] nameTransition = new GUIContent[]{ new("None"), new("Color Tint"), new("Sprite Swap") };
        private static readonly int[] idTransition = new[] { 0, 1, 2 };

        protected SerializedProperty _interactableIconProperty;
        protected SerializedProperty _alphaColliderProperty;
        protected SerializedProperty _thresholdProperty;
        protected SerializedProperty _targetGraphicsProperty;
        protected SerializedProperty m_TargetGraphicProperty;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_InteractableProperty;
        protected SerializedProperty m_TransitionProperty;
        protected SerializedProperty m_ColorBlockProperty;
        protected SerializedProperty m_SpriteStateProperty;
        protected SerializedProperty _isScalingProperty;
        protected SerializedProperty _targetTransformProperty;
        protected SerializedProperty _scaleBlockProperty;
        protected SerializedProperty m_NavigationProperty;

        private ColorBlockDrawer _colorBlockDrawer;

        private readonly GUIContent m_VisualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

        protected readonly AnimBool m_ShowColorTint = new();
        protected readonly AnimBool m_ShowSpriteTransition = new();
        protected readonly AnimBool m_ShowAnimTransition = new();
        private readonly AnimBool _showThreshold = new();
        private readonly AnimBool _showScaling = new();

        private static readonly List<VSelectableEditor> s_Editors = new();

        protected static bool s_ShowNavigation = false;
        protected static string s_ShowNavigationKey = "ASelectableCustomEditor.ShowNavigation";

        protected const float kArrowThickness = 2.5f;
        protected const float kArrowHeadSize = 1.2f;

        protected VSelectable _vSelectable;
        protected Selectable.Transition _transition;

        protected readonly List<SerializedProperty> _childrenProperties = new();

        protected virtual bool IsDerivedEditor => GetType() != typeof(VSelectableEditor);

        protected virtual void OnEnable()
        {
            _vSelectable = target as VSelectable;
            _transition = _vSelectable.transition;

            _interactableIconProperty   = serializedObject.FindProperty("_interactableIcon");
            _alphaColliderProperty      = serializedObject.FindProperty("_alphaCollider");
            _thresholdProperty          = serializedObject.FindProperty("_threshold");
            _targetGraphicsProperty     = serializedObject.FindProperty("_targetGraphics");
            m_TargetGraphicProperty     = serializedObject.FindProperty("m_TargetGraphic");
            m_Script                    = serializedObject.FindProperty("m_Script");
            m_InteractableProperty      = serializedObject.FindProperty("m_Interactable");
            m_TransitionProperty        = serializedObject.FindProperty("m_Transition");
            m_ColorBlockProperty        = serializedObject.FindProperty("m_Colors");
            m_SpriteStateProperty       = serializedObject.FindProperty("m_SpriteState");
            _isScalingProperty          = serializedObject.FindProperty("_isScaling");
            _targetTransformProperty    = serializedObject.FindProperty("_targetTransform");
            _scaleBlockProperty         = serializedObject.FindProperty("_scaleBlock");
            m_NavigationProperty        = serializedObject.FindProperty("m_Navigation");

            _colorBlockDrawer           = new(m_ColorBlockProperty);

            m_ShowColorTint.value           = _transition == Selectable.Transition.ColorTint;
            m_ShowSpriteTransition.value    = _transition == Selectable.Transition.SpriteSwap;
            m_ShowAnimTransition.value      = _transition == Selectable.Transition.Animation;
            _showThreshold.value            = _alphaColliderProperty.boolValue;
            _showScaling.value              = _isScalingProperty.boolValue;

            m_ShowColorTint.valueChanged.AddListener(Repaint);
            m_ShowSpriteTransition.valueChanged.AddListener(Repaint);
            _showThreshold.valueChanged.AddListener(Repaint);
            _showScaling.valueChanged.AddListener(Repaint);

            s_Editors.Add(this);
            RegisterStaticOnSceneGUI();
            s_ShowNavigation = EditorPrefs.GetBool(s_ShowNavigationKey);

            _transition = (Selectable.Transition)m_TransitionProperty.enumValueIndex;

            FindChildrenProperties();
        }

        protected virtual void OnDisable()
        {
            m_ShowColorTint.valueChanged.RemoveListener(Repaint);
            m_ShowSpriteTransition.valueChanged.RemoveListener(Repaint);
            _showThreshold.valueChanged.RemoveListener(Repaint);
            _showScaling.valueChanged.RemoveListener(Repaint);

            s_Editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }

        protected virtual HashSet<string> GetExcludePropertyPaths()
        {
            return new(15)
            {
                _interactableIconProperty.propertyPath,
                _alphaColliderProperty.propertyPath,
                _thresholdProperty.propertyPath,
                _targetGraphicsProperty.propertyPath,
                _isScalingProperty.propertyPath,
                _targetTransformProperty.propertyPath,
                _scaleBlockProperty.propertyPath,
                m_Script.propertyPath,
                m_NavigationProperty.propertyPath,
                m_TransitionProperty.propertyPath,
                m_ColorBlockProperty.propertyPath,
                m_SpriteStateProperty.propertyPath,
                serializedObject.FindProperty("m_AnimationTriggers").propertyPath,
                m_InteractableProperty.propertyPath,
                m_TargetGraphicProperty.propertyPath,
            };
        }

        protected void RegisterStaticOnSceneGUI()
        {
            SceneView.duringSceneGui -= StaticOnSceneGUI;
            if (s_Editors.Count > 0)
                SceneView.duringSceneGui += StaticOnSceneGUI;
        }

        sealed public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawChildrenProperties();
            CustomStartPropertiesGUI();
            InteractablePropertiesGUI();
            AlfaColliderPropertiesGUI();
            CustomMiddlePropertiesGUI();
            GraphicsAndGroupBlocksPropertiesGUI();
            NavigationPropertiesGUI();
            CustomEndPropertiesGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void FindChildrenProperties()
        {
            if (IsDerivedEditor) return;

            HashSet<string> excludeProperties = GetExcludePropertyPaths();
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (!excludeProperties.Contains(iterator.name))
                    _childrenProperties.Add(iterator.Copy());
            }
        }

        private void DrawChildrenProperties()
        {
            if (_childrenProperties.Count == 0) return;

            Space(1f);
            BeginVertical(GUI.skin.box);
            foreach (var child in _childrenProperties)
                PropertyField(child, true);
            EndVertical();
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
            Space(1f);
            EditorGUI.BeginChangeCheck();
            PropertyField(m_InteractableProperty);
            PropertyField(_interactableIconProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_vSelectable.gameObject.scene);

                Graphic icon = _interactableIconProperty.objectReferenceValue as Graphic;
                if (icon != null) icon.canvasRenderer.SetAlpha(m_InteractableProperty.boolValue ? 0f : 1f);
            }
        }

        private void AlfaColliderPropertiesGUI()
        {
            Image image = m_TargetGraphicProperty.objectReferenceValue as Image;
            bool isAlpha = _transition != Selectable.Transition.Animation && image != null && image.sprite != null && image.sprite.texture.isReadable;
            if(!isAlpha) _alphaColliderProperty.boolValue = false;

            Space(1f);
            EditorGUI.BeginDisabledGroup(!isAlpha);
            PropertyField(_alphaColliderProperty);
            EditorGUI.EndDisabledGroup();
            _showThreshold.target = _alphaColliderProperty.boolValue;
            if (BeginFadeGroup(_showThreshold.faded))
            {
                EditorGUI.indentLevel++;
                PropertyField(_thresholdProperty);
                EditorGUI.indentLevel--;
            }
            EndFadeGroup();
            Space(1f);
        }

        private void GraphicsAndGroupBlocksPropertiesGUI()
        {
            SerializedProperty targetGraphic = UpdateTargetGraphics();

            IntPopup(m_TransitionProperty, nameTransition, idTransition);
            _transition = (Selectable.Transition)m_TransitionProperty.enumValueIndex;

            m_ShowColorTint.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.ColorTint;
            m_ShowSpriteTransition.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.SpriteSwap;
            m_ShowAnimTransition.target = !m_TransitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.Animation;

            Space(1f);
            EditorGUI.indentLevel++;
            // ========= ColorTint =================================
            if (BeginFadeGroup(m_ShowColorTint.faded))
            {
                PropertyField(_targetGraphicsProperty);
                if (targetGraphic.objectReferenceValue as Graphic == null)
                    HelpBox("You must have a Graphics target in order to use a color transition.", UnityEditor.MessageType.Warning);

                _colorBlockDrawer.Draw();
            }
            EndFadeGroup();
            // ========= SpriteSwap =================================
            if (BeginFadeGroup(m_ShowSpriteTransition.faded))
            {
                PropertyField(m_TargetGraphicProperty);
                
                if (m_TargetGraphicProperty.objectReferenceValue as Image == null)
                {
                    m_TargetGraphicProperty.objectReferenceValue = null;
                    HelpBox("You must have a Image target in order to use a sprite swap transition.", UnityEditor.MessageType.Warning);
                }
                targetGraphic.objectReferenceValue = m_TargetGraphicProperty.objectReferenceValue;

                Space();
                PropertyField(m_SpriteStateProperty);
            }
            EndFadeGroup();
            // ========= Animation =================================
            if (BeginFadeGroup(m_ShowAnimTransition.faded))
            {
                HelpBox("Animation is not supported.", UnityEditor.MessageType.Warning);
            }
            EndFadeGroup();
            // ========= Scaling =================================
            PropertyField(_isScalingProperty);
            _showScaling.target = _isScalingProperty.boolValue;
            if (BeginFadeGroup(_showScaling.faded))
            {
                if(_targetTransformProperty.objectReferenceValue == null)
                    _targetTransformProperty.objectReferenceValue = _vSelectable.transform;

                PropertyField(_targetTransformProperty);
                PropertyField(_scaleBlockProperty);
            }
            EndFadeGroup();

            EditorGUI.indentLevel--;
        }

        private SerializedProperty UpdateTargetGraphics()
        {
            if(_targetGraphicsProperty.arraySize == 0)
                _targetGraphicsProperty.InsertArrayElementAtIndex(0);

            SerializedProperty targetGraphic = _targetGraphicsProperty.GetArrayElementAtIndex(0).FindPropertyRelative(P_GRAPHIC);
            if (targetGraphic.objectReferenceValue == null)
                targetGraphic.objectReferenceValue = _vSelectable.GetComponent<Graphic>();

            m_TargetGraphicProperty.objectReferenceValue = targetGraphic.objectReferenceValue;
            return targetGraphic;
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
