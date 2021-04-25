using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private GameSaveData GameSave;

    private const string FileSaveKey = "FileSaveData";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }


    }

    private void Start()
    {
        if (string.IsNullOrEmpty(GameManager.Instance.GameSaveName)) return;

        if (!PlayerPrefs.HasKey(GameManager.Instance.GameSaveName)) return;

        string jsonString = PlayerPrefs.GetString(GameManager.Instance.GameSaveName);
        GameSave = JsonUtility.FromJson<GameSaveData>(jsonString);

        //LoadGame();
    }

    public void SaveGame()
    {
        // ??= new GameSaveData();

        var savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();


        ISavable playerSaveData = savableObjects.First(monoObject => monoObject is PlayerController);
        GameSave.PlayerSaveData = playerSaveData?.SaveData() as PlayerSaveData;

        string saveDataString = JsonUtility.ToJson(GameSave);
        PlayerPrefs.SetString(GameManager.Instance.GameSaveName, saveDataString);

        SaveToFileList();

        Debug.Log("Working");
    }


    public void LoadGame()
    {
        var savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();

        ISavable playerSaveObj = savableObjects.First(monoObject => monoObject is PlayerController);
        playerSaveObj?.LoadData(GameSave.PlayerSaveData);

       
    }


    public void SaveToFileList()
    {
        if (PlayerPrefs.HasKey(FileSaveKey))
        {
            GameDataList dataList = JsonUtility.FromJson<GameDataList>(PlayerPrefs.GetString(FileSaveKey));

            if (dataList.SaveFileNames.Contains(GameManager.Instance.GameSaveName)) return;

            dataList.SaveFileNames.Add(GameManager.Instance.GameSaveName);

            PlayerPrefs.SetString(FileSaveKey, JsonUtility.ToJson(dataList));
        }
        else
        {
            GameDataList data = new GameDataList();
            data.SaveFileNames.Add(GameManager.Instance.GameSaveName);
            PlayerPrefs.SetString(FileSaveKey, JsonUtility.ToJson(data));
        }
    }

}

[Serializable]
public class GameSaveData
{
    public PlayerSaveData PlayerSaveData;
    public GameSaveData()
    {
        PlayerSaveData = new PlayerSaveData();
    }
}