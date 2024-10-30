using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{
    private Map map;

    private List<List<Node>> nodes;
    private List<Node> openSet = new List<Node>();
    private HashSet<Node> closedSet = new HashSet<Node>();
    private int[,] walkable = new int[11, 6];



    private void Start()
    {
        map = GetComponent<Map>();
        nodes = map.nodes;

        //设置起点和终点
        Node start = nodes[1][1];
        Node end = nodes[10][5];
        start.H = (Mathf.Abs(start.X - end.X) + Mathf.Abs(start.Y - end.Y)) * 10;
        start.gameObject.SetActive(true);
        end.gameObject.SetActive(true);

        Go(start, end);
    }

    private void Go(Node start, Node end)
    {
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            //if (!openSet[0].gameObject.activeSelf)
            //    openSet[0].gameObject.SetActive(true);
            Node min = openSet[0];
            foreach (Node node in openSet)
                if (node.F < min.F)
                    min = node;
            openSet.Remove(min);
            closedSet.Add(min);
            List<Node> neighbors = GetNeighbors(min, end);

            if (min.X == end.X && min.Y == end.Y)
            {
                RetracePath(min);
                return;
            }
            foreach (Node node in neighbors)
            {
                if (node != null)
                {
                    openSet.Add(node);
                    node.Parent = min;

                    int newPath;
                    if (Mathf.Abs(node.X - min.X) + Mathf.Abs(node.Y - min.Y) >= 2)
                        newPath = min.G + 14;
                    else
                        newPath = min.G + 10;
                    if (newPath < node.G)
                    {
                        node.G = newPath;
                        node.Parent = min;
                    }
                }
            }
        }
    }

    private List<Node> GetNeighbors(Node node, Node end)
    {
        List<Node> neighbors = new List<Node>();
        int x = node.X; int y = node.Y;
        //上
        if (IsInMap(x-1, y+1) && IsWalkable(x-1, y+1) && IsClosed(x-1, y+1)) {
            neighbors.Add(nodes[x - 1][y + 1]);
            Calculate(nodes[x - 1][y + 1], nodes[x - 1][y + 1].Parent, end);
        } else neighbors.Add(null);
        if (IsInMap(x, y+1) && IsWalkable(x, y+1) && IsClosed(x, y+1)) {
            neighbors.Add(nodes[x - 1][y + 1]);
            Calculate(nodes[x - 1][y + 1], nodes[x - 1][y + 1].Parent, end);
        } else neighbors.Add(null);
        if (IsInMap(x+1, y) && IsWalkable(x+1, y) && IsClosed(x+1, y)) {
            neighbors.Add(nodes[x+1][y + 1]);
            Calculate(nodes[x+1][y + 1], nodes[x+1][y + 1].Parent, end);
        } else neighbors.Add(null);
        //中
        if (IsInMap(x-1, y) && IsWalkable(x-1, y) && IsClosed(x-1, y)) {
            neighbors.Add(nodes[x - 1][y]);
            Calculate(nodes[x - 1][y], nodes[x - 1][y].Parent, end);
        } else neighbors.Add(null);
        if (IsInMap(x+1, y) && IsWalkable(x+1, y) && IsClosed(x+1, y)) {
            neighbors.Add(nodes[x+1][y + 1]);
            Calculate(nodes[x+1][y + 1], nodes[x+1][y + 1].Parent, end);
        } else neighbors.Add(null);
        //下
        if (IsInMap(x-1, y-1) && IsWalkable(x-1, y-1) && IsClosed(x-1, y-1)) {
            neighbors.Add(nodes[x - 1][y-1]);
            Calculate(nodes[x - 1][y-1], nodes[x - 1][y-1].Parent, end);
        } else neighbors.Add(null);
        if (IsInMap(x, y-1) && IsWalkable(x, y-1) && IsClosed(x, y-1)) {
            neighbors.Add(nodes[x][y-1]);
            Calculate(nodes[x][y-1], nodes[x][y-1].Parent, end);
        } else neighbors.Add(null);
        if (IsInMap(x+1, y-1) && IsWalkable(x+1, y-1) && IsClosed(x+1, y-1)) {
            neighbors.Add(nodes[x+1][y-1]);
            Calculate(nodes[x+1][y-1], nodes[x+1][y-1].Parent, end);
        } else neighbors.Add(null);

        
        //int x = node.X, y = node.Y, g = node.G;
        //neighbors.Add(new Node(x - 1, y + 1, g + 14, (Mathf.Abs(x-1 - end.X) + Mathf.Abs(y+1 - end.Y)) * 10));
        //neighbors.Add(new Node(x, y + 1, g + 10, (Mathf.Abs(x - end.X) + Mathf.Abs(y+1 - end.Y)) * 10));
        //neighbors.Add(new Node(x + 1, y + 1, g + 14, (Mathf.Abs(x+1 - end.X) + Mathf.Abs(y+1 - end.Y)) * 10));
        //neighbors.Add(new Node(x - 1, y, g + 10, (Mathf.Abs(x-1 - end.X) + Mathf.Abs(y - end.Y)) * 10));
        //neighbors.Add(new Node(x + 1, y, g + 10, (Mathf.Abs(x+1 - end.X) + Mathf.Abs(y - end.Y)) * 10));
        //neighbors.Add(new Node(x - 1, y - 1, g + 14, (Mathf.Abs(x-1 - end.X) + Mathf.Abs(y-1 - end.Y)) * 10));
        //neighbors.Add(new Node(x, y - 1, g + 10, (Mathf.Abs(x - end.X) + Mathf.Abs(y-1 - end.Y)) * 10));
        //neighbors.Add(new Node(x + 1, y - 1, g + 14, (Mathf.Abs(x+1 - end.X) + Mathf.Abs(y-1 - end.Y)) * 10));
        return neighbors;
    }
    private void Calculate(Node node, Node parent, Node end)
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
    }
    private bool IsInMap(int x, int y)
    {
        return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    }
    private bool IsWalkable(int x, int y)
    {
        return walkable[x, y] == 1;
    }
    private bool IsClosed(int x, int y)
    {
        return closedSet.Contains(nodes[x][y]);
    }
    private void RetracePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        Node curNode = endNode;
        while (curNode.Parent != null)
        {
            path.Add(curNode);
            curNode = curNode.Parent;
        }
    }

    }