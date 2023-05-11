using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        transform.position = new Vector3(0, 0, -10);
    }

    public void MoveMinimapCamera(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, -10);
    }

}
