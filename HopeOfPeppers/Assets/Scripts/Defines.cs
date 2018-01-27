using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DEFINE
{
    public const int SECOND_PER_MINUTE = 60;
    public const int SECOND_PER_HOUR = SECOND_PER_MINUTE * 60;
    public const int SECOND_PER_DAY = SECOND_PER_HOUR * 24;
    public const int SECOND_PER_YEAR = SECOND_PER_DAY * 365;

    public const int UNIT_MAX_LEVEL = 10;

}

public enum UnitContition : int
{
    NORMAL = 0,
    ATTACK = 1,
    DAMAGED = 2,
    MOVING = 3,
    DEAD = 4
}