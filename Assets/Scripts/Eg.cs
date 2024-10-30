//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Eg : MonoBehaviour { }

//public class Node
//{
//    public int X; // �ڵ�� X ����
//    public int Y; // �ڵ�� Y ����
//    public Node Parent; // ���ڵ�
//    public float G; // ����㵽��ǰ�ڵ��ʵ�ʳɱ�
//    public float H; // �ӵ�ǰ�ڵ㵽�յ�Ĺ���ɱ�
//    public float F => G + H; // �ܳɱ�

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
//    public int Size; // ������ܴ�С
//    public int[] Walkable; // �����ߵ�����1��ʾ�����ߣ�0��ʾ��������

//    public Grid(int Width, int Height)
//    {
//        Width = Width;
//        Height = Height;
//        Size = Width * Height;
//        Walkable = new int[Size]; // ��ʼ��Ϊ0���������ߣ�
//    }

//    public bool IsWalkable(int x, int y)
//    {
//        return Walkable[x + y * Width] == 1; // 1��ʾ������
//    }

//    public void SetWalkable(int x, int y, bool value)
//    {
//        Walkable[x + y * Width] = value ? 1 : 0; // ���ÿ�����״̬
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

//        List<Node> openSet = new List<Node>(); // �����Ľڵ�
//        HashSet<Node> closedSet = new HashSet<Node>(); // �Ѽ��Ľڵ�

//        openSet.Add(startNode);

//        while (openSet.Count > 0)
//        {
//            // �ҵ� F ֵ��͵Ľڵ�
//            Node currentNode = openSet[0];
//            foreach (Node node in openSet)
//            {
//                if (node.F < currentNode.F)
//                {
//                    currentNode = node;
//                }
//            }

//            // �������Ŀ��ڵ�
//            if (currentNode.X == targetNode.X && currentNode.Y == targetNode.Y)
//            {
//                return RetracePath(startNode, currentNode);
//            }

//            openSet.Remove(currentNode);
//            closedSet.Add(currentNode);

//            // ������ڽڵ�
//            foreach (Node neighbor in GetNeighbors(currentNode))
//            {
//                if (!grid.IsWalkable(neighbor.X, neighbor.Y) || closedSet.Contains(neighbor))
//                {
//                    continue; // �������߻��Ѽ��
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

//        return null; // û���ҵ�·��
//    }

//    private List<Node> GetNeighbors(Node node)
//    {
//        List<Node> neighbors = new List<Node>();

//        // �ĸ���������ڽڵ�
//        int[,] directions = {
//            { 1, 0 }, // ��
//            { -1, 0 }, // ��
//            { 0, 1 }, // ��
//            { 0, -1 } // ��
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
//        // �����پ���
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
//        path.Reverse(); // ��ת·��
//        return path;
//    }
//}




