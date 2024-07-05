using System.Collections;
using UnityEngine;

public class LogOnPanel : MonoBehaviour
{
    private WaitResult<bool> _waitLogOn;
    YandexSDK _ysdk;

    public IEnumerator TryLogOnCoroutine()
    {
        _ysdk = YandexSDK.InstanceF;

        _waitLogOn = new();
        gameObject.SetActive(true);

        WaitResult<bool> waitResult;
        bool resultAuthorization = false;
        while (true)
        {
            yield return _waitLogOn;
            if (!_waitLogOn.Result)
                break;

            yield return StartCoroutine(AuthorizationCoroutine());
            if (resultAuthorization)
                break;

            _waitLogOn = new();
            //Message.BannerKey("ErrorLogon", MessageType.Error);
        }

        gameObject.SetActive(false);

        #region Local Function
        IEnumerator AuthorizationCoroutine()
        {
            ///Message.BannersClear();

            if (!_ysdk.IsPlayer)
            {
                yield return (waitResult = _ysdk.InitPlayer());
                if (!waitResult.Result)
                {
                    resultAuthorization = false;
                    yield break;
                }
            }

            if (!_ysdk.IsLogOn)
            {
                yield return (waitResult = _ysdk.LogOn());
                if (!waitResult.Result)
                {
                    resultAuthorization = false;
                    yield break;
                }
            }

            if (!_ysdk.IsLeaderboard)
                yield return _ysdk.InitLeaderboards();

            resultAuthorization = true;

        }
        #endregion
    }

    public void OnGuest()
    {
        _waitLogOn.SetResult(false);
    }

    public void OnLogOn()
    {
        _waitLogOn.SetResult(true);
    }

    private void Update()
    {
        if (_ysdk.IsLogOn)
            _waitLogOn.SetResult(true);
    }
}
