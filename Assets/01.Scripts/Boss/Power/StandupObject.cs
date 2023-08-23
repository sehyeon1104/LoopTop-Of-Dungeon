using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandupObject : MonoBehaviour
{
    public bool isStandUp = false;

    private void Update()
    {
        StandUp();
    }

    public void StandUp()
    {
        if (isStandUp)
            transform.localRotation = Quaternion.Euler(-90f, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
