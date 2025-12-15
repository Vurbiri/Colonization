using UnityEngine;
using Vurbiri.EntryPoint;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class HexagonView : MonoBehaviour, ISelectableReference
    {
        private static Pool<HexagonMark> s_poolMarks;

        [SerializeField] private Collider _thisCollider;
        [SerializeField] private HexagonCaption _hexagonCaption;

        private Transform _thisTransform;
        private HexagonMark _mark;
        private Key _key;

        public ISelectable Selectable { [Impl(256)] get => GameContainer.Hexagons[_key]; }
        public HexagonCaption Caption { [Impl(256)] get => _hexagonCaption; }

        public static void Init(Pool<HexagonMark> poolMarks)
        {
            s_poolMarks = poolMarks;
            Transition.OnExit.Add(() => s_poolMarks = null);
        }

        public Vector3 Init(Key key, int id, SurfaceType surface)
        {
            _thisTransform = GetComponent<Transform>();
            _key = key;

            surface.Create(_thisTransform);

            _hexagonCaption.Init(id, surface.Profits, this);

            if (surface.IsWater)
            {
                Destroy(_thisCollider); _thisCollider = null;
            }
            else
            {
                _thisCollider.enabled = false;
            }

            return _thisTransform.localPosition;
        }

        [Impl(256)] public void ShowMark(bool isGreen) => (_mark ??= s_poolMarks.Get(_thisTransform, false)).View(isGreen);
        [Impl(256)] public void HideMark()
        {
            if (_mark != null)
            {
                s_poolMarks.Return(_mark); 
                _mark = null;
            }
        }

        [Impl(256)] public void SetSelectable(bool isGreen)
        {
            ShowMark(isGreen);
            _thisCollider.enabled = true;
        }
        [Impl(256)] public void SetUnselectable()
        {
            HideMark();
            _thisCollider.enabled = false;
        }

        [Impl(256)] public void SetSelectableForSwap()
        {
            SetSelectable(true);
            _hexagonCaption.SetActive(true);
        }
        [Impl(256)] public void SetSelectedForSwap(Color color)
        {
            _mark.View(false);
            _thisCollider.enabled = false;
            _hexagonCaption.SetColor(color);
        }
        [Impl(256)] public void SetUnselectableForSwap()
        {
            SetUnselectable();
            _hexagonCaption.SetActive(false);
        }

        [Impl(256)] public void SetCaptionActive(bool active) => _hexagonCaption.SetActive(active);

        private void OnDestroy()
        {
            _hexagonCaption.OnDestroy();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _thisCollider);

            _hexagonCaption.OnValidate(this);
        }
#endif
    }
}
