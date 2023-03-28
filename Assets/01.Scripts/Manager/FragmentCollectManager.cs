using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FragmentCollectManager : MonoSingleton<FragmentCollectManager>
{
    private GameObject fragmentCollect;
    private Poolable fragmentObj;

    private void Awake()
    {
        fragmentCollect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Fragment.prefab");
    }

    private void Start()
    {
        Managers.Pool.CreatePool(fragmentCollect, 10);
    }

    public void AddFragment(GameObject obj)
    {
        Debug.Log("AddFragment");
        for(int i = 0; i < 3; ++i)
        {
            fragmentObj = Managers.Pool.Pop(fragmentCollect);
            fragmentObj.transform.position = obj.transform.position;
        }

    }

}
