using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageManager
{
    [System.Serializable]
    public class CurrentStageInfo
    {
        public int Level = 0;
    }

    public CurrentStageInfo currentStageInfo { get; private set; }
    public Stage currentStage { get; private set; }

    public StageManager()
    {
        currentStage = null;
        currentStageInfo = null;
    }

    public bool SetStage(int aLevel)
    {
        if(null == currentStageInfo)
            currentStageInfo = new CurrentStageInfo();
        currentStageInfo.Level = aLevel;

        var stageData = GAMEDATA.GAMEDATAINFOS.Instance.GetStageData(aLevel);
        if (null == stageData)
            return false;

        Texture backgroundTexture = Resources.Load(stageData.BackgroundTexture) as Texture;
        Vector2 backgroundSize = new Vector2(stageData.BackgroundSize_X, stageData.BackgroundSize_Y);
        Vector2 terrainStart = new Vector2(stageData.TerrainStart_X, stageData.TerrainStart_Y);
        Vector2 terrainEnd = new Vector2(stageData.TerrainEnd_X, stageData.TerrainEnd_Y);

        currentStage = new Stage(backgroundTexture, backgroundSize, terrainStart, terrainEnd, stageData.Level);
        return true;
    }

    public string SaveCurrentStageToJson()
    {
        return JsonUtility.ToJson(currentStageInfo);
    }

    public void LoadCurrentStageFromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
            return;

        currentStageInfo = JsonUtility.FromJson<CurrentStageInfo>(json);
    }

    public void Update(float aDeltaTime)
    {

    }
}
