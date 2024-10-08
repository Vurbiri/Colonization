using System.Collections;
using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.Colonization
{
    public class ProjectEntryPoint : MonoBehaviour
    {
        [SerializeField] private string _keySave = "CLN";
        [Space]
        [SerializeField] private LoadScene _loadScene;
        [Space]
        [SerializeField, FindObject(true)] private LogOnPanel _logOnPanel;

        private void Start() => StartCoroutine(LoadingCoroutine());

        private IEnumerator LoadingCoroutine()
        {
            //Message.Log("Start LoadingPreGame");

            _loadScene.Start();

            if (!Language.IsValid)
                Message.Error("Error loading Localization!");

            YandexSDK ysdk = YandexSDK.Instance;
            Language localization = Language.Instance;
            SettingsData settings = SettingsData.Instance;

            yield return StartCoroutine(InitYSDK_Coroutine());

            //Banners.InstanceF.Initialize();

            yield return StartCoroutine(CreateStorages_Coroutine());

            //if (!ysdk.IsLogOn)
            //{
            //    yield return StartCoroutine(_logOnPanel.TryLogOnCoroutine());
            //    if (ysdk.IsLogOn)
            //        yield return StartCoroutine(CreateStoragesCoroutine());
            //}

            //Message.Log("End LoadingPreGame");
            _loadScene.End();

            #region Local: InitYSDK_Coroutine(), CreateStorages_Coroutine()
            //==========================================
            IEnumerator InitYSDK_Coroutine()
            {
                WaitResult<bool> waitResult;

                yield return (waitResult = ysdk.InitYsdk());
                if (!waitResult.Result)
                {
                    Message.Log("YandexSDK - initialization error!");
                    yield break;
                }

                yield return (waitResult = ysdk.InitPlayer());
                if (!waitResult.Result)
                    Message.Log("Player - initialization error!");

                yield return (waitResult = ysdk.InitLeaderboards());
                if (!waitResult.Result)
                    Message.Log("Leaderboards - initialization error!");
            }
            //==========================================
            IEnumerator CreateStorages_Coroutine()
            {
                if (!Storage.StoragesCreate())
                    Message.Log(localization.GetText(Files.Main, "ErrorStorage"));

                yield return StartCoroutine(InitStorages_Coroutine());

                #region Local: InitStorages_Coroutine()
                //==========================================
                IEnumerator InitStorages_Coroutine()
                {
                    WaitReturnData<bool> waitReturn = new(this);
                    yield return waitReturn.Start(Storage.Init_Coroutine, _keySave);

                    Message.Log(waitReturn.Return ? "Storage initialize" : "Storage not initialize");

                    Load(waitReturn.Return);

                    #region Local Functions
                    void Load(bool load)
                    {
                        bool result = false;
                        result = settings.Init(load) || result;
                        result = GameSettingsData.Instance.Init(load) || result;

                        settings.IsFirstStart = !result;
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        }

        //private void OnDisable() => YandexSDK.Instance.LoadingAPI_Ready();
    }
}
