using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Warrior : MonoBehaviour, ISelectable
    {
        private int _id;
        private int _owner;
        private GameplayEventBus _eventBus;
        private ActorSkin _skin;
        private Transform _thisTransform;
        private GameObject _gameObject;
        private Hexagon _currentHex;

        public Vector3 Position => _thisTransform.position;

        public Warrior Init(int id, int owner, ActorSkin skin, Hexagon startHex, GameplayEventBus eventBus)
        {
            _id = id;
            _owner = owner;
            _skin = skin;
            _eventBus = eventBus;
            _currentHex = startHex;
            _thisTransform = transform;
            _gameObject = transform.gameObject;

            _thisTransform.SetLocalPositionAndRotation(_currentHex.Position, KeyToQuaternion(_currentHex.GetNearGroundHexOffset()));
            _currentHex.EnterActor(_owner);
            
            _gameObject.SetActive(true);
            
            return this;
        }

        public void Select()
        {
            _eventBus.TriggerWarriorSelect(this);
        }

        public void Unselect(ISelectable newSelectable)
        {
            _eventBus.TriggerWarriorUnselect(this);
        }

        private Quaternion KeyToQuaternion(Key key)
        {
            int index = 0;

            for(int i = 0; i < HEX_COUNT_SIDES; i++)
            {
                if (NEAR_HEX[i] == key)
                {
                    index = i; break;
                }
            }

            return ROTATIONS_60[index];
        }

    }
}
