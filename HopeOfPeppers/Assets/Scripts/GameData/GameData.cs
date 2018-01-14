﻿using System;
using System.Collections;
using System.Collections.Generic;


namespace GAMEDATA
{
    namespace DATA
    {
        public enum STAGE_LEVEL : int
        {
            LEVEL_NONE,
            LEVEL_BEGIN,
            LEVEL_1 = LEVEL_BEGIN,
            LEVEL_2,
            LEVEL_END
        }

        public enum WORK_KIND : int
        {
            WORK_NONE,
            WORK_BEGIN,
            WORK_1 = WORK_BEGIN,
            WORK_2,
            WORK_KIND_END
        }

        public class GAMEDATA_STAGE
        {
            public readonly int Level = 0;
            public readonly string BackgroundTexture = string.Empty;
            public readonly int BackgroundSize_X = 0;
            public readonly int BackgroundSize_Y = 0;
            public readonly int TerrainStart_X = 0;
            public readonly int TerrainStart_Y = 0;
            public readonly int TerrainEnd_X = 0;
            public readonly int TerrainEnd_Y = 0;
            public readonly List<GAMEDATA_SPAWN> SpawnList = new List<GAMEDATA_SPAWN>();

            public GAMEDATA_STAGE(int aLevel, string aBackgroundTexture,
                int aBackgroundSizeX, int aBackgroundSizeY,
                int aTerrainStartX, int aTerrainStartY,
                int aTerrainEndX, int aTerrainEndY,
                List<GAMEDATA_SPAWN> aSpawnList)
            {
                Level = aLevel;
                BackgroundTexture = aBackgroundTexture;
                BackgroundSize_X = aBackgroundSizeX;
                BackgroundSize_Y = aBackgroundSizeY;
                TerrainStart_X = aTerrainStartX;
                TerrainStart_Y = aTerrainStartY;
                TerrainEnd_X = aTerrainEndX;
                TerrainEnd_Y = aTerrainEndY;
                SpawnList = aSpawnList;
            }
        }

        public class GAMEDATA_SPAWN
        {
            public readonly SpawnInfo.SPAWN_TYPE SpawnType = SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_NORMAL;
            public readonly Int32 UnitKind = 0;
            public readonly Int32 SpawnSeconds = 0;

            public GAMEDATA_SPAWN(SpawnInfo.SPAWN_TYPE aSpawnType, int aUnitKind, int aSpawnSeconds)
            {
                SpawnType = aSpawnType;
                UnitKind = aUnitKind;
                SpawnSeconds = aSpawnSeconds;
            }
        }

        public class GAMEDATA_WORK_LEVEL
        {
            public readonly int Level = 0;
            public readonly int WorkTime = 0;
            public readonly int EarningGold = 0;
            public readonly int RequireForNextLevel = 0;

            public GAMEDATA_WORK_LEVEL(int aLevel, int aWorkTime, int aEarning, int aRequireNextLevel)
            {
                Level = aLevel;
                WorkTime = aWorkTime;
                EarningGold = aEarning;
                RequireForNextLevel = aRequireNextLevel;
            }
        }

        public class GAMEDATA_WORK
        {
            public readonly int workKind = 0;
            public readonly int maxLevel = 0;
            public readonly List<GAMEDATA_WORK_LEVEL> workLevelInfos = new List<GAMEDATA_WORK_LEVEL>();
            public GAMEDATA_WORK(int aKind, List<GAMEDATA_WORK_LEVEL> aListWorkLevel)
            {
                workKind = aKind;
                workLevelInfos = aListWorkLevel;
                maxLevel = aListWorkLevel.Count;
            }
        }

    }



    public class GAMEDATAINFOS
    {

        private static GAMEDATAINFOS instance = null;
        public static GAMEDATAINFOS Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = new GAMEDATAINFOS();
                return instance;
            }
        }

        private GAMEDATAINFOS()
        {
            Init_Datas();
        }

        private void Init_Datas()
        {
            //스테이지 정보...
            for (int i = (int)DATA.STAGE_LEVEL.LEVEL_BEGIN; i < (int)DATA.STAGE_LEVEL.LEVEL_END; ++i)
            {

                List<DATA.GAMEDATA_SPAWN> spawnList = new List<DATA.GAMEDATA_SPAWN>();
                for (int k = 0; k < (i + 1) * 4; ++k)
                {
                    DATA.GAMEDATA_SPAWN spawnData = new DATA.GAMEDATA_SPAWN(SpawnInfo.SPAWN_TYPE.SPAWN_TYPE_NORMAL, 1, k * 5);
                    spawnList.Add(spawnData);
                }

                DATA.GAMEDATA_STAGE data = new DATA.GAMEDATA_STAGE(i
                    , string.Empty, 1280, 600
                    , 0, 0, 1280, 600
                    , spawnList);

                stageDatas.Add(data.Level, data);
            }


            //워크 정보...
            for (int i = (int)DATA.WORK_KIND.WORK_BEGIN; i < (int)DATA.WORK_KIND.WORK_KIND_END; ++i)
            {
                List<DATA.GAMEDATA_WORK_LEVEL> workLevelList = new List<DATA.GAMEDATA_WORK_LEVEL>();
                for (int k = 1; k < 4; ++k)
                {
                    DATA.GAMEDATA_WORK_LEVEL levelData = new DATA.GAMEDATA_WORK_LEVEL(k , k + 5, k * 2, k );
                    workLevelList.Add(levelData);
                }

                DATA.GAMEDATA_WORK data = new DATA.GAMEDATA_WORK(i, workLevelList);
                workDatas.Add(data.workKind, data);
            }
        }

        private Dictionary<int, DATA.GAMEDATA_STAGE> stageDatas = new Dictionary<int, DATA.GAMEDATA_STAGE>();
        private Dictionary<int, DATA.GAMEDATA_WORK> workDatas = new Dictionary<int, DATA.GAMEDATA_WORK>();


        public DATA.GAMEDATA_WORK GetWorkData(int aKind)
        {
            if (workDatas.ContainsKey(aKind))
                return workDatas[aKind];

            return null;
        }

        public DATA.GAMEDATA_WORK_LEVEL GetWorkLevelData(int aKind, int aLevel)
        {
            if (workDatas.ContainsKey(aKind))
            {
                DATA.GAMEDATA_WORK_LEVEL levelData = workDatas[aKind].workLevelInfos.Find((data) =>
                {
                    if (data.Level == aLevel)
                        return true;
                    else
                        return false;
                });
                return levelData;
            }

            return null;
        }


        public DATA.GAMEDATA_STAGE GetStageData(int aKind)
        {
            if (stageDatas.ContainsKey(aKind))
                return stageDatas[aKind];

            return null;
        }

    }
}
