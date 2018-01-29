using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : UI_Base
{
    public Button buttonStart = null;
    public Image imageStart = null;

    private void Start()
    {
        buttonStart.onClick.AddListener(Click_Start);
    }


    private void Click_Start()
    {
        //Debug.Log("ClickStart");
        GameMain.Instance.StageMgr.LoadLevel(1);
    }

}
