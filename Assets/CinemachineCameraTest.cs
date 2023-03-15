using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraTest : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;

    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if(GameManager.Instance.Player == null)
        {
            Debug.LogWarning("PlayerInstance is null");
        }
        cinemachineVirtualCamera.Follow = GameManager.Instance.Player.transform;
    }
}
