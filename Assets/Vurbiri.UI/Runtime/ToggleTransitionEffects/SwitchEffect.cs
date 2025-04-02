//Assets\Vurbiri.UI\Runtime\ToggleTransitionEffects\SwitchEffect.cs
namespace Vurbiri.UI
{
    sealed public partial class VToggle
    {
        sealed private class SwitchEffect : TransitionEffect
        {
            internal static bool Validate(VToggle parent) => parent._checkmarkOn != null & parent._checkmarkOff != null;


            public override bool IsValid => Validate(_parent);

            public SwitchEffect(VToggle parent) : base(parent)
            {

                PlayInstant();
            }


            public override void PlayInstant()
            {

            }

            public override void PlayDuration()
            {

            }
        }
    }
}
