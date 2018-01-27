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
    public int id;
    public int kind;
    public Stat stat;
    public Stat buffedStat;
    public Vector3 position;
    public Vector3 direction;
    public int condition;
    
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

    public void init()
    {

    }
}


public class UnitManager
{
    private static UnitManager _instance = null;
    public Hashtable unitHashTable;

    public static UnitManager instance
    {
        get
        {
            if (instance == null)
            {
                _instance = new UnitManager();
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
        if (unitHashTable.ContainsKey(campId))
        {
            (unitHashTable[campId] as List<Unit>).Add(unit);
        }
        else
        {
            List<Unit> tmp = new List<Unit>();
            tmp.Add(unit);

            unitHashTable.Add(campId, tmp); 
        }
    }
    
    public bool isEnemy(Unit unit)
    {
        return true;
    }

    public bool attackEnemy(Unit unit)
    {
        return true;
    }

    public bool moveToEnemy(Unit unit)
    {
        return true;
    }

    public bool isUnitDead(Unit unit)
    {
        return true;
    }

    public bool DeadProcess(Unit unit)
    {
        return true;
    }
}
