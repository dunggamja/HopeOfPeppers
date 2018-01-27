using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int level;
    public int power;
    public int hp;
    public int remainHp;
    public float moveSpeed;
    public float attackSpeed;
    public float range;
    
    public Stat(int aLevel = 0, int aPower = 0, int aHp = 0, int aRemainHp = 0, int aMoveSpeed = 0, int aAttackSpeed = 0, float aRange = 0)
    {
        level = aLevel;
        power = aPower;
        hp = aHp;
        remainHp = aRemainHp;
        moveSpeed = aMoveSpeed;
        attackSpeed = aAttackSpeed;
        range = aRange;
    }
}

public class Unit {
    public int campId;
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
        condition = (int)UnitContition.NORMAL;

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
            return _instance;
        }
    }

    private UnitManager()
    {
        unitHashTable = new Hashtable();
    }

    public void addUnit(Unit unit)
    {
        if (unitHashTable.ContainsKey(unit.campId))
        {
            (unitHashTable[unit.campId] as List<Unit>).Add(unit);
        }
        else
        {
            List<Unit> tmp = new List<Unit>();
            tmp.Add(unit);

            unitHashTable.Add(unit.campId, tmp); 
        }
    }
    
    public bool isEnemy(Unit unit)
    {
        var iter = unitHashTable.GetEnumerator();
        while (iter.MoveNext())
        {
            if (unit.campId == (int)iter.Key) continue;

            List<Unit> value = iter.Value as List<Unit>;

            foreach(Unit lUnit in value)
            {
                if(unit.stat.range > (unit.position - lUnit.position).magnitude)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool attackEnemy(Unit unit)
    {
        float minDistance = 0.0f;
        Unit closeEnemy = null;

        var iter = unitHashTable.GetEnumerator();
        while (iter.MoveNext())
        {
            if (unit.campId == (int)iter.Key) continue;

            List<Unit> value = iter.Value as List<Unit>;

            foreach(Unit lUnit in value)
            {
                float distance = (unit.position - lUnit.position).magnitude;
                if (unit.stat.range > distance)
                {
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        closeEnemy = lUnit;
                    }
                }
            }
        }

        if (closeEnemy == null) return false;


        // 충돌처리 또는 애니메이션 재생

        unit.condition = (int)UnitContition.ATTACK;
        closeEnemy.condition = (int)UnitContition.DAMAGED;
        closeEnemy.stat.remainHp -= unit.stat.power;

        return true;
    }

    public bool moveToEnemy(Unit unit)
    {
        float minDistance = 0.0f;
        Unit closeEnemy = null;

        var iter = unitHashTable.GetEnumerator();
        while (iter.MoveNext())
        {
            if (unit.campId == (int)iter.Key) continue;

            List<Unit> value = iter.Value as List<Unit>;

            foreach (Unit lUnit in value)
            {
                float distance = (unit.position - lUnit.position).magnitude;
                if (unit.stat.range > distance)
                {
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        closeEnemy = lUnit;
                    }
                }
            }
        }
        if (closeEnemy == null) return false;

        // 포지션 움직이기
        unit.position += (closeEnemy.position - unit.position).normalized*unit.stat.moveSpeed;
        unit.condition = (int)UnitContition.MOVING;


        return true;
    }

    public bool isUnitDead(Unit unit)
    {
        if (unit.stat.remainHp <= 0) return true;

        unit.condition = (int)UnitContition.NORMAL;
        
        return false;
    }

    public bool DeadProcess(Unit unit)
    {
        unit.condition = (int)UnitContition.DEAD;
        // 애니메이션 재생

        // 유닛 삭제
        ((List<Unit>)unitHashTable[unit.campId]).Remove(unit);
        // 버프 대상 삭제
        BuffManager.instance.buffHashTable.Remove(unit);
        return true;
    }

    public Unit CrateUnit(int campId, int kind, int level, Vector3 position)
    {
        Unit unit = new Unit();

        GAMEDATA.DATA.GAMEDATA_UNIT_STAT stat = GAMEDATA.GAMEDATAINFOS.Instance.GetUnitStat(kind, level);
        if (null == stat)
            return null;

        unit.campId = campId;
        unit.kind = kind;
        unit.position = position;
        unit.stat = new Stat(stat.unitLevel
            , stat.unitBaseStat.power
            , stat.unitBaseStat.hp
            , stat.unitBaseStat.hp
            , (int)stat.unitBaseStat.moveSpeed
            , (int)stat.unitBaseStat.attackSpeed
            , stat.unitBaseStat.range);


        addUnit(unit);

        return unit;
    }
}
