using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour {
    public Unit unit;
    UnitBehaviorTree unitBehaviorTree;
    public int campId = 1;
    public int unitKind = 0;
    public int unitLevel = 0;

    private void Awake()
    {
        unitBehaviorTree = gameObject.AddComponent<UnitBehaviorTree>();

    }

    // Use this for initialization
    void Start () {
        
        if (unit == null) GameObject.Destroy(gameObject);
        unitBehaviorTree.init(unit);
        unitBehaviorTree.startBehaviorTree();
	}
	
	// Update is called once per frame
	void Update () {
        unitBehaviorTree.behaviorProcess();
        gameObject.transform.localPosition = unit.position;

        if(unit.condition == (int)UnitContition.DEAD)
        {
            GameObject.Destroy(gameObject);
        }
		
	}
}
