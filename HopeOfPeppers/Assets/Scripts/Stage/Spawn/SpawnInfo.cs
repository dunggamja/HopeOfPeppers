using System;
using System.Collections;
using System.Collections.Generic;


public class SpawnInfo
{
    public enum SPAWN_TYPE : int
    {
        SPAWN_TYPE_BEGIN,
        SPAWN_TYPE_NORMAL = SPAWN_TYPE_BEGIN,
        SPAWN_TYPE_END,
    }

    private SPAWN_TYPE spawnType = SPAWN_TYPE.SPAWN_TYPE_NORMAL;
    private Int32   spawnUnitKind = 0;
    private Int32   spawnSeconds = 0;

    public SPAWN_TYPE SpawnType { get { return spawnType; }  private set { spawnType = value; } }
    public Int32 SpawnUnitKind { get { return spawnUnitKind; } private set { spawnUnitKind = value; } }
    public Int32 SpawnSeconds { get { return spawnSeconds; } private set { spawnSeconds = value; } }

    public SpawnInfo() { }
    public SpawnInfo(SPAWN_TYPE aSpawnType, Int32 aUnitKind, Int32 aSpawnSeconds)
    {
        spawnType = aSpawnType;
        spawnUnitKind = aUnitKind;
        spawnSeconds = aSpawnSeconds;
    }


}
