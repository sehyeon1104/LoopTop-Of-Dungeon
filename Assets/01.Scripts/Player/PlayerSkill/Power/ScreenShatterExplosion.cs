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
            childRigidbody[i].AddExplosionForce(800, a.position, 10f);
            //childRigidbody[i].useGravity = true;
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
            //childRigidbody[i].useGravity = false;
            //childRigidbody[i].velocity = Vector3.zero;
            childRigidbody[i].isKinematic = true;
        }
    }
}
