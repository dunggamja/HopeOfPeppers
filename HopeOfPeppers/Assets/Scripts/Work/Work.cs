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

    public void Update(float aDeltaTime)
    {
        CurAccumulateTime += aDeltaTime;

        if (WorkTime <= CurAccumulateTime)
        {
            //자원 획득 코드 추가해야됨.!!
            int addGold = GetAddGold();

            CurAccumulateTime = 0;
        }
    }

    private int GetAddGold()
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
