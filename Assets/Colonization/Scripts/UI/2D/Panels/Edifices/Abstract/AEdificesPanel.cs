using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEdificesPanel<TWidget, TButton> : ATogglePanel<TWidget, TButton> where TWidget : AHintWidget where TButton : AEdificeButton
    {
        [Space]
        [SerializeField] protected Id<EdificeGroupId> _id;

        private IdArray<EdificeId, Sprite> _sprites;

        protected void InitEdifice(ReactiveList<Crossroad> edifices, IdArray<EdificeId, Sprite> sprites, InputController inputController)
        {
            _sprites = sprites;
            _inputController = inputController;

            edifices.Subscribe(AddEdifice);
            InitToggle(edifices.CountReactive);
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
