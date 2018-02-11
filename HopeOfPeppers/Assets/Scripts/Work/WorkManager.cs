using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager
{
    private List<Work> workList = new List<Work>();

    public WorkManager()
    {
        SetWork((int)GAMEDATA.DATA.WORK_KIND.WORK_1, 1);
    }

    public void SetWork(int aKind, int aLevel)
    {
        var workLevelInfo = GAMEDATA.GAMEDATAINFOS.Instance.GetWorkLevelData(aKind, aLevel);
        if (null == workLevelInfo)
            return;

        var workInfo = workList.Find((work) => {
            if (work.Kind == aKind)
                return true;
            else
                return false;
        });

        if (null == workInfo)
        {
            workList.Add(new Work(aKind, aLevel, workLevelInfo.WorkTime, 0f));
        }
        else
        {
            workInfo = new Work(aKind, aLevel, workLevelInfo.WorkTime, 0f);
        }
    }

    public void RemoveWork(int aKind)
    {
        var workInfo = workList.Find((work) => {
            if (work.Kind == aKind)
                return true;
            else
                return false;
        });

        if (null == workInfo)
            return;

        workList.Remove(workInfo);
    }

    public void Update(float aDeltaTime)
    {
        for (int i = 0; i < workList.Count; ++i)
        {
            workList[i].Update(aDeltaTime);
        }
    }

    public Work GetWorkInfo(int aKind)
    {
        var workInfo = workList.Find((work) => {
            if (work.Kind == aKind)
                return true;
            else
                return false;
        });

        return workInfo;
    }


    public bool IsCanUpgrade(int aKind, int aUpgradeLevel)
    {
        var targetWorkLevelData = GAMEDATA.GAMEDATAINFOS.Instance.GetWorkLevelData(aKind, aUpgradeLevel);
        if (null == targetWorkLevelData)
            return false;

        //개방이 된 워크인지 체크
        //이전 워크가 개방이 되어 있어야 한다.!! 
        for (int i = (int)GAMEDATA.DATA.WORK_KIND.WORK_BEGIN; i < aKind; ++i)
        {
            var workInfo = GetWorkInfo(i);
            if (null == workInfo)
                return false;
        }


        //레벨 체크! 
        var currentWorkInfo = GetWorkInfo(aKind);
        if (null != currentWorkInfo)
        {
            if (aUpgradeLevel <= currentWorkInfo.Level)
                return false;
        }


        //돈 체크!
        if (GameMain.Instance.UserInfo.Gold < GetUpgradeRequireGold(aKind, aUpgradeLevel))
            return false;


        return true;
    }


    public Int64 GetUpgradeRequireGold(int aKind, int aUpgradeLevel)
    {
        int currentLevel = 0;
        var currentWorkInfo = GetWorkInfo(aKind);
        if (null != currentWorkInfo)
            currentLevel = currentWorkInfo.Level;

        Int64 needGold = 0;
        for (int i = currentLevel + 1; i <= aUpgradeLevel; ++i)
        {
            var workData = GAMEDATA.GAMEDATAINFOS.Instance.GetWorkLevelData(aKind, i);
            if (null != workData)
            {
                needGold += workData.RequireForNextLevel;
            }
        }

        return needGold;
    }


    public bool UpgradeWork(int aKind, int aLevel)
    {
        if (false == IsCanUpgrade(aKind, aLevel))
            return false;

        Int64 requireGold = GetUpgradeRequireGold(aKind, aLevel);

        GameMain.Instance.UserInfo.SubstructGold(requireGold);
        SetWork(aKind, aLevel);

        return true;
    }

    public List<Work> GetWorkList()
    {
        return workList;
    }

}
