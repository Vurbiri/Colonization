using System.Collections;
using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.Colonization
{
    public class ProjectEntryPoint : AProjectEntryPoint
    {
        [Space]
        [SerializeField] private YandexSDK _ysdk;

        private void Start()
        {
            StartCoroutine(Run_Coroutine());
        }

        private IEnumerator Run_Coroutine()
        {
            _startScene.Start();

            _container.AddInstance(_ysdk);

            yield return StartCoroutine(Init_Coroutine());

            yield return _startScene.End();

            FindAndRunScene();
        }

        private IEnumerator Init_Coroutine()
        {
            //Message.Log("Start LoadingPreGame");

            Language localization = Language.Instance;
            if (!Language.IsValid)
                Message.Error("Error loading Localization!");

            SettingsData settings = SettingsData.Instance;

            yield return StartCoroutine(_ysdk.Init_Coroutine());

            //Banners.InstanceF.Initialize();

            yield return StartCoroutine(CreateStorages_Coroutine());

            if (!_ysdk.IsLogOn)
            {
                _loadingScreen.SmoothOff_Wait();
                LogOnPanel logOnPanel = FindAnyObjectByType<LogOnPanel>();
                yield return StartCoroutine(logOnPanel.TryLogOn_Coroutine(_ysdk));
                yield return _loadingScreen.SmoothOn_Wait();
                if (_ysdk.IsLogOn)
                    yield return StartCoroutine(CreateStorages_Coroutine());
            }

            //Message.Log("End LoadingPreGame");
            _startScene.End();

            #region Local: CreateStorages_Coroutine()
            //==========================================
            IEnumerator CreateStorages_Coroutine()
            {
                if (!Storage.StoragesCreate(_container))
                    Message.Log(localization.GetText(Files.Main, "ErrorStorage"));

                yield return StartCoroutine(InitStorages_Coroutine());

                #region Local: InitStorages_Coroutine()
                //==========================================
                IEnumerator InitStorages_Coroutine()
                {
                    WaitReturnData<bool> waitReturn = new(this);
                    yield return waitReturn.Start(Storage.Load_Coroutine, _keySave);

                    Message.Log(waitReturn.Return ? "Storage initialize" : "Storage not initialize");

                    Load(waitReturn.Return);

                    #region Local Functions
                    void Load(bool load)
                    {
                        bool result = false;
                        result = settings.Init(_container, load) || result;
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_ysdk == null )
                _ysdk = FindAnyObjectByType<YandexSDK>();
        }
#endif
    }
}
