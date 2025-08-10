using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	sealed public class SpellCurrencyWidget : ASelectCurrencyCountWidget
    {
        private Action<int> a_changeCount;
        private Unsubscription _unsubscriber;

        public void Init(ACurrenciesReactive currencies, Action<int> action)
        {
            _unsubscriber = currencies.Get(_id).Subscribe(SetMax);
            a_changeCount = action;
            action(_count);
        }

        protected override void SetValue(int value)
        {
            bool changed = _count != value;
            base.SetValue(value);

            if (changed  & a_changeCount != null)
                a_changeCount(_count);
        }

        private void OnDestroy() => _unsubscriber?.Unsubscribe();

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private int _oldId = -1;
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) & _oldId != _id)
            {
                UnityEditor.SerializedObject so = new(this);
                so.FindProperty("_oldId").intValue = _id;
                so.ApplyModifiedProperties();

                SetSpite_Ed();
            }
        }
#endif
    }
}
