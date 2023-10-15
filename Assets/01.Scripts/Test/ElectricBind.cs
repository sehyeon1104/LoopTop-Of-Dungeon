using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX.Utility;

public class ElectricBind : MonoBehaviour
{
    public VFXPropertyBinder elecBinder;
    public VFXBinderBase elecTarget;
    // Start is called before the first frame update
    void Start()
    {
        elecBinder = GetComponent<VFXPropertyBinder>();
        elecTarget.GetComponent<MyVFXTransformBinder>().Target = GameManager.Instance.Player.transform;
    }

    void Update()
    {
        
    }
}
