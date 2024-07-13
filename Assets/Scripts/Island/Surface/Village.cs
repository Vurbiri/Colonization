using System.Collections;
using UnityEngine;

public class Village : ASurfaceCreated
{
    [Space]
    [SerializeField] private MinusPlusRange _offsetAngle = 15f;
    [Space]
    [SerializeField] private MeshFilter _windmillMesh;
    [SerializeField, GetComponentInChildren] private Animator _windmillAnimator;
    [Space]
    [SerializeField] private Mesh[] _windmillMeshes;
    [SerializeField] private float _windmillOffsetDistance = 0.6f;
    [SerializeField] private float _windmillSpeedAnimRange = 0.2f;
    [SerializeField] private MinMax _windmillPlayRange = new(0.2f, 1.22f);

    private const string NAME_ANIM_PARAMETER = "Play";

    public override void Initialize()
    {
        float size = CONST.HEX_HEIGHT * _ratioSize;

        transform.localRotation = Quaternion.Euler(0f, _offsetAngle.Rand + 60f * Random.Range(0, 6) + 30f, 0f);
        StartCoroutine(_generator.Generate_Coroutine(size));

        _windmillMesh.sharedMesh = _windmillMeshes.Rand();
        _windmillMesh.transform.localPosition = new(0f, 0f, size - _windmillOffsetDistance);

        StartCoroutine(WindmillPlay_Coroutine());

        #region Local: WindmillPlay_Coroutine()
        //=================================
        IEnumerator WindmillPlay_Coroutine()
        {
            yield return new WaitForSeconds(_windmillPlayRange.Rand);
            _windmillAnimator.SetTrigger(NAME_ANIM_PARAMETER);
            _windmillAnimator.speed = 1f + Random.Range(-_windmillSpeedAnimRange, _windmillSpeedAnimRange);
        }
        #endregion
    }
}
