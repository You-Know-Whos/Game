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
    public Node start;
    public Node end;



    //TODO：做UI，重新开始，保存按钮，跳过键，设置文本的显示，
    //      JPS每个节点多个祖先，JPS找点，Dijkstra找多个终点的路径（可行性待定），设置箭头图标（增加后，点击跳点显示父节点，防止路径重合）
    //      协程位置改一下，加入jumppoints的几步如果相同就写成函数，动态地图，dijkstra
    private void OnEnable()
    {
        EventManager.goAction += OnGo;
    }
    private void Start()
    {
        map = GetComponent<Map>();
        nodes = map.nodes;
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
                    if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.green) {
                        start = nodes[i][j]; isStart += 1; }
                    else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.blue) {
                        end = nodes[i][j]; isEnd += 1; }
                    else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.black)
                        map.walkable[i, j] = 0;
                }
            }
            if (isStart == 1 && isEnd == 1)
            {
                map.status = 1;

                start.H = (Mathf.Abs(start.X - end.X) + Mathf.Abs(start.Y - end.Y)) * 10;
                start.gText.text = start.G.ToString();
                start.hText.text = start.H.ToString();
                start.fText.text = start.F.ToString();
                end.gText.text = end.G.ToString();
                end.hText.text = end.H.ToString();
                end.fText.text = end.F.ToString();
                start.gameObject.SetActive(true);
                end.gameObject.SetActive(true);
                start.transform.Find("AStar").gameObject.SetActive(true);
                end.transform.Find("AStar").gameObject.SetActive(true);
                if (map.mapPrefab != null)
                {
                    for (int i = 1; i <= map.Width; i++)
                    {
                        for (int j = 1; j <= map.Height; j++)
                        {
                            if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.yellow)
                                map.mapPrefab.row[j - 1].column[i - 1] = BrushManager.NodeType.None;
                            else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.green)
                                map.mapPrefab.row[j - 1].column[i - 1] = BrushManager.NodeType.Start;
                            else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.blue)
                                map.mapPrefab.row[j - 1].column[i - 1] = BrushManager.NodeType.End;
                            else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.black)
                                map.mapPrefab.row[j - 1].column[i - 1] = BrushManager.NodeType.Block;
                        }
                    }
                }
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
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            Node min = null;
            foreach (Node node in openSet)
                if (min == null || node.F < min.F)
                    min = node;
            openSet.Remove(min);
            closedSet.Add(min);
            min.GetComponent<Image>().color = Color.gray;
            min.transform.parent.GetComponentInParent<Image>().color = Color.magenta;

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
                    node.transform.Find("AStar").gameObject.SetActive(true);
                    node.transform.parent.GetComponent<Image>().color = Color.red;
                    openSet.Add(node);

                    int newPath;
                    if (Mathf.Abs(node.X - min.X) + Mathf.Abs(node.Y - min.Y) >= 2)
                        newPath = min.G + 14;
                    else
                        newPath = min.G + 10;
                    if (newPath < node.G)
                    {
                        node.G = newPath;
                        node.gText.text = node.G.ToString();
                        node.fText.text = node.F.ToString();
                        node.Parent = min;
                    }
                    yield return new WaitForSeconds(0.02f);
                    yield return new WaitWhile(() => !Input.GetKey(KeyCode.RightArrow));//Lambda
                    node.transform.parent.GetComponent<Image>().color = Color.yellow;
                }
            }
            min.transform.parent.GetComponentInParent<Image>().color = Color.yellow;
        }
        if (!map.isReachable)
            print("NoPath!!!");
        else
            print("Over");
    }
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        int x = node.X; int y = node.Y;
        //下
        SetNeighbors(neighbors, node, x-1, y+1, true);
        SetNeighbors(neighbors, node, x, y+1, false);
        SetNeighbors(neighbors, node, x+1, y+1, true);
        //中
        SetNeighbors(neighbors, node, x-1, y, false);
        SetNeighbors(neighbors, node, x+1, y, false);
        //上
        SetNeighbors(neighbors, node, x-1, y-1, true);
        SetNeighbors(neighbors, node, x, y-1, false);
        SetNeighbors(neighbors, node, x+1, y-1, true);
        return neighbors;

    }
    private void SetNeighbors(List<Node> neighbors, Node node, int x, int y, bool isDiagonal)
    {
        if ( IsInMap(x, y) && IsNotblock(x, y) && IsClosed(x, y)
            && ( !isDiagonal || (isDiagonal && IsDiagonal(node, x, y)) ) )
        {
            neighbors.Add(nodes[x][y]);
            if (nodes[x][y].Parent == null)
                nodes[x][y].Parent = node;
            nodes[x][y].gameObject.SetActive(true);
            nodes[x][y].transform.Find("AStar").gameObject.SetActive(true);
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
    private bool IsDiagonal(Node node, int x, int y)
    {
        return IsNotblock(node.X, y) || IsNotblock(x, node.Y);
    }
    private void RetracePath(Node endNode)
    {
        print(endNode.G);
        Node curNode = endNode;
        while (curNode != null)
        {
            curNode.gameObject.GetComponent<Image>().color = Color.red;
            curNode = curNode.Parent;
        }
        map.isReachable = true;
    }

    private bool IsInMap(int x, int y)
    {
        return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    }
    private bool IsNotblock(int x, int y)
    {
        return map.walkable[x, y] == 1;
    }
    private bool IsClosed(int x, int y)
    {
        return !closedSet.Contains(nodes[x][y]);
    }
}
