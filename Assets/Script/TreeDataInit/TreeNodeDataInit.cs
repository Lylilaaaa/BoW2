using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TreeNodeDataInit : MonoBehaviour
{
    public static TreeNodeDataInit _instance;
    public TreeData treeData;
    public DownTreeData downTreeData;
    public List<NodeData> previousNodeData;
    public List<DownNodeData> previousDownNodeData;
    public string levelName = "GamePlay";
    private GlobalVar.GameState _previousGameState;
    public bool finish=false;

    private void Awake()
    {
        _instance = this;
        
    }
    private void Start()
    {
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.MainStart)
        {
            previousNodeData = GlobalVar._instance.nodeDataList;
            previousDownNodeData = GlobalVar._instance.downNodeDataList;
            
            treeData.nodeDictionary = new Dictionary<string, NodeData>();
            treeData.InitNodeData = initNodeData(treeData.InitNodeData);
            treeData.nodeDictionary.Add("0,1",treeData.InitNodeData);
            
            downTreeData.downNodeDictionary = new Dictionary<string, DownNodeData>();
            downTreeData.initDownNodeData = initDownNodeData(downTreeData.initDownNodeData);
            downTreeData.downNodeDictionary.Add("0,1",downTreeData.initDownNodeData);
            treeData.treeNodeCount += 1;
            downTreeData.downTreeNodeCount += 1;
            foreach (NodeData _nodeData in previousNodeData)
            {
                treeData.nodeDictionary.Add(_nodeData.nodeLayer.ToString()+','+_nodeData.nodeIndex.ToString(),_nodeData);
                treeData.treeNodeCount += 1;
            }
            foreach (DownNodeData _downNodeData in previousDownNodeData)
            {
                downTreeData.downNodeDictionary.Add(_downNodeData.nodeLayer.ToString()+','+_downNodeData.nodeIndex.ToString(),_downNodeData);
                treeData.treeNodeCount += 1;
            }
            //加node操作
            // fake_preAdd("0,1");
            // fake_preAdd("0,1");
            // fake_preAdd("0,1");
            // SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
            GlobalVar._instance._convert2TreeGen(treeData);
            GlobalVar._instance._downConvert2TreeGen(downTreeData);
            TreeGenerator._instance.InitTree();
            RedTreeGenerator._instance.InitDownTree();
        }
    }

    private void Update()
    {
        if (_previousGameState != GlobalVar.CurrentGameState &&
            GlobalVar.CurrentGameState == GlobalVar.GameState.MainStart && finish == false)
        {
            previousNodeData = GlobalVar._instance.nodeDataList;
            treeData.nodeDictionary = new Dictionary<string, NodeData>();
            treeData.InitNodeData = initNodeData(treeData.InitNodeData);
            treeData.nodeDictionary.Add("0,1",treeData.InitNodeData);
            treeData.treeNodeCount += 1;
            foreach (NodeData _nodeData in previousNodeData)
            {
                treeData.nodeDictionary.Add(_nodeData.nodeLayer.ToString()+','+_nodeData.nodeIndex.ToString(),_nodeData);
                treeData.treeNodeCount += 1;
            }
            GlobalVar._instance._convert2TreeGen(treeData);
            TreeGenerator._instance.InitTree();
            finish = true;
        }
        
        _previousGameState = GlobalVar.CurrentGameState;
        treeData.treeNodeCount = treeData.nodeDictionary.Count;
    }
    private DownNodeData initDownNodeData(DownNodeData initNode)
    {
        initNode.fatherLayer = 0;
        initNode.fatherIndex = 0;
        initNode.childCount = 0;
        initNode.nodeLayer = 0;
        initNode.nodeIndex = 1;
        initNode.debuffData = new int[3];
        initNode.debuffData[0] =initNode.debuffData[1] =initNode.debuffData[2]= 0;
        return initNode;
    }
    private NodeData initNodeData(NodeData initNode)
    {
        initNode.fatherLayer = 0;
        initNode.fatherIndex = 0;
        initNode.childCount = 0;
        initNode.nodeLayer = 0;
        initNode.nodeIndex = 1;
        initNode.fullHealth = 100;
        initNode.curHealth = 100;
        initNode.monsterCount = 2;
        initNode.money = 50;
        initNode.mapStructure = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
        return initNode;
    }
    //先假设预先生成几个node再说,真的加节点需要刷新树
    private void _preAdd(string father)
    {
        if (!treeData.nodeDictionary.ContainsKey(father))
        {
            Debug.Log("Base node not found.");
        }
        else
        {
            int[] layer_index = convertStrInt(father);
            NodeData baseNodeData = treeData.nodeDictionary[father];
            treeData.nodeDictionary[father].childCount += 1;
            //init
            NodeData newNodeData = new NodeData();
            newNodeData.fatherLayer = layer_index[0];
            newNodeData.fatherIndex = layer_index[1];
            newNodeData.childCount = 0;
            
            newNodeData.nodeLayer = layer_index[0]+1;
            newNodeData.nodeIndex = GetMaxSecondNumber(layer_index[0] + 1)+1;
            
            //处理layer升级之后的数据，待处理
            newNodeData.curHealth = baseNodeData.curHealth;
            newNodeData.fullHealth = baseNodeData.fullHealth;
            newNodeData.monsterCount = baseNodeData.monsterCount;
            newNodeData.money = baseNodeData.money;
            newNodeData.mapStructure = baseNodeData.mapStructure;
            string newNodeName = newNodeData.nodeLayer.ToString() + ',' + newNodeData.nodeIndex.ToString();
            
            treeData.nodeDictionary.Add(newNodeName, newNodeData);
            
            // 将新节点保存到文件夹路径中
            // string assetPath = "Assets/ScriptableObj/NodeDataObj/" + newNodeName + ".asset";
            // AssetDatabase.CreateAsset(newNodeData, assetPath);
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();

            Debug.Log("New node created and saved: " + newNodeName);
            treeData.treeNodeCount += 1;
        }
    }
    private int GetMaxSecondNumber(int firstNumber)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in treeData.nodeDictionary.Keys)
        {
            int[] indexPair = convertStrInt(key);
            sequence.Add(indexPair);
        }
        int maxSecondNumber = 0;
        foreach (int[] pair in sequence)
        {
            if (pair[0] == firstNumber && pair[1] > maxSecondNumber)
            {
                maxSecondNumber = pair[1];
            }
        }

        return maxSecondNumber;
    }

    private int[] convertStrInt(string layerIndex)
    {
        List<int> genList = new List<int>();
        string[] parts = layerIndex.Split(',');
        if (parts.Length == 2)
        {
            int layer;
            int index;
            if (int.TryParse(parts[0], out layer) && int.TryParse(parts[1], out index))
            {
                genList.Add(layer);
                genList.Add(index);
                
            }
        }
        else
        {
            Debug.LogError("树的key结构不正确!");
        }
        return genList.ToArray();
    }
    
}
