using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    public Unit unit;
    UnitBehaviorTree unitBehaviorTree;
    public int campId = 1;
    public int unitKind = 0;
    public int unitLevel = 0;
    protected Animator animator;

    private void Awake()
    {
        unitBehaviorTree = gameObject.AddComponent<UnitBehaviorTree>();

    }

    // Use this for initialization
    void Start()
    {
        if (unit == null) GameObject.Destroy(gameObject);
        unitBehaviorTree.init(unit);
        unitBehaviorTree.startBehaviorTree();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (1f <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            unitBehaviorTree.behaviorProcess();
        }
        switch (unit.condition)
        {
            case (int)UnitContition.DAMAGED:
                animator.SetBool("Run", false);
                animator.SetBool("Att", false);
                animator.SetBool("Hit", true);
                break;
            case (int)UnitContition.NORMAL:
                animator.SetBool("Run", false);
                animator.SetBool("Att", false);
                animator.SetBool("Hit", false);
                break;
            case (int)UnitContition.ATTACK:
                animator.SetBool("Run", false);
                animator.SetBool("Hit", false);
                animator.SetBool("Att", true);
                break;
            case (int)UnitContition.DEAD:
                animator.SetBool("Run", false);
                animator.SetBool("Hit", false);
                animator.SetBool("Att", false);
                animator.SetBool("Death", true);
                break;
            case (int)UnitContition.MOVING:
                animator.SetBool("Att", false);
                animator.SetBool("Hit", false);
                animator.SetBool("Run", true);
                break;
            default:
                break;
        }
        gameObject.transform.localPosition = new Vector3(unit.position.x, unit.position.y, unit.position.y);

        gameObject.transform.localRotation = Quaternion.Euler(0, unit.direction.y, 0);
    }

    public void damageEnemy()
    {
        unit.check = true;
    }
    public void deadUnit()
    {
        //GameObject.Destroy(gameObject);
    }
}