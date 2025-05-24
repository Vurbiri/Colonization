using System.Collections;

namespace Vurbiri.EntryPoint
{
    public interface ILoadingScreen
	{
		public string Description { set; }
        public float Progress { set; }

        public IEnumerator SmoothOn();
        public IEnumerator SmoothOff();
    }
}
