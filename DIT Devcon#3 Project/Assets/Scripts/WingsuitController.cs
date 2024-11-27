using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif

public class WingsuitController : MonoBehaviour
{

   

    public float speed = 22f;
    public float drag = 8f;

    public Rigidbody rb;
    public ThirdPersonController tp;
    public GameObject kyleBody;
    private Vector3 rotation;
    private bool isGliding;

    public float percentage;

    private void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        rotation = transform.eulerAngles;
        tp = rb.GetComponent<ThirdPersonController>();
        isGliding = false;

    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space) == true && isGliding == true)
        //{
        //    //tp.enabled = true;
        //    Vector3 currentrotation = kyleBody.gameObject.transform.eulerAngles;
        //    currentrotation.x -= 85f;
        //    kyleBody.gameObject.transform.eulerAngles = currentrotation;
        //    isGliding = false;
        //}
        if (Input.GetKeyUp(KeyCode.Space) == true && isGliding == false)
        {
            tp.enabled = false;
            Vector3 currentrotation = kyleBody.gameObject.transform.eulerAngles;
            currentrotation.x += 85f;
            kyleBody.gameObject.transform.eulerAngles = currentrotation;
            isGliding = true;
            
            rb.useGravity = true;
        }

        if (isGliding == true)
        {

            Gliding();
        }

        else
        {
            Move();
        }
    }

    private void Gliding()
    {
        //player rotation - X axis
        rotation.x += 20 * Input.GetAxis("Vertical") * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -45, 45);
        //player rotation - Y Axis
        rotation.y += 20 * Input.GetAxis("Horizontal") * Time.deltaTime;


        //player rotation - Z axis
        rotation.z = -5 * Input.GetAxis("Horizontal");
        rotation.z = Mathf.Clamp(rotation.z, -5, 5);
        //Transform
        transform.rotation = Quaternion.Euler(rotation);

        percentage = rotation.x / 45;
        //Drag: Fast = 4, slow = 6 (i guess the float?)
        float temp_drag = (percentage * -2) + 8;
        //Speed: fast = 13.8, Slow (12.5)
        float temp_speed = percentage * (22f - 19f) + 19f;

        rb.drag = temp_drag;
        Vector3 localvelocity = transform.InverseTransformDirection(rb.velocity);
        localvelocity.z = temp_speed;
        localvelocity = localvelocity * (1 - Time.deltaTime * temp_drag);
        rb.velocity = transform.TransformDirection(localvelocity);
    }

    private void Move()
    {
     
    }
}

