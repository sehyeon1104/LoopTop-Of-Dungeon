using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenShatterExplosion : MonoBehaviour
{
    [SerializeField] Transform a;
    public class Shatters
    {
        public Transform shatterTransform;
        public Vector3 startPos;
        public Shatters(Transform _shatterTransform, Vector3 _startPos)
        {
            shatterTransform = _shatterTransform;
            startPos =  _startPos;
        }
    }
    [SerializeField]
    List<Rigidbody> childRigidbody;
    [SerializeField] List<Shatters> shatters = new List<Shatters>();

    public void Explo()
    {
        StartCoroutine(dx());
    }
    IEnumerator dx()
    {
        if(shatters.Count <=1)
        {

        foreach (Transform child in transform)
        {
            shatters.Add(new Shatters(child, child.localPosition));          
        }
        }
        for (int i=0; i<childRigidbody.Count; i++)
        {
            childRigidbody[i].isKinematic = false;
            childRigidbody[i].useGravity = true;
        }
        yield return new WaitUntil(() => childRigidbody[29].isKinematic == false);
        for (int i = 0; i < childRigidbody.Count; i++)
        {

        childRigidbody[i].AddExplosionForce(1000, a.position, 10f);
        }


    }
    private void OnDisable()
    {
        foreach (Shatters shatter in shatters)
        {
            shatter.shatterTransform.localPosition = shatter.startPos;
            shatter.shatterTransform.rotation = Quaternion.identity;
        }   
        for (int i = 0; i < childRigidbody.Count; i++)
        {
            childRigidbody[i].useGravity = false;
            //childRigidbody[i].velocity = Vector3.zero;
            childRigidbody[i].isKinematic = true;
        }
    }
}
