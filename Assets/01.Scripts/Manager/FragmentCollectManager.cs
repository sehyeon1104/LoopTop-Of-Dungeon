using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Managers.Pool.CreatePool(fragmentCollect, 100);
    }

    public void AddFragment(GameObject obj, int count = 3)
    {
        for(int i = 0; i < count; ++i)
        {
            fragmentObj = Managers.Pool.Pop(fragmentCollect);
            fragmentObj.transform.position = obj.transform.position;
        }

    }

}
