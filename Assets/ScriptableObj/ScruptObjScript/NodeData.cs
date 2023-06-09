using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "NodeData",menuName = "ScriptableObjects/Node",order = 3)]
public class NodeData : ScriptableObject
{
    public string ownerAddr;
    public int fatherLayer;
    public int fatherIndex;
    public int childCount;

    public int nodeLayer;
    public int nodeIndex;
    
    //basicInfo
    public int curHealth;
    public int fullHealth;
    public int monsterCount;
    public int money;
    public string mapStructure;
    public int[] towerDebuffList = new int[3];
    public int[] towerProtectList = new int[3];
}
