using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    public GameObject levelCompleteUI;  // Assign the Canvas/UI that will show the level complete screen.
    public Image[] stars;  // Array to hold the star images.
    public Sprite emptyStar;  // Assign the empty star sprite in Inspector.
    public Sprite fullStar;  // Assign the full star sprite in Inspector.
    public Text timeText;  // Reference to the text displaying the player's time.
    public Button mainMenuButton;  // Reference to the Main Menu button.

    private float levelStartTime;

    void Start()
    {
        // Initialize the UI to be hidden
        levelCompleteUI.SetActive(false);

        // Capture the time the level started
        levelStartTime = Time.time;

        // Assign the button's onClick listener
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void CompleteLevel()
    {
        // Calculate the time it took the player to finish
        float timeTaken = Time.time - levelStartTime;
        timeText.text = "You took " + timeTaken.ToString("F2") + " seconds";

        // Show the UI
        levelCompleteUI.SetActive(true);

        // Update stars based on time taken
        UpdateStars(timeTaken);
    }

    void UpdateStars(float timeTaken)
    {
        // Here we assume thresholds for 3 stars, 2 stars, and 1 star
        if (timeTaken <= 20)
        {
            SetStars(3);
        }
        else if (timeTaken <= 30)
        {
            SetStars(2);
        }
        else if (timeTaken <= 40)
        {
            SetStars(1);
        }
        else
        {
            SetStars(0);
        }
    }

    void SetStars(int starCount)
    {
        // Loop through the stars and set full or empty stars
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < starCount)
            {
                stars[i].sprite = fullStar;
            }
            else
            {
                stars[i].sprite = emptyStar;
            }
        }
    }

    void GoToMainMenu()
    {
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu");  // Ensure your Main Menu scene is named correctly.
    }
}
