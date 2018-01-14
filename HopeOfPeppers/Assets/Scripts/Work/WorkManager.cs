using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager
{
    private List<Work> workList = new List<Work>();

    public void AddWork(int aKind, int aLevel)
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

}
