using UnityEngine;

public abstract class ASurface : MonoBehaviour
{
    [SerializeField, GetComponentInChildren] protected ASurfaceGenerator _generator;
    [Space]
    [SerializeField, Range(0.1f, 1f)] protected float _ratioSize = 0.8f;

    public virtual void Initialize(float offsetY = 0f)
    {
        _generator.transform.localPosition = new(0f, offsetY, 0f);
        Initialize();
    }

    public abstract void Initialize();
}
