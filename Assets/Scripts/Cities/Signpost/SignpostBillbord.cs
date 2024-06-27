
using UnityEngine;

[RequireComponent (typeof(MeshRenderer))] 
public class SignpostBillboard : ACityGraphicSide
{
    private Players _players;
    private MeshRenderer _renderer;

    public override void Initialize()
    {
        base.Initialize();

        _players = Players.Instance;
        _renderer = GetComponent<MeshRenderer> ();
    }


    public override void RoadBuilt() 
    {
        _renderer.sharedMaterial = _players.Current.Material;
    }
}
