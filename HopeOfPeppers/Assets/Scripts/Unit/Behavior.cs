using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Node
{
    public abstract bool Invoke();
}

public class CompositeNode : Node
{
    public override bool Invoke()
    {
        throw new NotImplementedException();
    }

    public void AddChild(Node node)
    {
        childrens.Add(node);
    }

    public List<Node> GetChildrens()
    {
        return childrens;
    }
    private List<Node> childrens = new List<Node>();
}

public class Selector : CompositeNode
{
    public override bool Invoke()
    {
        foreach (var node in GetChildrens())
        {
            if (node.Invoke())
            {
                return true;
            }
        }
        return false;
    }
}

public class Sequence : CompositeNode
{
    public override bool Invoke()
    {
        foreach (var node in GetChildrens())
        {
            if (!node.Invoke())
            {
                return false;
            }
        }
        return true;
    }
}

public class IsEnemy : Node
{
    public Unit unit;

    public override bool Invoke()
    {
        return UnitManager.instance.isEnemy(unit);
    }
}

public class AttackEnemy : Node
{
    public Unit unit;

    public override bool Invoke()
    {
        return UnitManager.instance.attackEnemy(unit);
    }
}

public class MoveToEnemy : Node
{
    public Unit unit;

    public override bool Invoke()
    {
        return UnitManager.instance.moveToEnemy(unit);
    }
}

public class IsUnitDead : Node
{
    public Unit unit;

    public override bool Invoke()
    {
        return UnitManager.instance.isUnitDead(unit);
    }
}

public class DeadProcess : Node
{
    public Unit unit;

    public override bool Invoke()
    {
        return UnitManager.instance.DeadProcess(unit);
    }
}

public abstract class BehaviorTreeBase : MonoBehaviour
{
    public abstract void init(Unit unit);
    public abstract void startBehaviorTree();
    public abstract void stopBehaviorTree();
    public abstract IEnumerator behaviorProcess();
}

public class UnitBehaviorTree : BehaviorTreeBase
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence seqAttack = new Sequence();
    private Sequence seqMove = new Sequence();
    private Sequence seqDead = new Sequence();

    private IsEnemy isEnemy = new IsEnemy();
    private AttackEnemy attackEnemy = new AttackEnemy();
    private MoveToEnemy moveToEnemy = new MoveToEnemy();
    private IsUnitDead isUnitDead = new IsUnitDead();
    private DeadProcess deadProcess = new DeadProcess();

    private IEnumerator _behaviorProcess;
    private Unit unit;

    public override void init(Unit aUnit)
    {
        Unit unit = aUnit;

        root.AddChild(selector);
        root.AddChild(seqDead);

        selector.AddChild(seqAttack);
        selector.AddChild(seqMove);

        isEnemy.unit = unit;
        attackEnemy.unit = unit;
        moveToEnemy.unit = unit;
        isUnitDead.unit = unit;
        deadProcess.unit = unit;

        seqAttack.AddChild(isEnemy);
        seqAttack.AddChild(attackEnemy);
        seqMove.AddChild(moveToEnemy);
        seqDead.AddChild(isUnitDead);
        seqDead.AddChild(deadProcess);

        _behaviorProcess = behaviorProcess();
    }

    public override void startBehaviorTree()
    {
        StartCoroutine(_behaviorProcess);
    }
    public override void stopBehaviorTree()
    {
        StopCoroutine(_behaviorProcess);
    }
    public override IEnumerator behaviorProcess()
    {
        while (!root.Invoke())
        {
            yield return null;
        }
        Debug.Log("behavior process exit");
    }

}