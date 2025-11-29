using UnityEngine;
namespace Vurbiri.UI
{
	sealed public class MBButton : AVButton<Id<MBButtonId>>, IValueId<MBButtonId>
    {
        public static readonly Id<MBButtonId>[] Cancel = { MBButtonId.Cancel };
        public static readonly Id<MBButtonId>[] Ok = { MBButtonId.Ok };
        public static readonly Id<MBButtonId>[] OkNo = { MBButtonId.Ok, MBButtonId.No };
        public static readonly Id<MBButtonId>[] OkCancel = { MBButtonId.Ok, MBButtonId.Cancel };

        public Id<MBButtonId> Id => _value;

        protected override void Start()
        {
            base.Start();

#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            gameObject.SetActive(false);
        }
        
        public void Setup(Vector3 position)
        {
            transform.localPosition = position;
            gameObject.SetActive(true);
        }

        public void Deactivate() => gameObject.SetActive(false);
    }
}
