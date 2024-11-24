using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsuitController : MonoBehaviour
{
    public float speed = 12.5f;
    public float drag = 6f;

    public Rigidbody rb;
    public ThirdPersonController tp;
    private Vector3 rotation;

    public float percentage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotation = transform.eulerAngles;
        tp = rb.GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) == true)
        {
            tp.enabled = false;
            //Vector3 currentrotation = rb.gameObject.transform.eulerAngles;
            //currentrotation.x += 85f;
            //rb.gameObject.transform.eulerAngles = currentrotation;
        }

        //player rotation - X axis
        rotation.x += 20 * Input.GetAxis("Vertical") * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, 0, 45);
        //player rotation - Y Axis
        rotation.y += 20 * Input.GetAxis("Horizontal") * Time.deltaTime;


        //player rotation - Z axis
        rotation.z = -5 * Input.GetAxis("Horizontal");
        rotation.z = Mathf.Clamp(rotation.z, -5, 5);
        //Transform
        transform.rotation = Quaternion.Euler(rotation);

        percentage = rotation.x / 45;
        //Drag: Fast = 4, slow = 6 (i guess the float?)
        float temp_drag = (percentage * -2) + 6;
        //Speed: fast = 13.8, Slow (12.5)
        float temp_speed = percentage * (13.8f - 12.5f) + 12.5f;

        rb.drag = temp_drag;
        Vector3 localveclocity = transform.InverseTransformDirection(rb.velocity);
        localveclocity.z = temp_speed;
        rb.velocity = transform.TransformDirection(localveclocity);
    }
}
