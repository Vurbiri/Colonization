namespace Vurbiri
{
#pragma warning disable
    public static class Dummy
	{
        public static void Action() { }
        public static void Action<T>(T t) { }
        public static void Action<TA, TB>(TA ta, TB tb) { }
        public static void Action<TA, TB, TC>(TA ta, TB tb, TC tc) { }
        public static void Action<TA, TB, TC, TD>(TA ta, TB tb, TC tc, TD td) { }
    }
#pragma warning restore
}
