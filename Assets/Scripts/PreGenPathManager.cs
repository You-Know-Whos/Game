using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PreGenPathManager : MonoBehaviour
{
    public static PreGenPathManager Instance { get; private set; }

    private Map map;
    private List<List<Node>> nodes;

    private List<Node> preGenPathNodes = new List<Node>();
    private int factor = 10;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }
        Instance = this;
    }
    private void Start()
    {
        map = GameObject.Find("Canvas/Map").GetComponent<Map>();
        nodes = map.nodes;
    }
    public void PreGenPath()
    {
        PreGenPathNodes();
        Node otherNode = preGenPathNodes[0];
        foreach (Node node in preGenPathNodes)
        {
            node.Parent = otherNode;
            otherNode = node;
        }
    }
    public void PreGenPathNodes()
    {
        print("111");
        for (int i = 1; i < map.Width; i++)
        {
            for (int j = 1; j < map.Height; j++)
            {
                if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.yellow)
                {
                    int blockNeighborCount = 0;
                    //ÏÂ
                    if (!IsInMap(i - 1, j + 1) || !IsWalkable(i - 1, j + 1))
                        blockNeighborCount++;
                    if (!IsInMap(i, j + 1) || !IsWalkable(i, j + 1))
                        blockNeighborCount++;
                    if (!IsInMap(i + 1, j + 1) || !IsWalkable(i + 1, j + 1))
                        blockNeighborCount++;
                    //ÖÐ
                    if (!IsInMap(i - 1, j) || !IsWalkable(i - 1, j))
                        blockNeighborCount++;
                    if (!IsInMap(i + 1, j) || !IsWalkable(i + 1, j))
                        blockNeighborCount++;
                    //ÉÏ
                    if (!IsInMap(i - 1, j - 1) || !IsWalkable(i - 1, j - 1))
                        blockNeighborCount++;
                    if (!IsInMap(i, j - 1) || !IsWalkable(i, j - 1))
                        blockNeighborCount++;
                    if (!IsInMap(i + 1, j - 1) || !IsWalkable(i + 1, j - 1))
                        blockNeighborCount++;

                    if (blockNeighborCount <= 6)
                    {
                        int rate = Random.Range(0 + blockNeighborCount * factor, 100);
                        print(rate);
                        if (rate >= 80)
                        {
                            preGenPathNodes.Add(nodes[i][j]);
                            nodes[i][j].transform.parent.GetComponent<Image>().color = Color.cyan;
                        }
                    }
                }
                else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.green)
                    preGenPathNodes.Add(nodes[i][j]);
                else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.blue)
                    preGenPathNodes.Add(nodes[i][j]);
            }
        }
    }
    private bool IsInMap(int x, int y)
    {
        return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    }
    private bool IsWalkable(int x, int y)
    {
        return map.walkable[x, y] == 1;
    }
}
