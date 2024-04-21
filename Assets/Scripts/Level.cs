using UnityEngine;

// This class simply update the counter of the level manager, it's used as a reference for each enemies in the current level
public class Level : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {

        if (levelManager) return;

        // With this we can use the Level-index as a prefab if we don't want to keep it in the scene
        FindLevelManager();
    }

    public void EnemyKilled ()
    {
        levelManager.EnemyKilled();
    }

    private void FindLevelManager ()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
}
