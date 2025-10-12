namespace Vurbiri.Colonization
{
	public class Shore : WeightsList<Key>
    {
        public Shore() : base(Key.Zero, HEX.SIDES * (CONST.MAX_CIRCLES + HEX.SIDES)) { }

        public void Add(Crossroad crossroad)
        {
            if(crossroad.CanBuildOnShore)
            {
                base.Add(crossroad.Key, crossroad.Weight);
                crossroad.BannedBuild.Add(Remove);
            }
        }
    }
}
