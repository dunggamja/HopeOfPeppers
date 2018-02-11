using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : UI_Base
{
    public UI_GridController gridController;

    public override void Open()
    {
        base.Open();


        gridController.ClearGridItem();

        List<Work> workList = GameMain.Instance.WorkMgr.GetWorkList();

        for (int k = 0; k < 10; ++k)
        {
            for (int i = 0; i < workList.Count; ++i)
            {
                gridController.listGridItemData.Add(new UI_WorkGridData(workList[i].Kind));
            }
        }
        
        gridController.CreateGridItem();
    }
}
