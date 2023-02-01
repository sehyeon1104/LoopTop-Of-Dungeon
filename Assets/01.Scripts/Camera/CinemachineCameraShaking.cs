using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CinemachineCameraShaking : MonoBehaviour
{

    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void Start()
    {
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    // µð¹ö±ë
    public void CameraShakeAtk()
    {
        StartCoroutine(IECameraShakeOnce());
    }

    private IEnumerator IECameraShakeOnce()
    {
        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

            yield return new WaitForEndOfFrame();

            virtualCameraNoise.m_AmplitudeGain = 0f;
        }
        else
        {
            Debug.LogError("VirtualCamera is null or virtualCameraNoise is null");
        }
    }

    private IEnumerator IECameraShakeMultiple()
    {
        ShakeElapsedTime = ShakeDuration;

        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            while(ShakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                ShakeElapsedTime -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            virtualCameraNoise.m_AmplitudeGain = 0f;
            ShakeElapsedTime = 0f;
        }
        else
        {
            Debug.LogError("VirtualCamera is null or virtualCameraNoise is null");
        }

        yield break;
    }

    //void Update()
    //{
    //    // TODO: Replace with your trigger
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        ShakeElapsedTime = ShakeDuration;
    //    }

    //    // If the Cinemachine componet is not set, avoid update
    //    if (VirtualCamera != null && virtualCameraNoise != null)
    //    {
    //        // If Camera Shake effect is still playing
    //        if (ShakeElapsedTime > 0)
    //        {
    //            // Set Cinemachine Camera Noise parameters
    //            virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
    //            virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

    //            // Update Shake Timer
    //            ShakeElapsedTime -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            // If Camera Shake effect is over, reset variables
    //            virtualCameraNoise.m_AmplitudeGain = 0f;
    //            ShakeElapsedTime = 0f;
    //        }
    //    }
    //}
}