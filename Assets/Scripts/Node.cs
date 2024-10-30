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
    private void Start()
    {
        gText = transform.Find("GText").GetComponent<TextMeshProUGUI>();
        hText = transform.Find("HText").GetComponent<TextMeshProUGUI>();
        fText = transform.Find("FText").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        gText.text = G.ToString();
        hText.text = H.ToString();
        fText.text = F.ToString();
    }


    //private Image image;
    //public AStar aStar;

    //private void Start()
    //{
    //    image = GetComponent<Image>();
    //    image.color = Color.yellow;
    //}
    //private void Update()
    //{
    //    GText.text = G.ToString();
    //    HText.text = H.ToString();
    //    FText.text = F.ToString();
    //    if (aStar.openSet.Contains(this))
    //        image.color = Color.red;
    //    else if (aStar.closedSet.Contains(this))
    //        image.color = Color.gray;
    //    else if (aStar.min == this)
    //        image.color = Color.green;
    //}
}
