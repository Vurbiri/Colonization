//Assets\Colonization\Scripts\UI\_UIGame\Panels\Abstract\EdificesPanel.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEdificesPanel : ATogglePanel<AEdificeButton>
    {
        [Space]
        [SerializeField] protected Id<EdificeGroupId> _id;

        protected IdArray<EdificeId, Sprite> _sprites;

        public virtual void Init(Human player, IdArray<EdificeId, Sprite> sprites, ProjectColors colors, InputController inputController, CanvasHint hint)
        {
            _sprites = sprites;
            _inputController = inputController;

            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());

            edifices.Subscribe(AddEdifice);

            Init(edifices.CountReactive, maxEdifices, colors, hint);
        }

        private void AddEdifice(int index, Crossroad crossroad, TypeEvent typeEvent)
        {
            if (typeEvent == TypeEvent.Add | typeEvent == TypeEvent.Subscribe)
            {
                var button = Instantiate(_buttonPrefab, _buttonContainer, false);
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
