using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    public GameObject levelCompleteUI;  
    public Image[] stars;  
    public Sprite emptyStar;  
    public Sprite fullStar;  
    public Text timeText;  
    public Button mainMenuButton;  

    private float levelStartTime;

    void Start()
    {
        
        levelCompleteUI.SetActive(false);

        // time of the start of the level
        levelStartTime = Time.time;

        // button onclick listener.
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void CompleteLevel()
    {
        // calculation of time taken to finish (current time - start time)
        float timeTaken = Time.time - levelStartTime;
        timeText.text = "You took " + timeTaken.ToString("F2") + " seconds";

        
        levelCompleteUI.SetActive(true);

        
        UpdateStars(timeTaken);
    }

    void UpdateStars(float timeTaken)
    {
        // this calculates the amount of stars that will be given to the player for the time they took to complete the level.
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
        // sets the star to either a full star or an empty one based using a for loop.
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
        
        SceneManager.LoadScene("MainMenu");  // loads the main menu after you click the main menu button
    }
}
