using UnityEngine;
using Vurbiri.Reactive;
namespace Vurbiri.UI
{
	sealed public class MBButton : AVButton<Id<MBButtonId>>, IValueId<MBButtonId>
    {
        public static readonly Id<MBButtonId>[] Cancel = { MBButtonId.Cancel };
        public static readonly Id<MBButtonId>[] Ok = { MBButtonId.Ok };
        public static readonly Id<MBButtonId>[] OkNo = { MBButtonId.Ok, MBButtonId.No };

        private GameObject _thisObject;

        public Id<MBButtonId> Id => _value;

        public ISubscription<Id<MBButtonId>> Init()
        {
            _thisObject = gameObject;
            _thisObject.SetActive(false);

            return _onClick;
        }
        
        public void Setup(Vector3 position)
        {
            _thisRectTransform.localPosition = position;
            _thisObject.SetActive(true);
        }

        public void Deactivate() => _thisObject.SetActive(false);
    }
}
