using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Manager
{
    public List<FlagStructure> playerTrueFlags;
    public List<FlagStructure> playerFalseFlags;
    // Start is called before the first frame update
    void Awake()
    {
        // Sostituisci "NomeDellaCartella" con il percorso relativo della cartella contenente gli oggetti desiderati (senza l'estensione del file)
        string folderPath = "SO/Flag";

        // Carica tutti gli oggetti di tipo MioScriptableObject dalla cartella "Resources"
        FlagsSO[] oggettiCaricati = Resources.LoadAll<FlagsSO>(folderPath);

        // Ora puoi accedere agli oggetti caricati e fare ciò che desideri
        foreach (FlagsSO oggetto in oggettiCaricati)
        {
            FlagStructure flagToAdd = new();
            flagToAdd.flag = oggetto;
            flagToAdd.flagName = oggetto.nomeFlag;
            flagToAdd.actualValue = oggetto.valoreBaseFlag;
            if (flagToAdd.actualValue)
            {
                playerTrueFlags.Add(flagToAdd);
            }
            else
            {
                playerFalseFlags.Add(flagToAdd);
            }
        }
    }

    public bool CheckFlag(FlagStructure flagToCheck)
    {
        if (flagToCheck.actualValue)
        {
            return playerTrueFlags.Find(f => f.flag == flagToCheck.flag) != null;
        }
        else
        {
            return playerFalseFlags.Find(f => f.flag == flagToCheck.flag) != null;
        }
    }

    public bool CheckFlag(List<FlagStructure> flagsToCheck)
    {
        bool tmpResult = true;
        foreach (var flagToCheck in flagsToCheck)
        {
            if (flagToCheck.actualValue)
            {
                tmpResult = playerTrueFlags.Find(f => f.flag == flagToCheck.flag) != null;
            }
            else
            {
                tmpResult = playerFalseFlags.Find(f => f.flag == flagToCheck.flag) != null;
            }

            if (tmpResult == false)
            {
                return tmpResult;
            }
        }
        return tmpResult;
    }

    public void SetFlag(FlagStructure flagToSet)
    {
        if (flagToSet.actualValue)
        {
            FlagStructure flagFinded = playerFalseFlags.Find(f => f.flag == flagToSet.flag);
            if (flagFinded != null)
            {
                flagFinded.actualValue = flagToSet.actualValue;
                playerFalseFlags.Remove(flagFinded);
                playerTrueFlags.Add(flagFinded);
            }
            else
            {
                flagFinded = playerTrueFlags.Find(f => f.flag == flagToSet.flag);
                if (flagFinded == null)
                {
                    Debug.LogWarning("Flag non trovata nel manager, procedo ad aggiungerla alle trueFlag");
                    playerTrueFlags.Add(flagToSet);
                }
            }
        }
        else
        {
            FlagStructure flagFinded = playerTrueFlags.Find(f => f.flag == flagToSet.flag);
            if (flagFinded != null)
            {
                flagFinded.actualValue = flagToSet.actualValue;
                playerTrueFlags.Remove(flagFinded);
                playerFalseFlags.Add(flagFinded);
            }
            else
            {
                flagFinded = playerFalseFlags.Find(f => f.flag == flagToSet.flag);
                if (flagFinded == null)
                {
                    Debug.LogWarning("Flag non trovata nel manager, procedo ad aggiungerla alle falseFlag");
                    playerFalseFlags.Add(flagToSet);
                }
            }
        }
    }

    public void SetFlag(List<FlagStructure> flagsToSet)
    {
        foreach (var flagToSet in flagsToSet)
        {
            if (flagToSet.actualValue)
            {
                FlagStructure flagFinded = playerFalseFlags.Find(f => f.flag == flagToSet.flag);
                if (flagFinded != null)
                {
                    flagFinded.actualValue = flagToSet.actualValue;
                    playerFalseFlags.Remove(flagFinded);
                    playerTrueFlags.Add(flagFinded);
                }
                else
                {
                    flagFinded = playerTrueFlags.Find(f => f.flag == flagToSet.flag);
                    if (flagFinded == null)
                    {
                        Debug.LogWarning("Flag non trovata nel manager, procedo ad aggiungerla alle trueFlag");
                        playerTrueFlags.Add(flagToSet);
                    }
                }
            }
            else
            {
                FlagStructure flagFinded = playerTrueFlags.Find(f => f.flag == flagToSet.flag);
                if (flagFinded != null)
                {
                    flagFinded.actualValue = flagToSet.actualValue;
                    playerTrueFlags.Remove(flagFinded);
                    playerFalseFlags.Add(flagFinded);
                }
                else
                {
                    flagFinded = playerFalseFlags.Find(f => f.flag == flagToSet.flag);
                    if (flagFinded == null)
                    {
                        Debug.LogWarning("Flag non trovata nel manager, procedo ad aggiungerla alle falseFlag");
                        playerFalseFlags.Add(flagToSet);
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class FlagStructure
{
    public string flagName;
    public FlagsSO flag;
    public bool actualValue;
}
[System.Serializable]
public class RequiredFlag
{
    public List<FlagStructure> flagList;
}
[System.Serializable]
public class RequisitoFlag
{
    public List<FlagStructure> flagToSet;
    public List<RequiredFlag> requiredFlagList;
}
