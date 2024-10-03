using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private string nextLevelName;

    [SerializeField] private GameObject endGame;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject startPosition;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButoon;

    private GameObject currentLevelInstance;

    private void Start()
    {
        nextLevelButton.onClick.AddListener(OnPLayerWin);
    }

    private void OnInit()
    {
        player.transform.position = startPosition.transform.position;
    }

    public void WinGame()
    {
        Invoke(nameof(ShowDisplayerEndMenu), 2f);
    }

    private void ShowDisplayerEndMenu()
    {
        endGame.SetActive(true);
    }

    private void OnPLayerWin()
    {
        if(currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        LoadNextLevel(nextLevelName);
    }

    private void LoadNextLevel(string _levelPrefabName)
    {
        endGame.SetActive(false);

        GameObject levelPrefab = Resources.Load<GameObject>(_levelPrefabName);
    
        if(_levelPrefabName != null)
        {
            currentLevelInstance = Instantiate(levelPrefab);
            OnInit();
            Debug.Log(_levelPrefabName + " loaded successfully");
        }
        else
        {
            Debug.Log("Level load unsucessfully");
        }
    }
}
