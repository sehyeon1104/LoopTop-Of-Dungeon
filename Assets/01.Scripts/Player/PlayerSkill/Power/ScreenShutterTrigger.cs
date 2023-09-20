using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShutterTrigger : MonoBehaviour
{
    [SerializeField] private Transform shatterTransform;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            shatterTransform.gameObject.SetActive(false);
    }
}
