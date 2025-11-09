using System.IO;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    const string saveFileName = "saveFile.whisper";
    string saveFilePath;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		saveFilePath = Application.persistentDataPath + "/" + saveFileName;
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGameData(SaveData saveData)
    {
		string jsonData = JsonUtility.ToJson(saveData, true);

		using (StreamWriter writer = new StreamWriter(saveFilePath))
		{
			writer.Write(jsonData);
		}

	}

    public void ReadSaveFile()
    {
		string json = "";
		try
		{
			using (StreamReader reader = new StreamReader(saveFilePath))
			{
				json = reader.ReadToEnd();
			}

			print(json);
		}
		catch (System.Exception exception)
		{
			Debug.LogError($"[DATA LOAD FAILED] Couldn't read file at {saveFilePath} | Error: {exception.Message}");
			return;
		}

		SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

		//StreamReader reader = new StreamReader(saveFilePath);
		//      string line = reader.ReadLine();
		//      reader.Close();
	}
}
