using UnityEngine;
using TMPro;

public class PinScoring : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    private bool hasFallen = false;
    private static int fallenPinCount = 0; 
    private static int totalPins = 10; 

    void Start()
    {
        fallenPinCount = 0;

        if (scoreText != null)
        {
            scoreText.text = "";
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasFallen && (collision.gameObject.CompareTag("Kyle") || collision.gameObject.CompareTag("Pin")))
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
            if (fallenPinCount == totalPins)
            {
                scoreText.text = "Strike!";
            }
            else if (fallenPinCount > 0) 
            {
                scoreText.text = "You knocked down " + fallenPinCount + " pins!";
            }
        }
    }
}
