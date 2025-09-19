using UnityEngine;
namespace Vurbiri.UI
{
	sealed public class MBButton : AVButton<Id<MBButtonId>>, IValueId<MBButtonId>
    {
        public static readonly Id<MBButtonId>[] Cancel = { MBButtonId.Cancel };
        public static readonly Id<MBButtonId>[] Ok = { MBButtonId.Ok };
        public static readonly Id<MBButtonId>[] OkNo = { MBButtonId.Ok, MBButtonId.No };
        public static readonly Id<MBButtonId>[] OkCancel = { MBButtonId.Ok, MBButtonId.Cancel };

        private GameObject _thisObject;

        public Id<MBButtonId> Id => _value;

        protected override void Start()
        {
            base.Start();

            _thisObject = gameObject;
            _thisObject.SetActive(false);
        }
        
        public void Setup(Vector3 position)
        {
            _thisRectTransform.localPosition = position;
            _thisObject.SetActive(true);
        }

        public void Deactivate() => _thisObject.SetActive(false);
    }
}
