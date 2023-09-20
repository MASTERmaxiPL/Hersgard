using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking.Types;
using System.Linq;
using System.IO.Enumeration;

public class DataToFile
{
    private string dataDirPath = "";
    private string dataFileName = "SavedGame";
    private string saveFolderName = "saveFolder";
    private bool useEncryption = false;
    private readonly string encryptionWord = "nfjhszbdvji";
    private int saveIndex;
    public DataToFile(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        saveIndex = PlayerPrefs.GetInt("saveIndex") - 1;
        string fullPath = Path.Combine(dataDirPath, saveFolderName, dataFileName);
        Debug.Log(fullPath);

        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.Log("Failed Loading File" + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }
    public void Save(GameData data, int saveLimit)
    {
        saveIndex = PlayerPrefs.GetInt("saveIndex");
        PlayerPrefs.SetInt("saveIndex", saveIndex + 1);

        dataFileName = DataManager.GetInstance().GetFileName();

        string fullPath = Path.Combine(dataDirPath, saveFolderName, dataFileName + saveIndex.ToString() + ".json");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Failed Saving File" + fullPath + "\n" + e);
        }

        FileInfo[] files = GetSaves();
        if(files.Length > saveLimit)
        {
            string path = Path.Combine(dataDirPath, saveFolderName);
            foreach (var file in new DirectoryInfo(path).GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(saveLimit))
                file.Delete();
        }
    }

    public FileInfo[] GetSaves() 
    {
        string path = Path.Combine(dataDirPath, saveFolderName);   
        if (Directory.Exists(path))
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles("*.json").OrderByDescending(x => x.CreationTime).ToArray();
            foreach(FileInfo file in files)
            {
                return files;
            }
        }
        return null;
    }

    private string EncryptDecrypt(string data)
    {
        string modiefiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modiefiedData += (char)(data[i] ^ encryptionWord[i % encryptionWord.Length]);
        }

        return modiefiedData;
    }
}
