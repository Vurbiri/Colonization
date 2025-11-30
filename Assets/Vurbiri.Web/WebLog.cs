namespace Vurbiri.Web
{
    sealed public class WebLog : Log
    {
        protected override void InfoInternal(string msg) => UtilityJS.LogJS(msg);
        protected override void WarningInternal(string msg) => UtilityJS.LogJS(msg);
        protected override void ErrorInternal(string msg) => UtilityJS.ErrorJS(msg);
    }
}
