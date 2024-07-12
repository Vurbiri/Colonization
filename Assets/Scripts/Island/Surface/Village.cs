using System.Collections;
using UnityEngine;

public class Village : ASurfaceCreated
{
    [Space]
    [SerializeField] private MeshFilter _windmillMesh;
    [SerializeField, GetComponentInChildren] private Animator _windmillAnimator;
    [Space]
    [SerializeField] private Mesh[] _windmillMeshes;
    [Space]
    [SerializeField] private float _windmillOffsetDistance = 0.6f;
    [SerializeField] private float _windmillSpeedAnimRange = 0.2f;
    [SerializeField] private Vector2 _windmillPlayRange = new(0.2f, 1.1f);

    private const string NAME_ANIM_PARAMETER = "Play";

    public override void Initialize()
    {
        float size = CONST.HEX_HEIGHT * _ratioSize;

        transform.localRotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
        StartCoroutine(_generator.Generate_Coroutine(size));

        _windmillMesh.sharedMesh = _windmillMeshes[Random.Range(0, _windmillMeshes.Length)];
        _windmillMesh.transform.localPosition = new(0f, 0f, size - _windmillOffsetDistance);

        StartCoroutine(WindmillPlay_Coroutine());

        #region Local: WindmillPlay_Coroutine()
        //=================================
        IEnumerator WindmillPlay_Coroutine()
        {
            yield return new WaitForSeconds(URandom.Range(_windmillPlayRange));
            _windmillAnimator.SetTrigger(NAME_ANIM_PARAMETER);
            _windmillAnimator.speed = 1f + Random.Range(-_windmillSpeedAnimRange, _windmillSpeedAnimRange);
        }
        #endregion
    }
}
