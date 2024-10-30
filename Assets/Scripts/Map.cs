using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public int Width;
    public int Height;
    public List<List<Node>> nodes;
    public GameObject NodePrefab;



    private void Start()
    {
        //≥ı ºªØµÿÕº
        nodes = new List<List<Node>>(Width + 1);
        nodes.Add(new List<Node>(Height + 1));
        for (int i = 1; i <= Width; i++)
        {
            nodes.Add(new List<Node>(Height + 1));
            nodes[i].Add(null);
            for (int j = 1; j <= Height; j++)
            {
                GameObject nodeUI = Instantiate(NodePrefab);
                nodeUI.transform.SetParent(transform);
                Node node = nodeUI.transform.Find("Node").GetComponent<Node>();
                node.X = i; node.Y = j;
                nodes[i].Add(node);
                node.gameObject.SetActive(false);
            }
        }
    }
}
