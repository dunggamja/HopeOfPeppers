using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public Texture BackgroundTexture { get; private set; }
    public Vector2 StageSize { get; private set; }
    public Vector2 TerrainStartPos { get; private set; }
    public Vector2 TerrainEndPos { get; private set; }
    public int Level { get; private set; }

    private SpawnHelper spawnHelper = new SpawnHelper();
    private float timeSinceStageStart = 0f;

    public Stage(Texture aBackgroundTexture, Vector2 aStageSize, Vector2 aTerrainStartPos, Vector2 aTerrainEndPos, int aLevel)
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

    }


    private List<SpawnInfo> spawnList = new List<SpawnInfo>();
    public void Update()
    {
        
        for (int i = (int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN; i < (int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END; ++i)
        {
            spawnHelper.Pop_SpawnInfo_By_Time((SpawnInfo.SPAWN_TYPE)i, (int)timeSinceStageStart, ref spawnList);
        }
        
    }


}
