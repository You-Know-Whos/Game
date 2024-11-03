//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Eg : MonoBehaviour
//{
//}
//public class Grid
//{
//    public Node[,] Nodes;
//    public int Width;
//    public int Height;

//    public Grid(int width, int height)
//    {
//        Width = width;
//        Height = height;
//        Nodes = new Node[width, height];

//        for (int x = 0; x < width; x++)
//        {
//            for (int y = 0; y < height; y++)
//            {
//                Nodes[x, y] = new Node();
//                Nodes[x, y].X = x;
//                Nodes[x, y].Y = y;
//                Nodes[x, y].IsNotBlock = true;//Ĭ�����нڵ����
//            }
//        }
//    }

//    public void SetWalkable(int x, int y, bool isWalkable)
//    {
//        Nodes[x, y].IsNotBlock = isWalkable;
//    }

//    public List<Node> GetNeighbors(Node node)
//    {
//        List<Node> neighbors = new List<Node>();

//        for (int dx = -1; dx <= 1; dx++)
//        {
//            for (int dy = -1; dy <= 1; dy++)
//            {
//                if (dx == 0 && dy == 0) continue; // ��������
//                if (Math.Abs(dx) + Math.Abs(dy) != 1) continue; // ֻ������������

//                int newX = node.X + dx;
//                int newY = node.Y + dy;

//                if (IsInBounds(newX, newY))
//                {
//                    neighbors.Add(Nodes[newX, newY]);
//                }
//            }
//        }

//        return neighbors;
//    }

//    public bool IsInBounds(int x, int y)
//    {
//        return x >= 0 && x < Width && y >= 0 && y < Height;
//    }
//}

//public class Jps
//{
//    private Grid grid;
//    public Jps(Grid grid)
//    {
//        this.grid = grid;
//    }
//    public List<Node> FindPath(Node start, Node goal)
//    {
//        List<Node> path = new List<Node>();
//        List<Node> jumpPoints = Jump(start, goal);

//        foreach (Node jumpPoint in jumpPoints)
//        {
//            path.Add(jumpPoint);
//        }

//        return path;
//    }
//    private List<Node> Jump(Node current, Node goal)
//    {
//        List<Node> jumpPoints = new List<Node>();

//        // �ӵ�ǰ�ڵ㿪ʼ��Ծ��ֱ���ﵽĿ��
//        while (current != null)
//        {
//            if (current.X == goal.X && current.Y == goal.Y)
//            {
//                jumpPoints.Add(current);
//                return jumpPoints;
//            }

//            // ��Ծ���ھӽڵ�
//            current = GetNextJumpPoint(current, goal);
//            if (current != null)
//            {
//                jumpPoints.Add(current);
//            }
//        }

//        return jumpPoints;
//    }
//    private Node GetNextJumpPoint(Node current, Node goal)
//    {
//        // ���ĸ���������Ծ
//        foreach (var direction in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
//        {
//            Node next = JumpInDirection(current, direction, goal);
//            if (next != null)
//            {
//                return next;
//            }
//        }

//        return null;
//    }
//    private Node JumpInDirection(Node current, Vector2Int direction, Node goal)
//    {
//        int x = current.X;
//        int y = current.Y;

//        while (grid.IsInBounds(x, y) && grid.Nodes[x, y].IsNotBlock)
//        {
//            // ����Ƿ񵽴�Ŀ��
//            if (x == goal.X && y == goal.Y)
//            {
//                return grid.Nodes[x, y];
//            }

//            // ����Ƿ���Ҫ��Ծ�������ڵ�
//            if (HasForcedNeighbor(x, y))
//            {
//                return grid.Nodes[x, y];
//            }

//            x += direction.x;
//            y += direction.y;
//        }

//        return null;
//    }
//    private bool HasForcedNeighbor(int x, int y)
//    {
//        // ����Ƿ���ǿ���ھ�
//        for (int dx = -1; dx <= 1; dx++)
//        {
//            for (int dy = -1; dy <= 1; dy++)
//            {
//                if (Math.Abs(dx) + Math.Abs(dy) != 1) continue; // ֻ������������
//                int newX = x + dx;
//                int newY = y + dy;

//                if (grid.IsInBounds(newX, newY) && !grid.Nodes[newX, newY].IsNotBlock)
//                {
//                    // ���Խ����ھ�
//                    if (grid.IsInBounds(x + dx, y) && grid.IsInBounds(x, y + dy))
//                    {
//                        if (grid.Nodes[x + dx, y].IsNotBlock || grid.Nodes[x, y + dy].IsNotBlock)
//                        {
//                            return true; // ��ǿ���ھ�
//                        }
//                    }
//                }
//            }
//        }
//        return false;
//    }
//}

