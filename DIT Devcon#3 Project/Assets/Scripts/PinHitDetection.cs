using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinHitDetection : MonoBehaviour
{
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin") == true || collision.gameObject.CompareTag("Kyle") == true)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
