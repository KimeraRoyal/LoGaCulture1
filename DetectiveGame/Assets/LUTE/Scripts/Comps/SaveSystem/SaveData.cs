using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// This component encodes and decodes a list of game objects to be saved for each Save Point.
/// To extend the save system to handle other data types, just modify or subclass this component.
public class SaveData : MonoBehaviour
{
    protected const string EngineDataKey = "EngineData";
    protected const string LogKey = "LogData";

    protected List<BasicFlowEngine> engines;

    private void Awake()
    {
        engines = FindObjectsOfType<BasicFlowEngine>().ToList();
    }

    public virtual void Encode(List<SaveDataItem> saveDataItems)
    {
        for(int i = 0; i < engines.Count; i++)
        {
            var engine = engines[i];
            var engineData = EngineData.Encode(engine);

            var saveDataItem = SaveDataItem.Create(EngineDataKey, JsonUtility.ToJson(engineData))   ;
            saveDataItems.Add(saveDataItem);

            var logData = SaveDataItem.Create(LogKey, LogaManager.Instance.SaveLog.GetJsonHistory());
            saveDataItems.Add(logData);
        }
    }

    public virtual void Decode(List<SaveDataItem> saveDataItems)
    {
        for(int i = 0; i < saveDataItems.Count; i++)
        {
            var saveDataItem = saveDataItems[i];
            if(saveDataItem == null)
            {
                continue;
            }

            if(saveDataItem.Type == EngineDataKey)
            {
                var engineData = JsonUtility.FromJson<EngineData>(saveDataItem.Data);
                if(engineData == null)
                {
                    Debug.LogError("Engine data is null so failed to decode engine data");
                    return;
                }

                EngineData.Decode(engineData);
            }

            if(saveDataItem.Type == LogKey)
            {
                LogaManager.Instance.SaveLog.LoadLogData(saveDataItem.Data);
            }
        }
    }
}
