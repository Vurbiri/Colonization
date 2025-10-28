namespace Vurbiri.Colonization
{
    public struct Layers
    {
        public const int Default            = 0;
        public const int TransparentFX      = 1;
        public const int IgnoreRaycast      = 2;
        public const int Script             = 3;
        public const int Water              = 4;
        public const int UI                 = 5;
        public const int Graphics           = 6;
        public const int SelectableRight    = 8;
        public const int UIGame             = 9;
        public const int SelectableLeft     = 10;
    }

    public struct LayersMask
	{
        public const int None               = 0;
        public const int Default            = 1 << Layers.Default;
        public const int TransparentFX      = 1 << Layers.TransparentFX;
        public const int IgnoreRaycast      = 1 << Layers.IgnoreRaycast;
        public const int Script             = 1 << Layers.Script;
        public const int Water              = 1 << Layers.Water;
        public const int UI                 = 1 << Layers.UI;
        public const int Graphics           = 1 << Layers.Graphics;
        public const int SelectableRight    = 1 << Layers.SelectableRight;
        public const int UIGame             = 1 << Layers.UIGame;
        public const int SelectableLeft     = 1 << Layers.SelectableLeft;
    }
}
