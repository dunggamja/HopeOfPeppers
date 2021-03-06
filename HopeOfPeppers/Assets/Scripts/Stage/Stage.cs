﻿
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


    private static GameObject backgroundFar = null;
    private static GameObject backgroundNear = null;
    private static GameObject cloud = null;
    


    public Stage(GAMEDATA.DATA.GAMEDATA_STAGE_BACKGROUND[] aBackgroundTexture, Vector2 aStageSize, Vector2 aTerrainStartPos, Vector2 aTerrainEndPos, int aLevel)
    {
        Init();
        BackgroundTexture = aBackgroundTexture;
        StageSize = aStageSize;
        TerrainStartPos = aTerrainStartPos;
        TerrainEndPos = aTerrainEndPos;
        Level = aLevel;

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

    public void Init()
    {
        BackgroundTexture = null;
        StageSize = Vector2.zero;
        TerrainStartPos = Vector2.zero;
        TerrainEndPos = Vector2.zero;
        Level = 0;
        if (null == spawnHelper)
            spawnHelper = new SpawnHelper();

       

      
    }


    
    public void Update(float aDeltaTime)
    {
        timeSinceStageStart += aDeltaTime;

        List<SpawnInfo> spawnList = new List<SpawnInfo>();
        for (int i = (int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN; i < (int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END; ++i)
        {
            spawnHelper.Pop_SpawnInfo_By_Time((SpawnInfo.SPAWN_TYPE)i, (int)timeSinceStageStart, ref spawnList);
            
        }

        cloud.transform.localPosition += new Vector3(aDeltaTime, 0, 0);
        if (cloud.transform.localPosition.x > cloud.GetComponent<SpriteRenderer>().size.x / 4)
        {
            cloud.transform.localPosition = new Vector3(-cloud.GetComponent<SpriteRenderer>().size.x / 4, cloud.transform.localPosition.y, cloud.transform.localPosition.z);
        }

        if (0 < spawnList.Count)
        {
            Debug.Log(spawnList.Count);
        }

        
        GameObject enemyOriginal = Resources.Load("Prefabs/Character/Rogue/Rogue_01") as GameObject;
        for (int i = 0; i < spawnList.Count; ++i)
        {
            GameObject enemy = GameObject.Instantiate(enemyOriginal);
            UnitAction enemyAction = enemy.AddComponent<UnitAction>();
            enemy.transform.localPosition = new Vector3(8, -2.1f, 1);
            enemyAction.unit = UnitManager.instance.CreateUnit(2, (int)GAMEDATA.DATA.UNIT_KIND.UNIT_LUCINA, 1, enemy.transform.localPosition);
            enemyAction.campId = 2;
            enemyAction.unitKind = (int)GAMEDATA.DATA.UNIT_KIND.UNIT_LUCINA;
            enemyAction.unitLevel = 1;
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

            

            Debug.Log(spriteRenderer.size);

        }

        
        cloud = backgroundFar.transform.GetChild(0).gameObject;
        var tmpSpriteRenderer = cloud.GetComponent<SpriteRenderer>();
        tmpSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        tmpSpriteRenderer.size = new Vector2(tmpSpriteRenderer.size.x * 2, tmpSpriteRenderer.size.y);


        //필요한작업! 
        // 아군 적군 스폰 건물 생성
        GameObject oriSpawn1  = Resources.Load("Prefabs/Spawn/Spawn_1") as GameObject;
        GameObject userSpawn = GameObject.Instantiate(oriSpawn1);
        userSpawn.transform.localPosition = new Vector3(-8, -2, 1);
        UnitAction spawnAction1 = userSpawn.AddComponent<UnitAction>();
        spawnAction1.unit = UnitManager.instance.CreateUnit(1, (int)GAMEDATA.DATA.UNIT_KIND.UNIT_SPWAN, 1, userSpawn.transform.localPosition);
        spawnAction1.campId = 1;
        spawnAction1.unitKind = (int)GAMEDATA.DATA.UNIT_KIND.UNIT_SPWAN;
        spawnAction1.unitLevel = 1;
 



        GameObject oriSpawn2 = Resources.Load("Prefabs/Spawn/Spawn_2") as GameObject;
        GameObject enemySpawn = GameObject.Instantiate(oriSpawn2);
        enemySpawn.transform.localPosition = new Vector3(8, -2, 1);
        UnitAction spawnAction2 = enemySpawn.AddComponent<UnitAction>();
        spawnAction2.unit = UnitManager.instance.CreateUnit(2, (int)GAMEDATA.DATA.UNIT_KIND.UNIT_SPWAN, 1, enemySpawn.transform.localPosition);
        spawnAction2.campId = 2;
        spawnAction2.unitKind = (int)GAMEDATA.DATA.UNIT_KIND.UNIT_SPWAN;
        spawnAction2.unitLevel = 1;

        // 아군 인원은 한번에 다 생성

        GameObject myArmy = Resources.Load("Prefabs/Character/Lucina") as GameObject;
        for (int i = 0; i < 2; ++i)
        {
            GameObject army = GameObject.Instantiate(myArmy);
            UnitAction armyAction = army.AddComponent<UnitAction>();
            army.transform.localPosition = new Vector3(-8+0.2f*i, -2.1f-0.2f*i, 1);
            armyAction.unit = UnitManager.instance.CreateUnit(1, (int)GAMEDATA.DATA.UNIT_KIND.UNIT_LUCINA, 1, army.transform.localPosition);
            armyAction.campId = 1;
            armyAction.unitKind = (int)GAMEDATA.DATA.UNIT_KIND.UNIT_LUCINA;
            armyAction.unitLevel = 1;
        }


    }

}
