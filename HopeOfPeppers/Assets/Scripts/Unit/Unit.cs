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

public class Unit {
    private int id;
    private int kind;
    private Stat stat;
    private Stat buffedStat;
    private Vector3 position;
    private Vector3 direction;
    
    public static bool operator ==(Unit aUnit, Unit bUnit)
    {
        return aUnit.id == bUnit.id;
    }
    public static bool operator !=(Unit aUnit, Unit bUnit)
    {
        return aUnit.id != bUnit.id;
    }
    public override bool Equals(object obj)
    {
        var item = obj as Unit;

        if (item == null)
        {
            return false;
        }

        return this.id.Equals(item.id);
    }
    public override int GetHashCode()
    {
        return this.id.GetHashCode();
    }
}


public class UnitManager
{
    private static UnitManager instance = null;
    public Hashtable unitHashTable;

    public static UnitManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UnitManager();
            }
            return instance;
        }
    }

    private UnitManager()
    {
        unitHashTable = new Hashtable();
    }

    public void addUnit(int campId, Unit unit)
    {
        
    }
}
