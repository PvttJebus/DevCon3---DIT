using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
public class WingsuitController : MonoBehaviour
{



    public float speed = 22f;
    public float drag = 8f;

    public Rigidbody rb;
    public Collider col;
    public GameObject kyleBody;
    private Vector3 rotation;
    private bool isGliding;
    private bool pressStart;
    public CinemachineVirtualCamera pinCam;

    public float percentage;
    public Vector3 currentRotation;



    //Variables for Handle Reset of player
    public Vector3 playerStartPOS;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
       rb.isKinematic = true;
        rotation = transform.eulerAngles;
      
        isGliding = false;
        
        playerStartPOS = transform.position;
        currentRotation = kyleBody.gameObject.transform.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && pressStart == false)
        {
            pinCam.m_Priority = 0;
            pressStart = true;
        }

        if (pressStart == true)
        {

            
            if (Input.GetKeyUp(KeyCode.Space) == true && isGliding == false)
            {

                rb.isKinematic = false;
                rb.gameObject.transform.eulerAngles = currentRotation;
                col.gameObject.transform.eulerAngles = currentRotation;
                kyleBody.gameObject.transform.eulerAngles = currentRotation;
                isGliding = true;
            }

            if (isGliding == true)
            {

                Gliding();
                if (Input.GetKey(KeyCode.Q) == true)
                {



                    rb.gameObject.transform.RotateAround(rb.transform.position, Vector3.forward, 1);
                    col.gameObject.transform.RotateAround(rb.transform.position, Vector3.forward, 1);
                    kyleBody.gameObject.transform.RotateAround(rb.transform.position, Vector3.forward, 1);
                }
                if (Input.GetKey(KeyCode.E) == true)
                {

                    rb.gameObject.transform.RotateAround(rb.transform.position, Vector3.back, 1);
                    col.gameObject.transform.RotateAround(rb.transform.position, Vector3.back, 1);
                    kyleBody.gameObject.transform.RotateAround(rb.transform.position, Vector3.back, 1);
                }
            }

        }
        PlayerReset();
        EndGame();

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
        
        pinCam.m_Priority = 100;
        
    }

       

    private void PlayerReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

          
            isGliding = false;
           
            transform.position = playerStartPOS;

            pinCam.m_Priority = 0;

        }
    }

    private void EndGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}