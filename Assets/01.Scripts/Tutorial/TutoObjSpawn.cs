using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoObjSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform dummySpawnPos = null;
    [SerializeField]
    private Transform statuePos = null;
    [SerializeField]
    private Transform dropItemPos = null;

    private GameObject dummy = null;
    private GameObject statue = null;
    private GameObject dropItem = null;

    public void InstantiateAllObj()
    {
        InstantiateDummy();
        InstantiateStatue();
        InstantiateDropItem();
    }

    public void InstantiateDummy()
    {

    }

    public void InstantiateStatue()
    {

    }

    public void InstantiateDropItem()
    {

    }
}
