using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager
{
    [System.Serializable]
    public class STAGEINFO_FOR_SAVE
    {
        public int Level = 0;
    }

    public readonly string KEY_STAGEINFO = "STAGEINFO_SAVE_KEY";

    public STAGEINFO_FOR_SAVE stageInfo { get; private set; }
    public Stage currentStage { get; private set; }

    public Action Post_OnCompleteSceneLoaded;

    public StageManager()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        currentStage = null;
        stageInfo = null;

        LoadSavedStgageInfo();
    }

    private void OnSceneLoaded(Scene aLoadedScene, LoadSceneMode aSceneMode)
    {
        if (string.Equals(aLoadedScene.name, "GameScene"))
        {
            if(null != stageInfo)
                SetStage(stageInfo.Level);   
        }

        if (null != Post_OnCompleteSceneLoaded)
            Post_OnCompleteSceneLoaded();
    }

    private bool SetStage(int aLevel)
    {
        //셋팅은 여기서 하고.... 
        stageInfo = new STAGEINFO_FOR_SAVE();
        stageInfo.Level = aLevel;

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

    public void Update(float aDeltaTime)
    {
        if (null != currentStage)
            currentStage.Update(aDeltaTime);
    }

    public void LoadLevel(int aLevel)
    {
        
        if (false == string.Equals("GameScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
        {

            GameMain.Instance.GameMainCoroutine += test;
            //현재 GameScene이 아닐 경우  씬로드 후 스테이지 오브젝트 생성.
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");

            stageInfo = new STAGEINFO_FOR_SAVE();
            stageInfo.Level = aLevel;
        }
        else
        {
            //현재 GameScene일 경우 바로 스테이지 오브젝트 생성.
            SetStage(aLevel);
        }
    }

    private IEnumerator test()
    {
        yield return null;
    }

    private void LoadSavedStgageInfo()
    {
        string loadString = PlayerPrefs.GetString(KEY_STAGEINFO);
        if (false == string.IsNullOrEmpty(loadString))
        {
            stageInfo = JsonUtility.FromJson<STAGEINFO_FOR_SAVE>(loadString);
        }
    }

    private void SaveStageInfo(STAGEINFO_FOR_SAVE data)
    {
        if (null != data)
        {
            PlayerPrefs.SetString(KEY_STAGEINFO, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }
    }
}
