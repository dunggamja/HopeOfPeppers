﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameMain : MonoBehaviour
{
    public static GameMain Instance { get; private set; }

      
    public TimeManager TimeMgr { get; private set; }
    public WorkManager WorkMgr { get; private set; }
    public StageManager StageMgr { get; private set; }
    public User         UserInfo { get; private set; }


    public delegate void UpdateTime(float aDeltaTime);
    public UpdateTime scaledTimeUpdate;
    public UpdateTime unscaledTimeUpdate;


    public delegate IEnumerator delegateCoroutine();
    public delegateCoroutine GameMainCoroutine;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GameObject.Find("ScreenCanvas"));
        DontDestroyOnLoad(GameObject.Find("EventSystem"));

        if (null == TimeMgr)
            TimeMgr = new TimeManager();

        if (null == WorkMgr)
            WorkMgr = new WorkManager();

        if (null == StageMgr)
            StageMgr = new StageManager();

        if (null == UserInfo)
            UserInfo = new User();

        scaledTimeUpdate += WorkMgr.Update;
        scaledTimeUpdate += StageMgr.Update;
        unscaledTimeUpdate += TimeMgr.Update;        
    }


    public void Start()
    {
        UIManager.Instance.OpenUI<StartUI>();
    }

    public void OnDestroy()
    {
        scaledTimeUpdate -= WorkMgr.Update;
        scaledTimeUpdate -= StageMgr.Update;
        unscaledTimeUpdate -= TimeMgr.Update;

        TimeMgr = null;
        WorkMgr = null;
        StageMgr = null;
    }

    private void Update()
    {
        scaledTimeUpdate(UnityEngine.Time.deltaTime);
        unscaledTimeUpdate(UnityEngine.Time.unscaledDeltaTime);
    }


    public void SaveData()
    {

    }

    public void LoadData()
    {

    }

    [ContextMenu("tstLoadlevel")]
    private void LoadLevelTest()
    {
        StageMgr.LoadLevel(1);
    }
}
