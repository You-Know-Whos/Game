using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

public class AStar : MonoBehaviour
{
    private Map map;

    private List<List<Node>> nodes;
    private HashSet<Node> openSet = new HashSet<Node>();
    private HashSet<Node> closedSet = new HashSet<Node>();
    private Node start;
    private Node end;


    //TODO：设置地图大小，重新开始，错误提示，开始后禁止改动地图，算法有点小问题：设置node的祖先的时间不对
    private void Start()
    {
        map = GetComponent<Map>();
        nodes = map.nodes;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 1; i <= map.Width; i++)
            {
                for (int j = 1; j <= map.Height; j++)
                {
                    if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.green)
                        start = nodes[i][j];
                    else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.blue)
                        end = nodes[i][j];
                    else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.black)
                        map.walkable[i, j] = 0;
                }
            }
            if (start != null && end != null)
            {
                start.H = (Mathf.Abs(start.X - end.X) + Mathf.Abs(start.Y - end.Y)) * 10;
                start.gText.text = start.G.ToString();
                start.hText.text = start.H.ToString();
                start.fText.text = start.F.ToString();
                end.gText.text = end.G.ToString();
                end.hText.text = end.H.ToString();
                end.fText.text = end.F.ToString();
                start.gameObject.SetActive(true);
                end.gameObject.SetActive(true);

                StartCoroutine(Go());
            }
            else print("ERROR!!!");
        }
    }

    IEnumerator Go()
    {
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            Node min = start;
            foreach (Node node in openSet)
            {
                min = node;
                break;
            }
            foreach (Node node in openSet)
                if (node.F < min.F)
                    min = node;
            openSet.Remove(min);
            closedSet.Add(min);
            min.gameObject.GetComponent<Image>().color = Color.gray;

            List<Node> neighbors = GetNeighbors(min);

            if (min.X == end.X && min.Y == end.Y)
            {
                RetracePath(min);
                yield break;
            }
            foreach (Node node in neighbors)
            {
                if (node != null)
                {
                    node.gameObject.SetActive(true);
                    node.transform.parent.GetComponent<Image>().color = Color.red;
                    openSet.Add(node);

                    int newPath;
                    if (Mathf.Abs(node.X - min.X) + Mathf.Abs(node.Y - min.Y) >= 2)
                        newPath = min.G + 14;
                    else
                        newPath = min.G + 10;
                    print(newPath + "\t" + node.G);
                    if (newPath < node.G)
                    {
                        node.G = newPath;
                        node.gText.text = node.G.ToString();
                        node.fText.text = node.F.ToString();
                        node.Parent = min;
                    }
                    yield return new WaitForSeconds(0.5f);
                    node.transform.parent.GetComponent<Image>().color = Color.yellow;
                }
            }
        }
    }
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        int x = node.X; int y = node.Y;
        //下
        SetNeighbors(neighbors, node, x-1, y+1);
        SetNeighbors(neighbors, node, x, y+1);
        SetNeighbors(neighbors, node, x+1, y+1);
        //中
        SetNeighbors(neighbors, node, x-1, y);
        SetNeighbors(neighbors, node, x+1, y);
        //上
        SetNeighbors(neighbors, node, x-1, y-1);
        SetNeighbors(neighbors, node, x, y-1);
        SetNeighbors(neighbors, node, x+1, y-1);
        return neighbors;

    }
    private void SetNeighbors(List<Node> neighbors, Node node, int x, int y)
    {
        if (IsInMap(x, y) && IsWalkable(x, y) && IsClosed(x, y))
        {
            neighbors.Add(nodes[x][y]);
            nodes[x][y].Parent = node;
            nodes[x][y].gameObject.SetActive(true);
            Calculate(nodes[x][y], nodes[x][y].Parent);
        }
        else neighbors.Add(null);
    }
    private void Calculate(Node node, Node parent)
    {
        int dx = Mathf.Abs(node.X - parent.X);
        int dy = Mathf.Abs(node.Y - parent.Y);
        node.H = Mathf.Abs(end.X - node.X) * 10 + Mathf.Abs(end.Y - node.Y) * 10;
        if (dx + dy >= 2)
        {
            node.G = node.Parent.G + 14;
        }
        else
        {
            node.G = node.Parent.G + 10;
        }
        node.hText.text = node.H.ToString();
        node.gText.text = node.G.ToString();
        node.fText.text = node.F.ToString();
    }
    private bool IsInMap(int x, int y)
    {
        return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    }
    private bool IsWalkable(int x, int y)
    {
        return map.walkable[x, y] == 1;
    }
    private bool IsClosed(int x, int y)
    {
        return !closedSet.Contains(nodes[x][y]);
    }
    private void RetracePath(Node endNode)
    {
        Node curNode = endNode;
        while (curNode != null)
        {
            curNode.gameObject.GetComponent<Image>().color = Color.red;
            curNode = curNode.Parent;
        }
    }

}
