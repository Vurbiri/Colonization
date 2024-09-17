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

            YandexSDK ysdk = YandexSDK.Instance;
            Language localization = Language.Instance;
            SettingsData settings = SettingsData.Instance;


            if (!localization.Initialize())
                Message.Error("Error loading Localization!");

            yield return StartCoroutine(InitializeYSDK_Coroutine());

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

            #region Local: InitializeYSDK_Coroutine(), CreateStorages_Coroutine()
            //==========================================
            IEnumerator InitializeYSDK_Coroutine()
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

                yield return StartCoroutine(InitializeStorages_Coroutine());

                #region Local: nitializeStorages_Coroutin
                //==========================================
                IEnumerator InitializeStorages_Coroutine()
                {
                    WaitReturnData<bool> waitReturn = new(this);
                    yield return waitReturn.Start(Storage.Initialize_Coroutine, _keySave);

                    Message.Log(waitReturn.Return ? "Storage initialize" : "Storage not initialize");

                    settings.IsFirstStart = !Load(waitReturn.Return);

                    #region Local Functions
                    bool Load(bool load)
                    {
                        bool result = false;

                        result = settings.Initialize(load) || result;
                        result = GameSettingsData.Instance.Initialize(load) || result;
                        return result;
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
