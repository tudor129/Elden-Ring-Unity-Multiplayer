using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    // BEFORE WE CREATE A NEW SAVE FILE, WE NEED TO CHECK IF ONE ALREADY EXISTS(MAX 10 CHARACTER SAVE FILES)
    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // USED TO DELETE CHARACTER SAVE FILES
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }
    // USED TO CREATE A NEW CHARACTER UPON STARTING A NEW GAME
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        // MAKE A PATH TO SAVE THE FILE (A LOCATION ON THE MACHINE)
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // CREATE THE DIR THE FILE WILL BE WRITTEN TO, IF IT DOESNT ALREADY EXIST
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("CREATING SAVE FILE, AT PATH: " + savePath);
            
            // SERIALIZE THE C# GAME DATA OBJECT TO A JSON

            string dataToStore = JsonUtility.ToJson(characterData, true);
            
            // WRITE THAT FILE TO OUR SYSTEM
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + e);
        }
    }
    // USED TO LOAD A SAVE FILE UPON LOADING A PREVIOUS GAME
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        // MAKE A PATH TO LOAD THE FILE (A LOCATION ON THE MACHINE)
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try 
            { 
                string dataToLoad = "";

                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // DESERIALIZE THE DATA FROM JSON BACK TO UNITY
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad); 
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR WHILST TRYING TO LOAD CHARACTER DATA, GAME NOT LOADED" + loadPath + "\n" + e);
            }
        }
        return characterData;
    }
    
}
