using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleFire : MonoBehaviour
{
    public float damage = 10;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.GetComponent<Health>())
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
