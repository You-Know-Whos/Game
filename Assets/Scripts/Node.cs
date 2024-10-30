using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Node Parent;
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public int X;
    public int Y;
    public enum NodeType
    {
        None,
        Start,
        End,
        Block,
    }
    public NodeType nodeType = NodeType.None;
    public Node(int x, int y)
    {
        X = x;
        Y = y;
        G = 0;
        H = 0;
        Parent = null;
    }
    public Node(int x, int y, Node parent)
    {
        X = x;
        Y = y;
        G = 0;
        H = 0;
        Parent = parent;
    }
    public Node(int x, int y, int g, int h)
    {
        X = x;
        Y = y;
        G = g;
        H = h;
        Parent = null;
    }

    public TextMeshProUGUI gText;
    public TextMeshProUGUI hText;
    public TextMeshProUGUI fText;
    private void Awake()
    {
        gText = transform.Find("GText").GetComponent<TextMeshProUGUI>();
        hText = transform.Find("HText").GetComponent<TextMeshProUGUI>();
        fText = transform.Find("FText").GetComponent<TextMeshProUGUI>();
    }
}
