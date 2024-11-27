using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
public class WingsuitController : MonoBehaviour
{



    public float speed = 22f;
    public float drag = 8f;

    public Rigidbody rb;
    public Collider col;
    public ThirdPersonController tp;
    public GameObject kyleBody;
    private Vector3 rotation;
    private bool isGliding;

    public float percentage;

    private Rigidbody[] ragdollRB;
    private Collider[] ragdollCol;
    private Animator animator;

    private void Awake()
    {
        ragdollRB = GetComponentsInChildren<Rigidbody>();
        ragdollCol = GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();

        DisableRagdoll();
    }

    private void Start()
    {


        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rotation = transform.eulerAngles;
        tp = rb.GetComponent<ThirdPersonController>();
        isGliding = false;
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space) == true && isGliding == true)
        //{

        //    Vector3 currentrotation = kyleBody.gameObject.transform.eulerAngles;
        //    currentrotation.x -= 85f;
        //    kyleBody.gameObject.transform.eulerAngles = currentrotation;
        //    isGliding = false;
        //}
        if (Input.GetKeyUp(KeyCode.Space) == true && isGliding == false)
        {
            animator.enabled = false;
            EnableRagdoll();
            tp.enabled = false;
            Vector3 currentrotation = kyleBody.gameObject.transform.eulerAngles;
            currentrotation.x += 85f;
            col.transform.eulerAngles = currentrotation;

            kyleBody.gameObject.transform.eulerAngles = currentrotation;
            isGliding = true;
        }

        if (isGliding == true)
        {

            Gliding();
        }
    }

    private void Gliding()
    {
        //player rotation - X axis
        rotation.x += 20 * Input.GetAxis("Vertical") * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -45, 90);
        //player rotation - Y Axis
        rotation.y += 20 * Input.GetAxis("Horizontal") * Time.deltaTime;


        //player rotation - Z axis
        rotation.z = -5 * Input.GetAxis("Horizontal");
        rotation.z = Mathf.Clamp(rotation.z, -5, 5);
        //Transform
        transform.rotation = Quaternion.Euler(rotation);


        // This is a tossed together solution from a few sources, which ideally will make the player lose speed if going up and gain if going down. 
        float pitchPercent = (rotation.x + 45f) / 135f;

        float temp_speed = Mathf.Lerp(-5, 22, pitchPercent);
        float temp_drag = Mathf.Lerp(9f, 7f, pitchPercent);

        rb.drag = temp_drag;

        Quaternion yawRotation = Quaternion.Euler(0, rotation.y, 0);
        Vector3 movementDirection = yawRotation * Vector3.forward;

        Vector3 force = movementDirection * temp_speed;
        force *= (1 - Time.deltaTime * temp_drag);

        rb.AddForce(force, ForceMode.Acceleration);



    }


    private void EnableRagdoll()
    {
        foreach (var rb in ragdollRB)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        foreach (var col in ragdollCol)
        {
            col.enabled = true;
        }

    }
    private void DisableRagdoll()
    {

        foreach (var rb in ragdollRB)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        foreach (var col in ragdollCol)
        {
            if (col.gameObject != gameObject)
            {
                col.enabled = false;
            }
        }
    }

}