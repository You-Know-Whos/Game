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
//                Nodes[x, y].IsNotBlock = true;//默认所有节点可走
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
//                if (dx == 0 && dy == 0) continue; // 忽略自身
//                if (Math.Abs(dx) + Math.Abs(dy) != 1) continue; // 只考虑上下左右

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

//        // 从当前节点开始跳跃，直到达到目标
//        while (current != null)
//        {
//            if (current.X == goal.X && current.Y == goal.Y)
//            {
//                jumpPoints.Add(current);
//                return jumpPoints;
//            }

//            // 跳跃到邻居节点
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
//        // 在四个方向上跳跃
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
//            // 检查是否到达目标
//            if (x == goal.X && y == goal.Y)
//            {
//                return grid.Nodes[x, y];
//            }

//            // 检查是否需要跳跃到其他节点
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
//        // 检查是否有强制邻居
//        for (int dx = -1; dx <= 1; dx++)
//        {
//            for (int dy = -1; dy <= 1; dy++)
//            {
//                if (Math.Abs(dx) + Math.Abs(dy) != 1) continue; // 只考虑上下左右
//                int newX = x + dx;
//                int newY = y + dy;

//                if (grid.IsInBounds(newX, newY) && !grid.Nodes[newX, newY].IsNotBlock)
//                {
//                    // 检查对角线邻居
//                    if (grid.IsInBounds(x + dx, y) && grid.IsInBounds(x, y + dy))
//                    {
//                        if (grid.Nodes[x + dx, y].IsNotBlock || grid.Nodes[x, y + dy].IsNotBlock)
//                        {
//                            return true; // 有强制邻居
//                        }
//                    }
//                }
//            }
//        }
//        return false;
//    }
//}

