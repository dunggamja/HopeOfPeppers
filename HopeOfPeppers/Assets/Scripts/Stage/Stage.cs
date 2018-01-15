using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public GAMEDATA.DATA.GAMEDATA_STAGE_BACKGROUND[] BackgroundTexture { get; private set; }
    public Vector2 StageSize { get; private set; }
    public Vector2 TerrainStartPos { get; private set; }
    public Vector2 TerrainEndPos { get; private set; }
    public int Level { get; private set; }

    private SpawnHelper spawnHelper = new SpawnHelper();
    private float timeSinceStageStart = 0f;


    private GameObject backgroundFar = null;
    private GameObject backgroundNear = null;
    


    public Stage(GAMEDATA.DATA.GAMEDATA_STAGE_BACKGROUND[] aBackgroundTexture, Vector2 aStageSize, Vector2 aTerrainStartPos, Vector2 aTerrainEndPos, int aLevel)
    {
        Init();
        BackgroundTexture = aBackgroundTexture;
        StageSize = aStageSize;
        TerrainStartPos = aTerrainStartPos;
        TerrainEndPos = aTerrainEndPos;
        Level = aLevel;
    }

    public void Init()
    {
        BackgroundTexture = null;
        StageSize = Vector2.zero;
        TerrainStartPos = Vector2.zero;
        TerrainEndPos = Vector2.zero;
        Level = 0;
        if (null == spawnHelper)
            spawnHelper = new SpawnHelper();

        spawnHelper.Clear();
        var stageData = GAMEDATA.GAMEDATAINFOS.Instance.GetStageData(Level);
        if (null != stageData && null != stageData.SpawnList)
        {
            for (int i = 0; i < stageData.SpawnList.Count; ++i)
            {
                var data = stageData.SpawnList[i];
                if (null == data) continue;

                spawnHelper.Insert(data.SpawnType, data.UnitKind, data.SpawnSeconds);
            }
        }

        spawnHelper.Sort_SpawnInfoList_BySpawnTime();
    }


    
    public void Update(float aDeltaTime)
    {
        timeSinceStageStart += aDeltaTime;

        List<SpawnInfo> spawnList = new List<SpawnInfo>();
        for (int i = (int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN; i < (int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END; ++i)
        {
            spawnHelper.Pop_SpawnInfo_By_Time((SpawnInfo.SPAWN_TYPE)i, (int)timeSinceStageStart, ref spawnList);
        }
    }

    public void Instantiate_Stage()
    {
        if (null != backgroundFar)
        {
            GameObject.Destroy(backgroundFar);
            backgroundFar = null;
        }

        if (null != backgroundNear)
        {
            GameObject.Destroy(backgroundNear);
            backgroundNear = null;
        }

        GameObject originFar = Resources.Load("Prefabs/Stage/Background_Far") as GameObject;
        GameObject originNear = Resources.Load("Prefabs/Stage/Background_Near") as GameObject;

        backgroundFar = GameObject.Instantiate(originFar);
        backgroundFar.transform.position = Vector3.zero;

        backgroundNear = GameObject.Instantiate(originNear);
        backgroundNear.transform.position = Vector3.zero;

        int farIdx = 0;
        int nearIdx = 0;

        for (int i = 0; i < BackgroundTexture.Length; ++i)
        {
            if (BackgroundTexture[i] == null) continue;

            Sprite sprLoad = null;

            if (false == string.IsNullOrEmpty(BackgroundTexture[i].imgSubFileName))
            {
                Sprite[] loadAll = Resources.LoadAll<Sprite>(string.Format("Images/BackgroundTexture/{0}", BackgroundTexture[i].imgFileName));
                if (null != loadAll)
                {
                    for (int loadIdx = 0; loadIdx < loadAll.Length; ++loadIdx)
                    {
                        if (string.Equals(loadAll[loadIdx].name, BackgroundTexture[i].imgSubFileName))
                        {
                            sprLoad = loadAll[loadIdx];
                            break;
                        }
                    }
                }
            }
            else
            {
                sprLoad = Resources.Load(string.Format("Images/BackgroundTexture/{0}", BackgroundTexture[i].imgFileName)) as Sprite;
            }

            if (null == sprLoad)
            {
                Debug.Log("sprite is null");
                continue;
            }

            GameObject parentObj = null;
            string sortingLayerName = string.Empty;
            int sortIdx = 0;

            if (BackgroundTexture[i].backgroundType == GAMEDATA.DATA.GAMEDATA_STAGE_BACKGROUND.BACKGROUND_TYPE.BACKGROUND_FAR)
            {
                sortIdx = farIdx++;
                parentObj = backgroundFar;
                sortingLayerName = "BACKGROUND_FAR";
            }
            else if (BackgroundTexture[i].backgroundType == GAMEDATA.DATA.GAMEDATA_STAGE_BACKGROUND.BACKGROUND_TYPE.BACKGROUND_NEAR)
            {
                sortIdx = nearIdx++;
                parentObj = backgroundNear;
                sortingLayerName = "BACKGROUND_NEAR";
            }
            else
            {
                Debug.Log(BackgroundTexture[i].imgFileName);
                continue;
            }

            GameObject childBackground = new GameObject(BackgroundTexture[i].imgFileName);
            childBackground.transform.parent = parentObj.transform;
            childBackground.transform.localPosition = Vector3.zero;

            var spriteRenderer = childBackground.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprLoad;
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = sortIdx;

        }
    }

}
