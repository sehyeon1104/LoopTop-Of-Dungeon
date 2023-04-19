using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerBeam : MonoBehaviour
{
    public bool IsReady { get; private set; } = false;
    [SerializeField] GameObject beamPos;
    public float beamDuration =2;
    [SerializeField] LayerMask enemy;
    [SerializeField] GameObject startFX;
    [SerializeField] LineRenderer beam;
    [SerializeField] Light2D beamLight;

    //EdgeCollider2D col;
    //Vector2[] points;
    public float timerA { get; private set; } = 0;
    float lineLength = 0.0f;
  [HideInInspector] public float lineWidth = 0.5f;
    Vector3 tempScale;

    [SerializeField] float length;
    public float width;
    [SerializeField] float intensity = 2f;

    WaitForSeconds waitTime = new WaitForSeconds(0.4f);
    List<ParticleSystem> startFXList = new List<ParticleSystem>();


    private void Awake()
    {
        
        foreach (ParticleSystem fx in startFX.GetComponentsInChildren<ParticleSystem>())
            startFXList.Add(fx);
        tempScale = transform.localScale;
    }

    private void OnEnable()
    {
        
        Init();
        StartCoroutine(OnBeam());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void Init()
    {
        StopCoroutine(OnBeam());

        lineLength = 0;
        lineWidth = width;

        beam.startWidth = lineWidth;
        beam.endWidth = lineWidth;


        beam.SetPosition(1, Vector3.zero);
    }


    public IEnumerator OnBeam()
    {

        timerA = 0;
        float timer = 0;
        yield return waitTime;

        while (beamLight.intensity <= intensity)
        {
            beamLight.intensity += 0.1f;
            yield return null;
        }

        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);

        for (int i = 0; i < startFXList.Count; i++)
        {
            startFXList[i].Play();
        }
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Ghost/G_Beam.wav", Define.Sound.Effect, 1, 0.5f);

        while (lineLength <= length)
        {
            lineLength += 1f;
            beam.SetPosition(1, new Vector3(lineLength, 0, 0));
            yield return null;
        }

        IsReady = true;

        while(timerA<beamDuration)
        {
            if(timer >0.1f)
            {   

                RaycastHit2D[] attachBeam = Physics2D.BoxCastAll(transform.position, new Vector2(width, 1), 0 , beamPos.transform.position - transform.position , length, enemy);
                for (int i = 0; i < attachBeam.Length; i++)
                {
                    attachBeam[i].transform.GetComponent<IHittable>().OnDamage(3, 0);
                }
                timer = 0;
            }
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            yield return null;

        }

        while (lineWidth >= 0.0f)
        {
            lineWidth -= Time.deltaTime * 2f;
            beamLight.intensity -= 0.01f;
            beam.startWidth = lineWidth;
            beam.endWidth = lineWidth;

            yield return null;
        }


        beamLight.intensity = 0;
        beam.startWidth = 0;
        beam.endWidth = 0;
        IsReady = false;
        
    }
}


