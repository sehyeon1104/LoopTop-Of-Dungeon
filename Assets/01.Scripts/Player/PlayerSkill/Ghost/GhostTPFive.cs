using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostTPFive : MonoBehaviour
{
    [SerializeField] private GameObject[] subTps;
    [SerializeField] private GameObject[] tpEffects;
    [SerializeField] private GameObject mainTp;

    private void OnEnable()
    {
        StartCoroutine(GhostTp());
    }

    private IEnumerator GhostTp()
    {
        yield return new WaitForSeconds(0.5f);

        Transform pTransform = GameManager.Instance.Player.transform;
        for(int i = 0; i < subTps.Length; i += 2)
        {
            subTps[i].SetActive(true);
            subTps[i + 1].SetActive(true);

            pTransform.position = subTps[i].transform.position;
            pTransform.DOMove(subTps[i + 1].transform.position, 0.2f);
            tpEffects[i/2].SetActive(true);

            yield return new WaitForSeconds(0.25f);

            subTps[i].SetActive(false);
            subTps[i + 1].SetActive(false);
        }

        yield return new WaitForSeconds(0.35f);
        pTransform.position = mainTp.transform.position;
        mainTp.SetActive(true);
        pTransform.GetComponent<Rigidbody2D>().DOMove(pTransform.position + Vector3.right * 15f, 0.3f);

        yield return new WaitForSeconds(0.4f);
        Time.timeScale = 1f;

    }
}
