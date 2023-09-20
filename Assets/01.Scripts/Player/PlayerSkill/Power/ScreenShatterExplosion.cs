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
        foreach (Transform child in transform)
        {
            shatters.Add(new Shatters(child, child.localPosition));          
        }
        for (int i=0; i<childRigidbody.Count; i++)
        {
            childRigidbody[i].AddExplosionForce(1000, a.position, 10f);
            childRigidbody[i].useGravity = true;
        }

    }
    private void OnDisable()
    {
        for (int i = 0; i < childRigidbody.Count; i++)
        {
            childRigidbody[i].useGravity = false;
        }
        foreach (Shatters shatter in shatters)
        {
            shatter.shatterTransform.localPosition = shatter.startPos;
        }   
    }
}
