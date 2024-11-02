using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    public static BrushManager Instance { get; private set; }
    public enum NodeType
    {
        None,
        Start,
        End,
        Block,
    }
    public NodeType nodeType;
    public Dictionary<NodeType, Color> NodeTypeDict;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Instance = null;
        }
        Instance = this;
    }
    private void Start()
    {
        NodeTypeDict = new Dictionary<NodeType, Color>();
        NodeTypeDict.Add(NodeType.None, Color.yellow);
        NodeTypeDict.Add(NodeType.Start, Color.green);
        NodeTypeDict.Add(NodeType.End, Color.blue);
        NodeTypeDict.Add(NodeType.Block, Color.black);
    }
}
