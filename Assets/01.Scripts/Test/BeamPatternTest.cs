using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BeamPatternTest : MonoBehaviour
{
    [SerializeField] GameObject startFX;
    [SerializeField] LineRenderer beam;
    [SerializeField] Light2D beamLight;

    float lineLength = 0.0f;
    float lineWidth = 0.5f;

    [SerializeField] float length;
    [SerializeField] float width;
    [SerializeField] float intensity = 2f;

    private void Awake()
    {
   
    }
    void Start()
    {
        StartCoroutine(testPlay());
    }

    private IEnumerator testPlay()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            lineWidth = width;

            beam.startWidth = lineWidth;
            beam.endWidth = lineWidth;

            beam.SetPosition(1, Vector3.zero);

            while(beamLight.intensity <= intensity)
            {
                beamLight.intensity += 0.01f;
                yield return null;
            }
            CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);

            startFX.GetComponentInChildren<ParticleSystem>().Play();
            while (beam.GetPosition(1).x <= length)
            {
                lineLength += 0.5f;
                beam.SetPosition(1, new Vector3(lineLength, 0, 0));

                yield return null;
            }

            lineLength = 0;

            while(lineWidth >= 0.0f)
            {
                lineWidth -= Time.deltaTime;

                beamLight.intensity -= 0.01f;
                beam.startWidth = lineWidth;
                beam.endWidth = lineWidth;

                yield return null;
            }
            beamLight.intensity = 0;
            beam.startWidth = 0;
            beam.endWidth = 0;

        }

    }
}
