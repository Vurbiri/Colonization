//Assets\Colonization\Scripts\UI\_UIGame\Panels\Abstract\EdificesPanel.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEdificesPanel<T> : ATogglePanel<T>, IValueId<EdificeGroupId> where T : AEdificeButton
    {
        [Space]
        [SerializeField] protected Id<EdificeGroupId> _id;

        private IdArray<EdificeId, Sprite> _sprites;

        public Id<EdificeGroupId> Id => _id;

        public void Init(Human player, ProjectColors colors, IdArray<EdificeId, Sprite> sprites, InputController inputController)
        {
            _sprites = sprites;
            _inputController = inputController;

            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());

            edifices.Subscribe(AddWarrior);

            Init(edifices.CountReactive, maxEdifices, colors);
        }

        private void AddWarrior(int index, Crossroad crossroad, TypeEvent typeEvent)
        {
            if (typeEvent == TypeEvent.Add | typeEvent == TypeEvent.Subscribe)
            {
                T button = Instantiate(_buttonPrefab, _buttonContainer, false);
                button.Init(crossroad, _inputController, index, _sprites[crossroad.Id], _toggle.IsOn);
                _buttons.Add(button);
            }
            else if(typeEvent == TypeEvent.Change)
            {
                _buttons[index].OnChange(crossroad, _sprites[crossroad.Id]);
            }
        }

    }
}
