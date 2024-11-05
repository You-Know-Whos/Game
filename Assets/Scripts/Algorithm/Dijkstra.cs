using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Dijkstra : MonoBehaviour
{
    private Map map;
    private List<List<Node>> nodes;
    private ButtonManager buttonManager;

    private HashSet<Node> openSet = new HashSet<Node>();
    private HashSet<Node> closedSet = new HashSet<Node>();
    public Node start;
    public Node end;



    private void OnEnable()
    {
        EventManager.goAction += OnGo;
    }
    private void Start()
    {
        map = GetComponent<Map>();
        nodes = map.nodes;
        buttonManager = GetComponent<ButtonManager>();
    }
    private void OnDisable()
    {
        EventManager.goAction -= OnGo;
    }
    private void OnGo()
    {
        if (map.status == 0)
        {
            int isStart = 0, isEnd = 0;
            for (int i = 1; i <= map.Width; i++)
            {
                for (int j = 1; j <= map.Height; j++)
                {
                    if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.green)
                    {
                        start = nodes[i][j]; isStart += 1;
                    }
                    else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.blue)
                    {
                        end = nodes[i][j]; isEnd += 1;
                    }
                    else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.black)
                        map.walkable[i, j] = 0;
                }
            }
            if (isStart == 1 && isEnd == 1)
            {
                buttonManager.OnSaveButtonClick();
                map.status = 1;

                start.gameObject.SetActive(true);
                end.gameObject.SetActive(true);
                
                StartCoroutine(Go());
            }
            else
            {
                if (isEnd > 1)
                    print("Too many Ends!!!");
                else if (isEnd < 1)
                    print("The end haven't been set yet!!!");
                if (isStart > 1)
                    print("Too many starts!!!");
                else if (isEnd < 1)
                    print("The start haven't been set yet!!!");
            }
        }
    }
    IEnumerator Go()
    {
        Init();
        openSet.Add(start);
        while (openSet.Count > 0) 
        {
            Node min = null;
            foreach (Node node in openSet)
                if (min == null || node.D < min.D)
                    min = node;
            List<Node> nodes = GetNeighbors();
            foreach (Node node in nodes)
            {
                int newPath = min.D + 10;
                if (newPath < node.D)
                {

                }
            }
            openSet.Remove(min);
            closedSet.Add(min);




            yield return new WaitForSeconds(0.05f);
        }
        if (!map.isReachable)
            print("NoPath!!!");
        else
            print("Over");
    }

    private void Init()
    {
        openSet.Add(start);
        Node node = null;
        
        if (IsWalkable(node.X - 1, node.Y))
        {
            nodes[node.X][node.Y].Parent = node;
        }
    }
    private void Horizontal(Node node, int x)
    {
        if (IsWalkable(node.X + x, node.Y))
        {
            Node nextNode = nodes[node.X + x][node.Y];
            nextNode.Parent = node;
            Horizontal(nextNode, x);
        }
        Vertical(node, 1);
        Vertical(node, -1);
    }
    private void Vertical(Node node, int y)
    {
        if (IsWalkable(node.X, node.Y + y))
        {
            Node nextNode = nodes[node.X][node.Y + y];
            nextNode.Parent = node;
            Vertical(nextNode, y);
        }
    }
    private bool IsInMap(int x, int y)
    {
        return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    }
    private bool IsNotBlock(int x, int y)
    {
        return map.walkable[x, y] == 1;
    }
    private bool IsWalkable(int x, int y)
    {
        return IsInMap(x, y) && IsNotBlock(x, y);
    }




















    //IEnumerator Go()
    //{
    //    PreGenPathManager.Instance.PreGenPath();

    //    openSet.Add(start);
    //    while (openSet.Count > 0)
    //    {
    //        Node min = null;
    //        foreach (Node node in openSet)
    //            if (min == null || node.D < min.D)
    //                min = node;
    //        openSet.Remove(min);
    //        closedSet.Add(min);
    //        min.gameObject.GetComponent<Image>().color = Color.gray;
    //        min.transform.parent.GetComponentInParent<Image>().color = Color.magenta;

    //        List<Node> neighbors = GetNeighbors(min);


    //        if (min.X == end.X && min.Y == end.Y)
    //        {
    //            RetracePath(min);
    //            yield break;
    //        }

    //        foreach (Node node in neighbors)
    //        {
    //            if (node != null)
    //            {
    //                node.gameObject.SetActive(true);
    //                node.transform.parent.GetComponent<Image>().color = Color.red;

    //                openSet.Add(node);
    //                if (Mathf.Abs(node.X - min.X) + Mathf.Abs(node.Y - min.Y) >= 2)
    //                {
    //                    if (min.D + 14 < node.D)
    //                    {
    //                        node.D = min.D;
    //                        node.Parent = min;
    //                    }
    //                }
    //                else
    //                {
    //                    if (min.D + 10 < node.D)
    //                    {
    //                        node.D = min.D;
    //                        node.Parent = min;
    //                    }
    //                }
    //                yield return new WaitForSeconds(0.01f);
    //                yield return new WaitWhile(() => !Input.GetKey(KeyCode.RightArrow));//Lambda
    //                node.transform.parent.GetComponent<Image>().color = Color.yellow;
    //            }
    //        }
    //        min.transform.parent.GetComponentInParent<Image>().color = Color.yellow;
    //    }
    //    if (!map.isReachable)
    //        print("NoPath!!!");
    //    else
    //        print("Over");
    //}
    //private List<Node> GetNeighbors(Node node)
    //{
    //    List<Node> neighbors = new List<Node>();
    //    int x = node.X; int y = node.Y;
    //    //ÏÂ
    //    SetNeighbors(neighbors, node, x - 1, y + 1);
    //    SetNeighbors(neighbors, node, x, y + 1);
    //    SetNeighbors(neighbors, node, x + 1, y + 1);
    //    //ÖÐ
    //    SetNeighbors(neighbors, node, x - 1, y);
    //    SetNeighbors(neighbors, node, x + 1, y);
    //    //ÉÏ
    //    SetNeighbors(neighbors, node, x - 1, y - 1);
    //    SetNeighbors(neighbors, node, x, y - 1);
    //    SetNeighbors(neighbors, node, x + 1, y - 1);
    //    return neighbors;

    //}
    //private void SetNeighbors(List<Node> neighbors, Node node, int x, int y)
    //{
    //    if (IsInMap(x, y) && IsWalkable(x, y) && IsClosed(x, y))
    //    {
    //        neighbors.Add(nodes[x][y]);
    //        if (nodes[x][y].Parent == null)
    //            nodes[x][y].Parent = node;
    //        nodes[x][y].gameObject.SetActive(true);
    //    }
    //    else neighbors.Add(null);
    //}
    //private bool IsInMap(int x, int y)
    //{
    //    return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    //}
    //private bool IsWalkable(int x, int y)
    //{
    //    return map.walkable[x, y] == 1;
    //}
    //private bool IsClosed(int x, int y)
    //{
    //    return !closedSet.Contains(nodes[x][y]);
    //}
    //private void RetracePath(Node endNode)
    //{
    //    print(endNode.D);
    //    Node curNode = endNode;
    //    while (curNode != null)
    //    {
    //        curNode.gameObject.GetComponent<Image>().color = Color.red;
    //        curNode = curNode.Parent;
    //    }
    //    map.isReachable = true;
    //}
}
