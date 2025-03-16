//Assets\Colonization\Scripts\Data\PlayersData\DemonsSaveData.cs

namespace Vurbiri.Colonization.Data
{
    public class SatanSaveData : APlayerSaveData
    {
        public SatanSaveData(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage, isLoad)
        {
            if (!(isLoad && storage.TryGet(_keyBuffs, out int[] buffs)))
                buffs = new int[0];
        }
    }
}
