using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int X;
    public int Y;

    //AStar
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
    public List<Node> forcedNeighbor = new List<Node>();
    public Vector2Int dir;



    private void OnEnable()
    {
        //AStar
        gText = transform.Find("AStar/GText").GetComponent<TextMeshProUGUI>();
        hText = transform.Find("AStar/HText").GetComponent<TextMeshProUGUI>();
        fText = transform.Find("AStar/FText").GetComponent<TextMeshProUGUI>();
    }
}
