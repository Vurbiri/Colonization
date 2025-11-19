using UnityEditor;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public class ActorsAISettingsDrawer
    {
        private readonly int _count;
        private readonly ActorAISettingsDrawer[] _drawers;

        public ActorsAISettingsDrawer(int typeId, int count, string[] names, SerializedProperty mainProperty)
        {
            _count = count;
            _drawers = new ActorAISettingsDrawer[count];

            var arrayProperty = mainProperty.FindPropertyRelative("_settings");
            for (int i = 0; i < count; ++i)
                _drawers[i] = new(typeId, i, names[i], arrayProperty.GetArrayElementAtIndex(i));
        }

        public void Draw()
        {
            Space();
            BeginVertical(STYLES.border);
            {
                Space(5f);
                for (int i = 0; i < _count; ++i)
                    _drawers[i].Draw();
                Space(5f);
            }
            EndVertical();
        }

        // ************ Nested ****************
        private class ActorAISettingsDrawer
        {
            private readonly int _typeId, _id;

            private readonly SerializedProperty _parentProperty;

            private readonly SerializedProperty _supportProperty;
            private readonly SerializedProperty _raiderProperty;

            private readonly UsedDefenseDrawer _defenseDrawer;
            private readonly UsedHealDrawer _healDrawer;
            private readonly UsedSelfBuffsDrawer _selfBuffsDrawer;
            private readonly UsedDebuffsDrawer _debuffsDrawer;
            private readonly UsedAttacksDrawer _attacksDrawer;

            private readonly string _name;

            public ActorAISettingsDrawer(int typeId, int id, string name, SerializedProperty parentProperty)
            {
                _typeId = typeId; _id = id; _name = name;

                _parentProperty  = parentProperty;

                _supportProperty = parentProperty.FindPropertyRelative(nameof(ActorAISettings.support));
                _raiderProperty  = parentProperty.FindPropertyRelative(nameof(ActorAISettings.raider));

                _defenseDrawer   = new(parentProperty.FindPropertyRelative(nameof(ActorAISettings.defense)), typeId, id);

                _healDrawer      = new(parentProperty.FindPropertyRelative(nameof(ActorAISettings.heal)), typeId, id);
                _selfBuffsDrawer = new(parentProperty.FindPropertyRelative(nameof(ActorAISettings.selfBuffs)), typeId, id);
                _debuffsDrawer   = new(parentProperty.FindPropertyRelative(nameof(ActorAISettings.debuffs)), typeId, id);
                _attacksDrawer   = new(parentProperty.FindPropertyRelative(nameof(ActorAISettings.attacks)), typeId, id);
            }

            public void Draw()
            {
                BeginVertical(STYLES.borderLight);
                {
                    if(_parentProperty.isExpanded = Foldout(_parentProperty.isExpanded, _name))
                    {
                        Space(4f);
                        BeginVertical(STYLES.borderDark);
                        {
                            PropertyField(_raiderProperty);
                            PropertyField(_supportProperty);
                            Space();
                            _defenseDrawer.Draw();
                            Space();
                            if (_raiderProperty.isExpanded = Foldout(_raiderProperty.isExpanded, "Combat"))
                            {
                                BeginVertical(STYLES.border);
                                {
                                    _healDrawer.Draw();
                                    _selfBuffsDrawer.Draw();
                                    _debuffsDrawer.Draw();
                                    _attacksDrawer.Draw();
                                }
                                EndVertical();
                            }
                            Space(3f);
                        }
                        EndVertical();
                        Space(3f);
                    }
                }
                EndVertical();
            }
        }
        // ************************************
    }
}
