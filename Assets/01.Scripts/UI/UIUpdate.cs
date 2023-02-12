using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    [SerializeField]
    GameObject hpPrefab;    
    private void Start()
    { 
        HpUpdate();
    }
    public void HpUpdate()
    {
        Transform[] hpbars = GetComponentsInChildren<RectTransform>();
       for(int i=1; i<hpbars.Length; i++)
        {
            Destroy(hpbars[i].gameObject);
        }
        for (int i = 0; i < Player.Instance.Hp; i++)
        {
            Instantiate(hpPrefab, new Vector3(255+105 * i, 1030 ,0), Quaternion.identity,transform/*, transform.Find("HPbar").transform*/);
        }

    }
  
}
