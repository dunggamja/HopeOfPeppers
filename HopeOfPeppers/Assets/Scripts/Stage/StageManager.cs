using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager
{
    [System.Serializable]
    public class CurrentStageInfo
    {
        public int Level = 0;
    }

    public CurrentStageInfo currentStageInfo { get; private set; }
    public Stage currentStage { get; private set; }

    public Action Post_OnCompleteSceneLoaded;

    public StageManager()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        currentStage = null;
        currentStageInfo = null;
    }

    private void OnSceneLoaded(Scene aLoadedScene, LoadSceneMode aSceneMode)
    {
        if(null != Post_OnCompleteSceneLoaded)
            Post_OnCompleteSceneLoaded();
    }

    private bool SetStage(int aLevel)
    {
        if(null == currentStageInfo)
            currentStageInfo = new CurrentStageInfo();
        currentStageInfo.Level = aLevel;

        var stageData = GAMEDATA.GAMEDATAINFOS.Instance.GetStageData(aLevel);
        if (null == stageData)
            return false;

        Vector2 backgroundSize = new Vector2(stageData.BackgroundSize_X, stageData.BackgroundSize_Y);
        Vector2 terrainStart = new Vector2(stageData.TerrainStart_X, stageData.TerrainStart_Y);
        Vector2 terrainEnd = new Vector2(stageData.TerrainEnd_X, stageData.TerrainEnd_Y);

        currentStage = new Stage(stageData.BackgroundTexture, backgroundSize, terrainStart, terrainEnd, stageData.Level);
        currentStage.Instantiate_Stage();
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
        if (null != currentStage)
            currentStage.Update(aDeltaTime);
    }

    public void LoadLevel(int aLevel)
    {
        if (false == string.Equals("GameScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            Post_OnCompleteSceneLoaded += () =>
            {
                if (string.Equals("GameScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
                    SetStage(aLevel);
            };
        }
        else
        {
            SetStage(aLevel);
        }
    }
}
