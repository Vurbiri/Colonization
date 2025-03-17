//Assets\Colonization\Scripts\Data\PlayersData\DemonsSaveData.cs

namespace Vurbiri.Colonization.Data
{
    public class SatanSaveData : APlayerSaveData
    {
        //private readonly string _keyBuffs;

        public SatanLoadData LoadData { get; set; }

        public SatanSaveData(IStorageService storage, bool isLoad) : base(PlayerId.Satan, storage, isLoad)
        {
            //string strId = PlayerId.Satan.ToString();
            //_keyBuffs = P_BUFFS.Concat(strId);

            if (isLoad)
                LoadData = new(storage.Get<int[]>(_keyArtefact), _actors);
        }
    }
}
