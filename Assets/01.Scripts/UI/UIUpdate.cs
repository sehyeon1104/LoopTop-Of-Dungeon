using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    [SerializeField]
    GameObject hpPrefab;    
    private void Start()
    { 
        HpUpdate(3);
    }
    public void HpUpdate(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            Instantiate(hpPrefab, new Vector3(255+105 * i, 1030 ,0), Quaternion.identity,transform/*, transform.Find("HPbar").transform*/);
        }

    }
  
}
