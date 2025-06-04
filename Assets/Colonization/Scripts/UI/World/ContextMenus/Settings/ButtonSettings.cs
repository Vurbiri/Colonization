using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonSettings
    {
        public readonly Human player;
        public readonly WorldHint hint;
        public readonly ProjectColors colorSettings;

        public ButtonSettings(Human player, WorldHint hint)
        {
            this.player = player;
            this.hint = hint;
            colorSettings = SceneContainer.Get<ProjectColors>();
        }
    }
}
