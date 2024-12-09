using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
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

    //bools for player inputs in fixed update
    private float verticalInput;
    private float horizontalInput;
    private bool qKey;
    private bool eKey;
    private bool spaceKey;
    private bool spaceKeyUp;
    private bool rKeyDown;
    private bool escapeKey;

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
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        qKey = Input.GetKey(KeyCode.Q);
        eKey = Input.GetKey(KeyCode.E);
        spaceKey = Input.GetKey(KeyCode.Space);
        spaceKeyUp = Input.GetKeyUp(KeyCode.Space);
        rKeyDown = Input.GetKeyDown(KeyCode.R);
        escapeKey = Input.GetKey(KeyCode.Escape);

        if (spaceKey && pressStart == false)
        {
            pinCam.m_Priority = 0;
            pressStart = true;
        }

        if (pressStart)
        {
            if (spaceKeyUp && isGliding == false)
            {
                rb.isKinematic = false;
                rb.transform.eulerAngles = currentRotation;
                col.transform.eulerAngles = currentRotation;
                kyleBody.transform.eulerAngles = currentRotation;
                isGliding = true;
            }


           
        }
        PlayerReset();
        EndGame();

        //Debug.DrawLine(rb.transform.position, rb.transform.position + rb.transform.forward * 5f, Color.red);

    }

    private void FixedUpdate()
    {
        if (isGliding == true)
        {
            Gliding();

        }
    }

    private void Gliding()
    {

        // Player rotation - X axis
        rotation.x += 20f * verticalInput * Time.fixedDeltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -45, 90);
        // Player rotation - Y axis
        rotation.y += 20f * horizontalInput * Time.fixedDeltaTime;


        


        if (qKey)
        {
            rotation.z -= 60f * Time.fixedDeltaTime;
        }
        if (eKey)
        {
            rotation.z += 60f * Time.fixedDeltaTime;
        }

        // this determines the angle of the player against the y axis
        float facingSimilarity = Vector3.Dot(Vector3.down, rb.transform.forward);
        transform.rotation = Quaternion.Euler(rotation);
        // this determines what the current X rotation pitch is
        float pitchPercent = (rotation.x + 45f) / 135f;
        //  These determine speed and drag based on the difference between pitch percentage
        float temp_speed = Mathf.Lerp(0, 22f, pitchPercent);
        float temp_drag = Mathf.Lerp(12f, 7f, pitchPercent);
       
        rb.drag = temp_drag;
        



        Vector3 localvelocity = transform.InverseTransformDirection(rb.velocity);
        localvelocity.z = temp_speed * 5f;
        localvelocity.z *= (1f - Time.fixedDeltaTime * temp_drag);
        rb.velocity = transform.TransformDirection(localvelocity);

        //this uses the dot vector return to slow speed and create new gravity to simulate stopping flight
        if (facingSimilarity <= -0.1f)
        {
            //rb.useGravity = false;
            localvelocity.z *= (1f - Time.fixedDeltaTime * temp_drag) - 11f;
            //rb.AddForce(new Vector3(0, -20, 0));
            //rb.AddForce(Physics.gravity * 5f);
            Physics.gravity = new Vector3(0, -90.0f, 0);
        }
       else
        {
            rb.useGravity = true;

        }

        //Debug.Log(temp_speed);
        //Debug.Log(pitchPercent);
        //Debug.Log(facingSimilarity);
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