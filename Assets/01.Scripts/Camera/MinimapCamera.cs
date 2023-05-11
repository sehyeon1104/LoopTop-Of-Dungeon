using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    public void MoveMinimapCamera(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, -10);
    }

}
