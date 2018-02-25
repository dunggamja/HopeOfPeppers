using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Work
{
    public int Kind { get; private set; }
    public int Level { get; private set; }
    public int WorkTime { get; private set; }
    public float CurAccumulateTime { get; private set; }

    public Work(int aWorkKind, int aWorkLevel, int aWorkTime, float aCurAccumulateTime)
    {
        Kind = aWorkKind;
        Level = aWorkLevel;
        WorkTime = aWorkTime;
        CurAccumulateTime = aCurAccumulateTime;
    }

    public void UpgradeLevel(int aLevel)
    {
        if (aLevel <= Level)
            return;

        var workLevelInfo = GAMEDATA.GAMEDATAINFOS.Instance.GetWorkLevelData(Kind, aLevel);
        if (null == workLevelInfo)
            return;

        Level = aLevel;
        WorkTime = workLevelInfo.WorkTime;
        CurAccumulateTime = 0f;
    }

    public void Update(float aDeltaTime)
    {
        CurAccumulateTime += aDeltaTime;

        if (WorkTime <= CurAccumulateTime)
        {
            //골드 획득
            GameMain.Instance.UserInfo.AddGold(GetEarningGold());

            Debug.Log("AddGold : " + GameMain.Instance.UserInfo.Gold);

            CurAccumulateTime = 0;
        }
    }

    public int GetEarningGold()
    {
        int addGold = 0;

        var workInfo = GAMEDATA.GAMEDATAINFOS.Instance.GetWorkLevelData(Kind, Level);
        if (null != workInfo)
        {
            addGold = workInfo.EarningGold;
        }

        return addGold;
    }

    

}
