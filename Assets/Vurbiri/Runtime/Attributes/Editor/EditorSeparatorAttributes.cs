using System;
using UnityEngine;

namespace Vurbiri
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class StartEditorAttribute : PropertyAttribute
    {
        public readonly string separator = "┌──────────────────── Editor ─────────────────────────";
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class EndEditorAttribute : PropertyAttribute
    {
        public readonly string separator = "└──────────────────────────────────────────────────";
    }
}
