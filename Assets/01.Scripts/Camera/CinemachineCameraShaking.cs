using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CinemachineCameraShaking : MonoSingleton<CinemachineCameraShaking>
{

    //public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    [SerializeField]
    private float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    public Camera mainCam;
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void Start()
    {
        mainCam = Camera.main;

        if (VirtualCamera == null)
            VirtualCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
    }

    //// 디버깅
    //public void CameraShakeAtk()
    //{
    //    StartCoroutine(IECameraShakeOnce());
    //}

    ///// <summary>
    ///// 고정된 세기
    ///// </summary>
    //public void CameraShakeOnce()
    //{
    //    StopCoroutine(IECameraShakeOnce());
    //    StartCoroutine(IECameraShakeOnce());
    //}

    ///// <summary>
    ///// 세기 조절 가능
    ///// </summary>
    ///// <param name="power"></param>
    //public void CameraShakeOnce(float amplitude)
    //{
    //    StopCoroutine(IECameraShakeOnce(amplitude));
    //    StartCoroutine(IECameraShakeOnce(amplitude));
    //}

    //private IEnumerator IECameraShakeOnce()
    //{
    //    if (VirtualCamera != null && virtualCameraNoise != null)
    //    {
    //        virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
    //        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

    //        yield return new WaitForSeconds(0.1f);

    //        virtualCameraNoise.m_AmplitudeGain = 0f;
    //    }
    //    else
    //    {
    //        Debug.LogError("VirtualCamera is null or virtualCameraNoise is null");
    //    }
    //}

    //private IEnumerator IECameraShakeOnce(float amplitude)
    //{
    //    if (VirtualCamera != null && virtualCameraNoise != null)
    //    {
    //        virtualCameraNoise.m_AmplitudeGain = amplitude;
    //        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

    //        yield return new WaitForSeconds(0.1f);

    //        virtualCameraNoise.m_AmplitudeGain = 0f;
    //    }
    //    else
    //    {
    //        Debug.LogError("VirtualCamera is null or virtualCameraNoise is null");
    //    }
    //}

    //public void CameraShakeMultiple(float duration)
    //{
    //    StopCoroutine(IECameraShakeMultiple(duration));
    //    StartCoroutine(IECameraShakeMultiple(duration));
    //}

    //private IEnumerator IECameraShakeMultiple(float duration)
    //{
    //    ShakeElapsedTime = duration;

    //    if (VirtualCamera != null && virtualCameraNoise != null)
    //    {
    //        virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
    //        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

    //        yield return new WaitForSeconds(ShakeElapsedTime);

    //        virtualCameraNoise.m_AmplitudeGain = 0f;
    //    }
    //    else
    //    {
    //        Debug.LogError("VirtualCamera is null or virtualCameraNoise is null");
    //    }

    //    yield break;
    //}
    public void ChangeCam()
    {
        VirtualCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }
    public void CameraShake(float amplitude = 2f, float duration = 0.1f)
    {
        ChangeCam();
        StopCoroutine(IECameraShake(amplitude, duration));
        StartCoroutine(IECameraShake(amplitude,duration));
    }
    private IEnumerator IECameraShake(float amplitude,float duration)
    {
        ShakeElapsedTime = duration;

        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            virtualCameraNoise.m_AmplitudeGain = amplitude;
            virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

            yield return new WaitForSeconds(ShakeElapsedTime);

            virtualCameraNoise.m_AmplitudeGain = 0f;
        }
        else
        {
            Rito.Debug.LogError("VirtualCamera is null or virtualCameraNoise is null");
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