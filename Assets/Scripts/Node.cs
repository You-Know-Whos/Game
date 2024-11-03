using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Node Parent;
    public int X;
    public int Y;

    //AStart
    public int G;
    public int H;
    public int F { get { return G + H; } }
    public TextMeshProUGUI gText;
    public TextMeshProUGUI hText;
    public TextMeshProUGUI fText;

    //Dijkstra
    public int D;

    //JPS
    public bool IsChecked = false;
    public List<Node> reachablePath = new List<Node>();


    private void Awake()
    {
        gText = transform.Find("GText").GetComponent<TextMeshProUGUI>();
        hText = transform.Find("HText").GetComponent<TextMeshProUGUI>();
        fText = transform.Find("FText").GetComponent<TextMeshProUGUI>();
    }
}
