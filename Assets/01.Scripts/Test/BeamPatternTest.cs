using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPatternTest : MonoBehaviour
{
    [SerializeField]GameObject startFX;
    [SerializeField] LineRenderer beam;

    float lineLength = 0.0f;
    float lineWidth = 0.5f;

    [SerializeField] float length;
    [SerializeField] float width;

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

                beam.startWidth = lineWidth;
                beam.endWidth = lineWidth;

                yield return null;
            }
            beam.startWidth = 0;
            beam.endWidth = 0;

        }

    }
}
