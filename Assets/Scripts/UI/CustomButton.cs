using UnityEngine;
using UnityEngine.UI;


//[AddComponentMenu("UI/CustomButton", 35)]
public class CustomButton : Button
{
    [SerializeField] private Graphic[] _targetGraphics;

    public Graphic[] Graphics => _targetGraphics;
}
