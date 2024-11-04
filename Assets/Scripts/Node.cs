using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int X;
    public int Y;

    //AStart
    public Node Parent;
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public TextMeshProUGUI gText;
    public TextMeshProUGUI hText;
    public TextMeshProUGUI fText;

    //Dijkstra
    public int D;

    //JPS
    public List<Node> children = new List<Node>();
    public Vector2Int dir;
    public Node forcedNeighbor;

    public bool IsChecked = false;
    public List<Node> reachablePath = new List<Node>();


    private void Awake()
    {
        gText = transform.Find("GText").GetComponent<TextMeshProUGUI>();
        hText = transform.Find("HText").GetComponent<TextMeshProUGUI>();
        fText = transform.Find("FText").GetComponent<TextMeshProUGUI>();
    }
}
