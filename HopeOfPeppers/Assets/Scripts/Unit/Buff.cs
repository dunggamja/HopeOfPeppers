using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public int kind;
    public float buffPower;
    public float buffHp;
    public float buffMoveSpeed;
    public float buffAttackSpeed;
    public float buffRange;
    public int buffRemainTime;
}


public class BuffManager
{
    private static BuffManager _instance = null;
    public Hashtable buffHashTable;

    public static BuffManager instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new BuffManager();
            }
            return _instance;
        }
    }

    private BuffManager ()
    {
        buffHashTable = new Hashtable();
    }

    public void addBuff(Unit target, Buff buff)
    {
        if(buffHashTable.ContainsKey(target))
        {
            List<Buff> tmp = buffHashTable[target] as List<Buff>;
            tmp.Add(buff);
        }
        else
        {
            List<Buff> tmp = new List<Buff>();
            tmp.Add(buff);
            buffHashTable.Add(target, tmp);
        }        
    }
    public void calculateBuff()
    {
    }
    public void deleteTarget(Unit target)
    {
        if(buffHashTable.ContainsKey(target))
        {
            buffHashTable.Remove(target);
        }
    }
    
}