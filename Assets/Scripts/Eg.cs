//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Eg : MonoBehaviour { }

//public class Node
//{
//    public int X; // 节点的 X 坐标
//    public int Y; // 节点的 Y 坐标
//    public Node Parent; // 父节点
//    public float G; // 从起点到当前节点的实际成本
//    public float H; // 从当前节点到终点的估算成本
//    public float F => G + H; // 总成本

//    public Node(int x, int y)
//    {
//        X = x;
//        Y = y;
//        G = 0;
//        H = 0;
//        Parent = null;
//    }
//}
//public class Grid
//{
//    public int Width;
//    public int Height;
//    public int Size; // 网格的总大小
//    public int[] Walkable; // 可行走的区域，1表示可行走，0表示不可行走

//    public Grid(int Width, int Height)
//    {
//        Width = Width;
//        Height = Height;
//        Size = Width * Height;
//        Walkable = new int[Size]; // 初始化为0（不可行走）
//    }

//    public bool IsWalkable(int x, int y)
//    {
//        return Walkable[x + y * Width] == 1; // 1表示可行走
//    }

//    public void SetWalkable(int x, int y, bool value)
//    {
//        Walkable[x + y * Width] = value ? 1 : 0; // 设置可行走状态
//    }
//}
//public class AStarPathfinding
//{
//    private Grid grid;

//    public AStarPathfinding(Grid grid)
//    {
//        this.grid = grid;
//    }

//    public List<(int, int)> FindPath(int startX, int startY, int targetX, int targetY)
//    {
//        Node startNode = new Node(startX, startY);
//        Node targetNode = new Node(targetX, targetY);

//        List<Node> openSet = new List<Node>(); // 待检查的节点
//        HashSet<Node> closedSet = new HashSet<Node>(); // 已检查的节点

//        openSet.Add(startNode);

//        while (openSet.Count > 0)
//        {
//            // 找到 F 值最低的节点
//            Node currentNode = openSet[0];
//            foreach (Node node in openSet)
//            {
//                if (node.F < currentNode.F)
//                {
//                    currentNode = node;
//                }
//            }

//            // 如果到达目标节点
//            if (currentNode.X == targetNode.X && currentNode.Y == targetNode.Y)
//            {
//                return RetracePath(startNode, currentNode);
//            }

//            openSet.Remove(currentNode);
//            closedSet.Add(currentNode);

//            // 检查相邻节点
//            foreach (Node neighbor in GetNeighbors(currentNode))
//            {
//                if (!grid.IsWalkable(neighbor.X, neighbor.Y) || closedSet.Contains(neighbor))
//                {
//                    continue; // 不可行走或已检查
//                }

//                float newCostToNeighbor = currentNode.G + GetDistance(currentNode, neighbor);
//                if (newCostToNeighbor < neighbor.G || !openSet.Contains(neighbor))
//                {
//                    neighbor.G = newCostToNeighbor;
//                    neighbor.H = GetDistance(neighbor, targetNode);
//                    neighbor.Parent = currentNode;

//                    if (!openSet.Contains(neighbor))
//                    {
//                        openSet.Add(neighbor);
//                    }
//                }
//            }
//        }

//        return null; // 没有找到路径
//    }

//    private List<Node> GetNeighbors(Node node)
//    {
//        List<Node> neighbors = new List<Node>();

//        // 四个方向的相邻节点
//        int[,] directions = {
//            { 1, 0 }, // 右
//            { -1, 0 }, // 左
//            { 0, 1 }, // 上
//            { 0, -1 } // 下
//        };

//        for (int i = 0; i < directions.GetLength(0); i++)
//        {
//            int neighborX = node.X + directions[i, 0];
//            int neighborY = node.Y + directions[i, 1];
//            if (IsInBounds(neighborX, neighborY))
//            {
//                neighbors.Add(new Node(neighborX, neighborY));
//            }
//        }

//        return neighbors;
//    }

//    private bool IsInBounds(int x, int y)
//    {
//        return x >= 0 && x < grid.Width && y >= 0 && y < grid.Height;
//    }

//    private float GetDistance(Node a, Node b)
//    {
//        // 曼哈顿距离
//        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
//    }

//    private List<(int, int)> RetracePath(Node startNode, Node endNode)
//    {
//        List<(int, int)> path = new List<(int, int)>();
//        Node currentNode = endNode;

//        while (currentNode != startNode)
//        {
//            path.Add((currentNode.X, currentNode.Y));
//            currentNode = currentNode.Parent;
//        }
//        path.Reverse(); // 反转路径
//        return path;
//    }
//}




