using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public Int64 Gold { get; private set; }

    public User()
    {
        Gold = 0;
    }

    public void AddGold(Int64 aAddGold)
    {
        Gold += aAddGold;

        EventNotifySystem.Instance.NotifyEvent(EventNotifySystem.EVENT_ID.GOLD_CHANGE, new Int64[]{ Gold, aAddGold});
    }

    public bool SubstructGold(Int64 aSubstructGold)
    {
        if (Gold - aSubstructGold < 0)
            return false;

        Gold -= aSubstructGold;
        EventNotifySystem.Instance.NotifyEvent(EventNotifySystem.EVENT_ID.GOLD_CHANGE, new Int64[] { Gold, aSubstructGold });
        return true;
    }

    public void SetGold(Int64 aGold)
    {
        Gold = aGold;
        EventNotifySystem.Instance.NotifyEvent(EventNotifySystem.EVENT_ID.GOLD_CHANGE, new Int64[] { Gold, 0 });
    }
}
