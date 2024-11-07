using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int X;
    public int Y;
    public Node Parent;

    //AStar
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public TextMeshProUGUI gText;
    public TextMeshProUGUI hText;
    public TextMeshProUGUI fText;

    //Dijkstra
    public bool isChecked = false;
    public int D;

    //JPS
    public List<Node> children_J = new List<Node>();
    public List<Node> forcedNeighbor = new List<Node>();
    public Vector2Int dir;
    public int Gj;
    public int Hj;
    public int Fj { get { return Gj + Hj; } }
    public TextMeshProUGUI gjText;
    public TextMeshProUGUI hjText;
    public TextMeshProUGUI fjText;



    private void OnEnable()
    {
        EventManager.StopAction += ResetNode;
        //AStar
        gText = transform.Find("AStar/GText").GetComponent<TextMeshProUGUI>();
        hText = transform.Find("AStar/HText").GetComponent<TextMeshProUGUI>();
        fText = transform.Find("AStar/FText").GetComponent<TextMeshProUGUI>();

        //JPS
        gjText = transform.Find("JPS/GjText").GetComponent<TextMeshProUGUI>();
        hjText = transform.Find("JPS/HjText").GetComponent<TextMeshProUGUI>();
        fjText = transform.Find("JPS/FjText").GetComponent<TextMeshProUGUI>();
    }
    private void OnDisable()
    {
        EventManager.StopAction -= ResetNode;
    }
    private void ResetNode()
    {
        Parent = null;
        //AStar
        //Dijkstra
        isChecked = false;
        //JPS
        children_J = new List<Node>();
        forcedNeighbor = new List<Node>();
        dir = new Vector2Int();
    }
}
