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
        private static readonly GUIContent[] s_nameTransition = new GUIContent[]{ new("None"), new("Color Tint"), new("Sprite Swap") };
        private static readonly int[] s_idTransition = new[] { 0, 1, 2 };

        protected SerializedProperty _interactableIconProperty;
        protected SerializedProperty _targetGraphicsProperty;
        protected SerializedProperty _targetGraphicProperty;
        protected SerializedProperty _script;
        protected SerializedProperty _interactableProperty;
        protected SerializedProperty _transitionProperty;
        protected SerializedProperty _colorBlockProperty;
        protected SerializedProperty _spriteStateProperty;
        protected SerializedProperty _isScalingProperty;
        protected SerializedProperty _scalingTargetProperty;
        protected SerializedProperty _scaleBlockProperty;
        protected SerializedProperty _navigationProperty;

        private ColorBlockDrawer _colorBlockDrawer;
        private ScaleBlockDrawer _scaleBlockDrawer;

        private readonly GUIContent _visualizeNavigation = EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

        private readonly AnimBool _showColorTint = new();
        private readonly AnimBool _showSpriteTransition = new();
        private readonly AnimBool _showAnimTransition = new();
        private readonly AnimBool _showScaling = new();

        private static readonly List<VSelectableEditor> s_editors = new();

        private static bool s_showNavigation = false;
        private static readonly string s_showNavigationKey = "ASelectableCustomEditor.ShowNavigation";

        private const float kArrowThickness = 2.5f;
        private const float kArrowHeadSize = 1.2f;

        private VSelectable _vSelectable;
        private Selectable.Transition _transition;

        protected readonly List<SerializedProperty> _childrenProperties = new();

        protected virtual bool IsDerivedEditor => GetType() != typeof(VSelectableEditor);

        protected virtual void OnEnable()
        {
            _vSelectable = target as VSelectable;
            _transition = _vSelectable.transition;

            _interactableIconProperty = serializedObject.FindProperty("_interactableIcon");
            _targetGraphicsProperty   = serializedObject.FindProperty("_targetGraphics");
            _targetGraphicProperty    = serializedObject.FindProperty("m_TargetGraphic");
            _script                   = serializedObject.FindProperty("m_Script");
            _interactableProperty     = serializedObject.FindProperty("m_Interactable");
            _transitionProperty       = serializedObject.FindProperty("m_Transition");
            _colorBlockProperty       = serializedObject.FindProperty("m_Colors");
            _spriteStateProperty      = serializedObject.FindProperty("m_SpriteState");
            _isScalingProperty        = serializedObject.FindProperty("_scaling");
            _scalingTargetProperty    = serializedObject.FindProperty("_scalingTarget");
            _scaleBlockProperty       = serializedObject.FindProperty("_scales");
            _navigationProperty       = serializedObject.FindProperty("m_Navigation");

            _colorBlockDrawer         = new(_colorBlockProperty);
            _scaleBlockDrawer         = new(_scaleBlockProperty);

            _showColorTint.value           = _transition == Selectable.Transition.ColorTint;
            _showSpriteTransition.value    = _transition == Selectable.Transition.SpriteSwap;
            _showAnimTransition.value      = _transition == Selectable.Transition.Animation;
            _showScaling.value             = _isScalingProperty.boolValue;

            _showColorTint.valueChanged.AddListener(Repaint);
            _showSpriteTransition.valueChanged.AddListener(Repaint);
            _showScaling.valueChanged.AddListener(Repaint);

            s_editors.Add(this);
            RegisterStaticOnSceneGUI();
            s_showNavigation = EditorPrefs.GetBool(s_showNavigationKey);

            _transition = (Selectable.Transition)_transitionProperty.enumValueIndex;

            FindChildrenProperties();
        }

        protected virtual void OnDisable()
        {
            _showColorTint.valueChanged.RemoveListener(Repaint);
            _showSpriteTransition.valueChanged.RemoveListener(Repaint);
            _showScaling.valueChanged.RemoveListener(Repaint);

            s_editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }

        protected virtual HashSet<string> GetExcludePropertyPaths()
        {
            return new(13)
            {
                _interactableIconProperty.propertyPath,
                _targetGraphicsProperty.propertyPath,
                _isScalingProperty.propertyPath,
                _scalingTargetProperty.propertyPath,
                _scaleBlockProperty.propertyPath,
                _script.propertyPath,
                _navigationProperty.propertyPath,
                _transitionProperty.propertyPath,
                _colorBlockProperty.propertyPath,
                _spriteStateProperty.propertyPath,
                _interactableProperty.propertyPath,
                _targetGraphicProperty.propertyPath,
                serializedObject.FindProperty("m_AnimationTriggers").propertyPath,
            };
        }

        protected void RegisterStaticOnSceneGUI()
        {
            SceneView.duringSceneGui -= StaticOnSceneGUI;
            if (s_editors.Count > 0)
                SceneView.duringSceneGui += StaticOnSceneGUI;
        }

        sealed public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawChildrenProperties();
            CustomStartPropertiesGUI();
            InteractablePropertiesGUI();
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
            BeginVertical(STYLES.borderDark);
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
                PropertyField(_interactableProperty);
                PropertyField(_interactableIconProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(_vSelectable.gameObject.scene);

                Graphic icon = _interactableIconProperty.objectReferenceValue as Graphic;
                if (icon != null) icon.canvasRenderer.SetAlpha(_interactableProperty.boolValue ? 0f : 1f);
            }
            Space();
        }

        private void GraphicsAndGroupBlocksPropertiesGUI()
        {
            TargetGraphicProperty targetGraphic = UpdateTargetGraphics();

            BeginVertical(STYLES.borderLight);

            IntPopup(_transitionProperty, s_nameTransition, s_idTransition);
            _transition = (Selectable.Transition)_transitionProperty.enumValueIndex;

            bool colorTintMode = _transition == Selectable.Transition.ColorTint;
            _showColorTint.target = !_transitionProperty.hasMultipleDifferentValues && colorTintMode;
            _showSpriteTransition.target = !_transitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.SpriteSwap;
            _showAnimTransition.target = !_transitionProperty.hasMultipleDifferentValues && _transition == Selectable.Transition.Animation;

            Space(1f);

            // ========= ColorTint =================================
            if (BeginFadeGroup(_showColorTint.faded))
            {
                PropertyField(_targetGraphicsProperty);
                if (targetGraphic.IsNotGraphic)
                    HelpBox("You must have a Graphics target in order to use a color transition.", UnityEditor.MessageType.Warning);

                _colorBlockDrawer.Draw();
            }
            EndFadeGroup();
            // ========= SpriteSwap =================================
            if (BeginFadeGroup(_showSpriteTransition.faded))
            {
                PropertyField(_targetGraphicProperty);
                
                if (_targetGraphicProperty.objectReferenceValue is not Image)
                {
                    _targetGraphicProperty.objectReferenceValue = null;
                    HelpBox("You must have a Image target in order to use a sprite swap transition.", UnityEditor.MessageType.Warning);
                }
                targetGraphic.SetGraphic(_targetGraphicProperty);

                Space();
                PropertyField(_spriteStateProperty);
            }
            EndFadeGroup();
            // ========= Animation =================================
            if (BeginFadeGroup(_showAnimTransition.faded))
            {
                HelpBox("Animation is not supported.", UnityEditor.MessageType.Warning);
            }
            EndFadeGroup();
            // ========= Scaling =================================
            Space();
            PropertyField(_isScalingProperty);
            _showScaling.target = _isScalingProperty.boolValue;
            if (BeginFadeGroup(_showScaling.faded))
            {
                if(_scalingTargetProperty.objectReferenceValue == null)
                    _scalingTargetProperty.objectReferenceValue = _vSelectable.transform;

                PropertyField(_scalingTargetProperty);
                _scaleBlockDrawer.Draw(!colorTintMode);
            }
            EndFadeGroup();

            EndVertical();
        }

        private TargetGraphicProperty UpdateTargetGraphics()
        {
            if(_targetGraphicsProperty.arraySize == 0)
                _targetGraphicsProperty.InsertArrayElementAtIndex(0);

            TargetGraphicProperty targetGraphic = new(_targetGraphicsProperty.GetArrayElementAtIndex(0));
            if (targetGraphic.IsNull)
                targetGraphic.ReferenceValue = _vSelectable.GetComponent<Graphic>();

            _targetGraphicProperty.objectReferenceValue = targetGraphic.ReferenceValue;
            return targetGraphic;
        }

        private void NavigationPropertiesGUI()
        {
            Space();
            PropertyField(_navigationProperty);
            EditorGUI.BeginChangeCheck();
            Rect controlRect2 = GetControlRect();
            controlRect2.xMin += EditorGUIUtility.labelWidth;
            s_showNavigation = GUI.Toggle(controlRect2, s_showNavigation, _visualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(s_showNavigationKey, s_showNavigation);
                SceneView.RepaintAll();
            }
        }

        protected static void StaticOnSceneGUI(SceneView view)
        {
            if (!s_showNavigation)
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
