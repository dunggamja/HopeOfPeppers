using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorkGridData : UI_GridItemData
{
    public int workKind = 0;

    public UI_WorkGridData(int aKind)
    {
        workKind = aKind;
    }
}

public class UI_WorkGrid : UI_GridItem
{
    public Slider sliderWorkProgress ;
    public Text textWorkDescription ;
   

    public override void UpdateUI(UI_GridItemData itemData)
    {
        base.UpdateUI(itemData);

        UI_WorkGridData workData = itemData as UI_WorkGridData;
        if (null == workData)
            return;

        var workInfo = GameMain.Instance.WorkMgr.GetWorkInfo(workData.workKind);
        if (null == workInfo)
            return;

        int totalTime = workInfo.WorkTime;
        if (totalTime <= 0)
            totalTime = 1;

        sliderWorkProgress.value = Mathf.Clamp01((float)workInfo.CurAccumulateTime / (float)totalTime);
        textWorkDescription.text = string.Format("레벨 : {0}, 버는 돈 : {1}", workInfo.Level, workInfo.GetEarningGold());
    }
}
