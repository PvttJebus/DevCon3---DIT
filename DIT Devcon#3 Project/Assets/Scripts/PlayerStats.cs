using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI altitudeText;
    public TextMeshProUGUI speedText;

    private Rigidbody kyleRigidbody;

    void Start()
    {
        kyleRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float altitude = transform.position.y;
        altitudeText.text = "Altitude: " + altitude.ToString("F2") + " meters";

        float speed = kyleRigidbody.velocity.magnitude;
        speedText.text = "Speed: " + speed.ToString("F2") + " m/s";
    }
}