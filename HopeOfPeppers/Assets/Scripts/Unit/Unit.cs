using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int level{get; private set;}
    public int power{get; private set;}
    public int hp{get; private set;}
    public int moveSpeed{get; private set;}
    public int attackSpeed{get; private set;}
    public int range {get; private set;}
    
    public Stat(int aLevel = 0, int aPower = 0, int aHp = 0, int aMoveSpeed = 0, int aAttackSpeed = 0, int aRange = 0)
    {
        level = aLevel;
        power = aPower;
        hp = aHp;
        moveSpeed = aMoveSpeed;
        attackSpeed = aAttackSpeed;
        range = aRange;
    }
}

public class Buff
{

}

public class BuffedStat
{
    private float powerBuff;
    private float hpBuff;
    private float moveSpeedBuff;
    private float attackSpeedBuff;
    private float range;

    public BuffedStat(float aPowerBuff = 0, float aHpBuff = 0, float aMoveSpeedBuff = 0, float aAttackSpeedBuff = 0, float aRange = 0)
    {
        powerBuff = aPowerBuff;
        hpBuff = aHpBuff;
        moveSpeedBuff = aMoveSpeedBuff;
        attackSpeedBuff = aAttackSpeedBuff;
        range = aRange;

    }
    public BuffedStat(BuffedStat un)
    {
        powerBuff = un.powerBuff;
        hpBuff = un.hpBuff;
        moveSpeedBuff = un.moveSpeedBuff;
        attackSpeedBuff = un.attackSpeedBuff;
        range = un.range;
    }

    public static BuffedStat operator+(BuffedStat un1, BuffedStat un2)
    {
        BuffedStat result = new BuffedStat();

        result.powerBuff = un1.powerBuff + un2.powerBuff;
        result.hpBuff = un1.hpBuff + un2.hpBuff;
        result.moveSpeedBuff = un1.moveSpeedBuff + un2.moveSpeedBuff;
        result.attackSpeedBuff = un1.attackSpeedBuff + un2.attackSpeedBuff;
        result.range = un1.range + un2.range;
        
        return result;
    }


}

public class BuffManager
{
    

}

public class Unit {
    private int id;
    private int kind;
    private Vector3 position;
    private Stat stat;
    private BuffedStat buffedStat;
    private BuffManager BuffManager;

    
    

}
