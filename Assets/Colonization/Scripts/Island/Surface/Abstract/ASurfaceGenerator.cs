using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public abstract class ASurfaceGenerator : MonoBehaviour
{
    public abstract IEnumerator Generate_Coroutine(float size);
}
