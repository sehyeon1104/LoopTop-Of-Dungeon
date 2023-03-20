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
    Vector3 tempScale;

    [SerializeField] float length;
    [SerializeField] float width;
    [SerializeField] float intensity = 2f;

    List<ParticleSystem> startFXList = new List<ParticleSystem>();

    private void Awake()
    {
        foreach(ParticleSystem fx in startFX.GetComponentsInChildren<ParticleSystem>())
            startFXList.Add(fx);

        tempScale = transform.localScale;
    }

    private void OnEnable()
    {
        Init();
        SetObjectFlip();
        StartCoroutine(OnBeam());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SetObjectFlip()
    {
        tempScale.y = transform.rotation.z >= 0.7f ? -1.35f : 1.35f;
        Debug.Log(transform.rotation.z);
        transform.localScale = tempScale;
    }

    private void Init()
    {
        lineLength = 0;
        lineWidth = width;

        beam.startWidth = lineWidth;
        beam.endWidth = lineWidth;

        beam.SetPosition(1, Vector3.zero);
    }

    private void CheckPlayer()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(width, length), 0);
        foreach(var col in cols)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameManager.Instance.Player.OnDamage(2, gameObject, 0);
            }
        }
    }

    private IEnumerator OnBeam()
    {
        yield return new WaitForSeconds(0.5f);

        while(beamLight.intensity <= intensity)
        {
            beamLight.intensity += 0.02f;
            yield return null;
        }

        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);
        for(int i = 0; i < startFXList.Count; i++)
        {
            startFXList[i].Play();
        }

        while (beam.GetPosition(1).x <= length)
        {
            lineLength += 1f;
            beam.SetPosition(1, new Vector3(lineLength, 0, 0));

            yield return null;
        }

        CheckPlayer();

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
