using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private List<GameObject> levelEnvironment;
    [SerializeField] private List<int> maxEnemies;
    [SerializeField] private List<int> currentEnemiesDead;
    [SerializeField] private List<Door> door;
    [SerializeField] private int indexLevel;

    [Header("UI")]
    [SerializeField] private TMP_Text tmpCurrentLevel;
    [SerializeField] private TMP_Text tmpCurrentEnemiesDead;

    // Start is called before the first frame update
    void Start()
    {
        indexLevel = 0;
        levelEnvironment[0].SetActive(true);

        // UI
        UpdateUI();
    }

    private void UpdateUI ()
    {
        int currentLevelFixDisplayValue = indexLevel + 1;
        tmpCurrentLevel.text = "Level " + currentLevelFixDisplayValue;
        tmpCurrentEnemiesDead.text = currentEnemiesDead[indexLevel] + " - " + maxEnemies[indexLevel];
    }

    public void EnemyKilled ()
    {
        currentEnemiesDead[indexLevel]++;
        UpdateUI();
        // Check if the player can go to the next level or it has a win condition
        if (currentEnemiesDead[indexLevel] >= maxEnemies[indexLevel])
        {
            // if this is the last level
            if (indexLevel == maxEnemies.Count - 1 )
            {
                WinGame();
            } else
            {
                NextLevel();
            }
        }
    }

    public void NextLevel ()
    {
        door[indexLevel].ToggleAnimationDoor(true);
        indexLevel++;
        currentEnemiesDead[indexLevel] = 0;             // Be sure that the next level have the current counter set to 0
        levelEnvironment[indexLevel].SetActive(true);
        UpdateUI();
    }

    public void WinGame()
    {
        door[indexLevel].ToggleAnimationDoor(true);
        tmpCurrentEnemiesDead.text = "YOU WON";
    }

    public void ReloadCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
