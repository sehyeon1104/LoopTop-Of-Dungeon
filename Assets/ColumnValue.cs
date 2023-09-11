using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColumnValue : MonoBehaviour
{
    VisualEffect column;
    private void Awake()
    {
        column = Managers.Resource.Load<VisualEffect>("Assets/10.Effects/player/Power/Column.vfx");
    }
    public void ColumnStart()
    {
        column.SetVector3("LeftHand", new Vector3(-0.36f, -0.23f, 0));
        column.SetVector3("RightHand", new Vector3(0.47f, -0.29f, 0));
    }
    public void columnEnd()
    {
        column.SetVector3("LeftHand", new Vector3(-0.6f, -0.23f, 0));
        column.SetVector3("RightHand", new Vector3(0.73f, 0.14f, 0));
    }
}
