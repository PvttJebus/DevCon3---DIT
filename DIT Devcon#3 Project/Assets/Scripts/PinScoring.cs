using UnityEngine;
using TMPro;

public class PinScoring : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private bool hasFallen = false;
    private static int fallenPinCount = 0;
    private int attempts = 1; // Start at the first attempt
    public GameObject[] pins;

    // Arrays to store initial positions and rotations
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;

    void Start()
    {
        // Initialize the arrays
        initialPositions = new Vector3[pins.Length];
        initialRotations = new Quaternion[pins.Length];

        // Store initial positions and rotations
        for (int i = 0; i < pins.Length; i++)
        {
            initialPositions[i] = pins[i].transform.position;
            initialRotations[i] = pins[i].transform.rotation;
        }

        // Display the initial message for the first attempt
        scoreText.text = "First attempt! Hit the pins.";
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasFallen && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Pin")))
        {
            hasFallen = true;
            fallenPinCount++;
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            if (attempts == 1)
            {
                scoreText.text = "You knocked down " + fallenPinCount + " pins! Press R for next attempt.";
            }
            else if (attempts == 2)
            {
                scoreText.text = "Your total score is: " + fallenPinCount + " pins! Press R to try again.";
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (attempts == 1)
            {
                ResetPins();
                attempts++;
                hasFallen = false; // Reset the hasFallen flag for the next attempt
                scoreText.text = "Second attempt! Hit the pins.";
            }
            else if (attempts == 2)
            {
                scoreText.text = "Your total score is: " + fallenPinCount + " pins! Press R to try again.";
                ResetGame();
            }
        }
    }

    void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].transform.position = initialPositions[i];
            pins[i].transform.rotation = initialRotations[i];
            Rigidbody rb = pins[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Enable constraints to stabilize pins
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    void ResetGame()
    {
        // Reset the game state
        attempts = 1; // Reset to 1 to start the first attempt again
        fallenPinCount = 0; // Reset fallenPinCount for new game
        scoreText.text = "First attempt started! Hit the pins.";
        ResetPins();
    }
}
