using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	public abstract class ActorAISettingsWindow<TSettings, TActorId, TStateId> : EditorWindow 
        where TSettings : ActorsAISettings<TActorId, TStateId>, new() where TActorId : ActorId<TActorId> where TStateId : ActorAIStateId<TStateId>
    {
        protected static readonly Vector2 s_minSize = new(550f, 800f);

        [SerializeField] protected TSettings _settings;

        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;
        private StatesPriorityDrawer<TStateId> _prioritiesDrawer;
        private ActorsAISettingsDrawer _settingsDrawer;
        private Vector2 _scrollPos;

        protected abstract string Label { get; }

        protected abstract ActorsAISettingsDrawer GetSettingsDrawer(SerializedProperty property);

        private void OnEnable()
        {
            SettingsFileEditor.Load(ref _settings);
            _settings ??= new();
            _settings.OnValidate();

            _serializedObject = new(this);
            _serializedProperty = _serializedObject.FindProperty("_settings");

            CreateDrawers();
        }

        private void OnGUI()
        {
            _serializedObject.Update();
            CreateDrawers();
            BeginWindows();
            {
                Space(10f);
                LabelField(Label, STYLES.H1);

                BeginVertical(GUI.skin.box);
                {
                    _scrollPos = BeginScrollView(_scrollPos);
                    {
                        BeginVertical(STYLES.borderDark);
                        {
                            PropertyField(_serializedProperty);
                            Space(2f);
                        }
                        EndVertical();

                        _settingsDrawer.Draw();
                        _prioritiesDrawer.Draw();
                    }
                    EndScrollView();
                }
                EndVertical();
            }
            EndWindows();
            _serializedObject.ApplyModifiedProperties();
        }

        private void CreateDrawers()
        {
            _prioritiesDrawer ??= new(_serializedObject, _serializedProperty);
            _settingsDrawer ??= GetSettingsDrawer(_serializedProperty);
        }

        private void OnDisable()
        {
            SettingsFileEditor.Save(_settings);
        }
    }
}
