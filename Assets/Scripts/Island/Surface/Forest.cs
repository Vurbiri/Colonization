using UnityEngine;

public class Forest : ASurface
{
    public override void Initialize()
    {
        StartCoroutine(_generator.Generate_Coroutine(CONST.HEX_HEIGHT * _ratioSize));
    }
}
