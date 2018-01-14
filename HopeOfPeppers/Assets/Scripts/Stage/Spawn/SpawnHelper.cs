using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHelper
{
    private List<SpawnInfo>[] listSpawnInfo = new List<SpawnInfo>[(int)SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END];



    public void Clear()
    {
        for (SpawnInfo.SPAWN_TYPE spawnType = SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN; spawnType < SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END; ++spawnType)
            ClearSpawnList(spawnType);
    }

    public void ClearSpawnList(SpawnInfo.SPAWN_TYPE  aSpawnType)
    {
        if (aSpawnType < SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN || SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END <= aSpawnType)
            return;

        listSpawnInfo[(int)aSpawnType].Clear();
    }

    public void Insert(SpawnInfo.SPAWN_TYPE aSpawnType, int aUnitKind, Int32 aSpawnSeconds)
    {
        if (aSpawnType < SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN || SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END <= aSpawnType)
            return;

        listSpawnInfo[(int)aSpawnType].Add(new SpawnInfo(aSpawnType, aUnitKind, aSpawnSeconds));
    }

    public void Sort_SpawnInfoList_BySpawnTime()
    {
        for (SpawnInfo.SPAWN_TYPE spawnType = SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN; spawnType < SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END; ++spawnType)
        {
            List<SpawnInfo> listSpawn = listSpawnInfo[(int)spawnType];
            if (null == listSpawn) continue;

            listSpawn.Sort((SpawnInfo src, SpawnInfo dst)=>{
                if (null == src || null == dst)
                    return 0;
                return (src.SpawnSeconds - dst.SpawnSeconds);
            });
         }        
    }

    public void Pop_SpawnInfo_By_Time(SpawnInfo.SPAWN_TYPE aSpawnType, Int32 aCurrentSeconds, ref List<SpawnInfo> aListPop)
    {
        if (aSpawnType < SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_BEGIN || SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_END <= aSpawnType)
            return;

        List<SpawnInfo> listSpawn = listSpawnInfo[(int)aSpawnType];
        if (null == listSpawn) return;


        int popCount = 0;
        for (; popCount < listSpawn.Count; ++popCount)
        {
            if (null == listSpawn[popCount]) continue;
            if (listSpawn[popCount].SpawnSeconds <= aCurrentSeconds)
            {
                //시간값 체크 후  리스트에 추가 
                aListPop.Add(listSpawn[popCount]);
            }
            else
            {
                //이미 스폰리스트는 등장 시간순으로 정렬이 되어 있으므로 break!!.
                break;
            }
        }

        //시간값 체크 후 실행될 정보는 삭제된다...;;
        listSpawn.RemoveRange(0, popCount);
    }
}
