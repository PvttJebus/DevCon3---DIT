using Cinemachine;
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
    public CinemachineVirtualCamera pinCam;

    public Rigidbody hips;

    public float percentage;

    private Rigidbody[] ragdollRB;
    private Collider[] ragdollCol;
    private Animator animator;

    //Variables for Handle Reset of player
    public Vector3 playerStartPOS;
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
        pinCam.m_Priority = 0;
        playerStartPOS = transform.position;
    }

    private void Update()
    {
        Vector3 currentrotation = kyleBody.gameObject.transform.eulerAngles;
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
            currentrotation.x += 85f;
            col.gameObject.transform.eulerAngles = currentrotation;
            kyleBody.gameObject.transform.eulerAngles = currentrotation;
            isGliding = true;
        }

        if (isGliding == true)
        {

            Gliding();
            if (Input.GetKey(KeyCode.Q) == true)
            {

                
                
                rb.gameObject.transform.RotateAround(rb.transform.position, Vector3.forward, 1 );
                col.gameObject.transform.RotateAround(rb.transform.position, Vector3.forward, 1 );
                kyleBody.gameObject.transform.RotateAround(rb.transform.position, Vector3.forward, 1);
            }
            if (Input.GetKey(KeyCode.E) == true)
            {
                
                rb.gameObject.transform.RotateAround(rb.transform.position, Vector3.back, 1 );
                col.gameObject.transform.RotateAround(rb.transform.position, Vector3.back, 1);
                kyleBody.gameObject.transform.RotateAround(rb.transform.position, Vector3.back, 1);
            }
        }


    }

    private void Gliding()
    {

        // Player rotation - X axis
        rotation.x += 20 * Input.GetAxis("Vertical") * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -45, 90);
        // Player rotation - Y axis
        rotation.y += 20 * Input.GetAxis("Horizontal") * Time.deltaTime;
        // Player rotation - Z axis
        rotation.z = -5 * Input.GetAxis("Horizontal");
        rotation.z = Mathf.Clamp(rotation.z, -5, 5);
        // Apply rotation
        transform.rotation = Quaternion.Euler(rotation);

        // Correct the pitchPercent calculation
        float pitchPercent = (rotation.x + 45f) / 135f;

        // Reverse the arguments in Mathf.Lerp
        float temp_speed = Mathf.Lerp(0f, 22f, pitchPercent);
        float temp_drag = Mathf.Lerp(9f, 7f, pitchPercent);

        rb.drag = temp_drag;

        Vector3 localvelocity = transform.InverseTransformDirection(rb.velocity);
        localvelocity.z = temp_speed * 4;
        localvelocity.z *= (1 - Time.deltaTime * temp_drag);
        rb.velocity = transform.TransformDirection(localvelocity);

    }


    private void OnTriggerEnter(Collider other)
    {
        //foreach (var rb in ragdollRB)
        //{
        //    rb.isKinematic = false;
        //    rb.useGravity = true;
        //}
        pinCam.m_Priority = 100;
        //isGliding = false;
    }

        private void EnableRagdoll()
    {


        foreach (var rb in ragdollRB)
        {
            if (rb.gameObject.CompareTag("Kyle"))
            {

                rb.isKinematic = true;
                rb.useGravity = false;

            }
            else
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }


        }

        
        //FixedJoint joint = hips.gameObject.AddComponent<FixedJoint>();
        //joint.connectedBody = rb; // 'rb' is the parent object's Rigidbody

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

    private void PlayerReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            tp.enabled = true;
            isGliding = false;
            DisableRagdoll();
            transform.position = playerStartPOS;
            pinCam.m_Priority = 0;

        }
    }

}