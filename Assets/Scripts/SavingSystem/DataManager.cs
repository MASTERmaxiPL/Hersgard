using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class DataManager : MonoBehaviour
{
    #region singleton
    private static DataManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        this.dataHandler = new DataToFile(Application.persistentDataPath, fileName, useEncryption);
    }
    public static DataManager GetInstance()
    {
        return instance;
    }
    #endregion

    [Header("Debugg")]
    [SerializeField] private bool initalizeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private GameData gameData;
    private DataToFile dataHandler;
    private List<IData> dataObjects;

    private int saveLimit = 50;

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void SaveGame()
    {
        if(gameData == null)
        {
            Debug.Log("There is no data to save");
        }
        else
        {
            this.dataObjects = FindAllDataObjects();
            foreach (IData data in dataObjects)
            {
                data.SaveData(gameData);
            }
            dataHandler.Save(gameData, saveLimit);
        }
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if(this.gameData == null && initalizeDataIfNull)
        {
            NewGame();
        }

        if(this.gameData == null)
        {
            NewGame();
        }

        foreach(IData data in dataObjects)
        {
            Debug.Log(gameData.xPosition);
            data.LoadData(gameData);
        }
    }

    public void OnSceneLoaded()
    {
        Debug.Log("OnSceneLoaded");
        this.dataObjects = FindAllDataObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IData> FindAllDataObjects()
    {
        IEnumerable<IData> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();
        return new List<IData>(dataObjects); 
    }

    public bool CheckIfDataSaveExists()
    {
        if(dataHandler.GetSaves() == null)
        {
            return false;
        }
        return true;
    }

    public FileInfo[] LoadExistingSaves()
    {
        
        return dataHandler.GetSaves();
    }

    public void setDataHandler(string fileName)
    {
        this.dataHandler = new DataToFile(Application.persistentDataPath, fileName, useEncryption);
    }

    public string GetFileName()
    {
        return fileName;
    }

    public void SetSaveLimit(int num)
    {
        saveLimit = num;
    }
}
