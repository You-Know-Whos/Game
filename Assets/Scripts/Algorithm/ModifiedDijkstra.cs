using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ModifiedDijkstra : MonoBehaviour
{
    private Map map;
    private List<List<Node>> nodes;

    private List<Node> openSet = new List<Node>();
    private Node start;
    private Node end;
    private List<Vector2Int> dirs = new List<Vector2Int>{ Vector2Int.zero.Settt(-1, -1), Vector2Int.down, Vector2Int.zero.Settt(1, -1),//��
                                                          Vector2Int.zero.Settt(-1, 0), Vector2Int.zero.Settt(1, 0),//��
                                                          Vector2Int.zero.Settt(-1, 1), Vector2Int.up, Vector2Int.zero.Settt(1, 1)};//��
    private float time = 0;


    //���ø�����
    private void OnEnable()
    {
        EventManager.StartAction += OnGo;
        EventManager.StopAction += StopGo;
    }
    private void Start()
    {
        map = GetComponent<Map>();
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
    }
    private void OnDisable()
    {
        EventManager.StartAction -= OnGo;
        EventManager.StopAction -= StopGo;
    }
    private void OnGo()
    {
        nodes = map.nodes;
        start = map.start;
        end = map.end;

        start.gameObject.SetActive(true);
        end.gameObject.SetActive(true);

        StartCoroutine(Go());
    }
    private void StopGo()
    {
        StopCoroutine(Go());
        openSet.Clear();
    }
    IEnumerator Go()
    {
        float time1 = time;
        Init();
        openSet.Add(start);
        start.D = 0;
        start.isChecked = true;
        while (openSet.Count > 0)
        {
            Node min = openSet[0];
            if (min.X == end.X && min.Y == end.Y)
            {
                RetracePath(min);
                break;
            }
            min.GetComponent<Image>().color = Color.gray;
            openSet.Remove(min);
            Heapify(openSet.Count, 0);
            GetNeighbors(min);

            //yield return new WaitForSeconds(1f);
            //yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.RightArrow));
            yield return null;
        }
        if (!map.isReachable)
            print("NoPath!!!");
        else
            print("Over");
        print(time - time1);
    }
    private void Init()
    {
        int max = (map.Width + map.Height) * 10;
        for (int i = 1; i <= map.Width; i++)
        {
            for (int j = 1; j <= map.Height; j++)
            {
                nodes[i][j].D = max;
            }
        }
    }
    private void GetNeighbors(Node node)
    {
        foreach (Vector2Int dir in dirs)
        {
            int x = node.X + dir.x; int y = node.Y + dir.y;
            if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2)
            {
                if (IsWalkable(x, y) && nodes[x][y].isChecked == false)
                {
                    if (IsWalkable(x, y - dir.y) || IsWalkable(x - dir.x, y))
                    {
                        nodes[x][y].D = node.D + 14;
                        nodes[x][y].isChecked = true;
                        nodes[x][y].Parent = node;
                        openSet.Add(nodes[x][y]);
                        Heapify(openSet.Count, (openSet.Count - 1) / 2);
                        nodes[x][y].gameObject.SetActive(true);
                    }
                }
            }
            if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 1)
            {
                if (IsWalkable(x, y) && nodes[x][y].isChecked == false)
                {
                    nodes[x][y].D = node.D + 10;
                    nodes[x][y].isChecked = true;
                    nodes[x][y].Parent = node;
                    openSet.Add(nodes[x][y]);
                    Heapify(openSet.Count, (openSet.Count - 1) / 2);
                    nodes[x][y].gameObject.SetActive(true);
                }
            }
        }
    }
    private void RetracePath(Node endNode)
    {
        print("Actual distance = " + endNode.D);
        Node curNode = endNode;
        while (curNode != null)
        {
            curNode.gameObject.GetComponent<Image>().color = Color.red;
            curNode = curNode.Parent;
        }
        map.isReachable = true;
    }

    void swap(int a, int b)
    {
        Node temp = openSet[a];
        openSet[a] = openSet[b];
        openSet[b] = temp;
    }
    void Heapify(int n, int i)
    {
        int min = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;
        if (left < n && openSet[left].D < openSet[min].D)
        {
            min = left;
        }
        if (right < n && openSet[right].D < openSet[min].D)
        {
            min = right;
        }
        if (min != i)
        {
            swap(i, min);
            Heapify(n, min);
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
}