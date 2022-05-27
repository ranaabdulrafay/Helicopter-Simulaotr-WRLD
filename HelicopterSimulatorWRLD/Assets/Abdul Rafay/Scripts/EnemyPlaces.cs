using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Space;
using System.IO;
[CreateAssetMenu(fileName = "Enemy Points",menuName ="Abdul Rafay/Create Ai Places")]
public class EnemyPlaces : ScriptableObject
{
    public List<Vector2> pointsVctr = new List<Vector2>();
    public List<Vector2> PathpointsVctr = new List<Vector2>();

    private string FILENAME
    {
        get
        {
            return FileName + FileType;
        }
    }
    private const string FileName = "EquipedGears";
    private const string FileType = ".dat";

    public void SaveToFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, FILENAME);
        Debug.Log("Save file EquipedGears" + Application.persistentDataPath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        var json = JsonUtility.ToJson(this);
        File.WriteAllText(filePath, json);
    }


    public void LoadDataFromFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, FILENAME);
        Debug.Log("Load file EquipedGears" + Application.persistentDataPath);
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File \"{filePath}\" not found! Using Pre Set Asset", this);
            return;
        }

        var json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
