using UnityEngine;

public abstract class ASurfaceCreated : ASurface
{
    [SerializeField, GetComponentInChildren] protected ASurfaceGenerator _generator;
    [Space]
    [SerializeField, Range(0.1f, 1f)] protected float _ratioSize = 0.8f;

}
