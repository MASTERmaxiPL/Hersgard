using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerBodyManager bodyManager;

    public static GameManager instance;
    public Player player;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bodyManager = PlayerBodyManager.GetInstance();
        if (PlayerPrefs.GetString("NewGame") == "True" )
        {
            PlayerPrefs.SetString("NewGame", "False");
            LoadNewCharacterVisuals();
            DataManager.GetInstance().NewGame();
            DataManager.GetInstance().SaveGame();
        }
        DataManager.GetInstance().OnSceneLoaded();
    }

    private void LoadNewCharacterVisuals()
    {
        bodyManager.LoadNewPlayerBodyParts();
    }

}