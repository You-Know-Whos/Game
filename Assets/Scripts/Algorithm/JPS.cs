using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JPS : MonoBehaviour
{
    private Map map;
    private List<List<Node>> nodes;

    private HashSet<Node> jumpPoints = new HashSet<Node>();
    private HashSet<Node> openSet = new HashSet<Node>();
    private HashSet<Node> closedSet = new HashSet<Node>();
    public Node start;
    public Node end;
    private Vector2Int[] dirs = new Vector2Int[] { Vector2Int.zero.Settt(-1, -1), Vector2Int.zero.Settt(1, -1),//上
                                                   Vector2Int.zero.Settt(-1, 1), Vector2Int.zero.Settt(1, 1) };//下
    


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
                map.status = 1;
                start.gameObject.SetActive(true);
                end.gameObject.SetActive(true);
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
        jumpPoints.Add(start);
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            foreach (var node in openSet)
            {
                foreach (var dir in dirs)
                {
                    Jump(node, node, dir);
                    //yield return new WaitForSeconds(0.2f);
                    yield return null;
                }
                closedSet.Add(node);
            }
            openSet.Clear();
            foreach (var node in jumpPoints)
            {
                openSet.Add(node);
            }
            foreach (var node in closedSet)
                if (openSet.Contains(node))
                    openSet.Remove(node);
        }
        print(end.reachablePath.Count);
        if (!map.isReachable)
            print("NoPath!!!");
        else
            print("Over");
    }
    private void Jump(Node nodeParent, Node node, Vector2Int dir)
    {
        //Notice：判断方式有问题
        if (!node.IsChecked && (IsJumpPoint(node, dir.Settt(0, dir.y)) || IsJumpPoint(node, dir.Settt(dir.x, 0))))
        {
            if (node.X == end.X && node.Y == end.Y)
            {
                if (!end.reachablePath.Contains(nodeParent))
                    end.reachablePath.Add(nodeParent);
                print(nodeParent.X + ", " + nodeParent.Y);
                nodeParent.GetComponent<Image>().color = Color.cyan;
                map.isReachable = true;
                return;
            }
            node.gameObject.SetActive(true);
            node.transform.parent.GetComponent<Image>().color = Color.yellow;
            jumpPoints.Add(node);
            if (node.Parent == null)
                node.Parent = nodeParent;
        }
        node.IsChecked = true;

        StraightJump(node, node, dir.Settt(0, dir.y));
        StraightJump(node, node, dir.Settt(dir.x, 0));

        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsWalkable(x, y) && !nodes[x][y].IsChecked)
            Jump(nodeParent, nodes[x][y], dir);
    }
    private void StraightJump(Node nodeParent, Node node, Vector2Int dir)
    {
        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsWalkable(x, y) && !nodes[x][y].IsChecked)
        {
            nodes[x][y].transform.parent.GetComponent<Image>().color = Color.gray;
            nodes[x][y].IsChecked = true;
            if (IsJumpPoint(nodes[x][y], dir))
            {
                if (x == end.X && y == end.Y)
                {
                    if (!end.reachablePath.Contains(nodeParent))
                        end.reachablePath.Add(nodeParent);
                    print(nodeParent.X + ", " + nodeParent.Y);
                    nodeParent.gameObject.SetActive(true);
                    nodeParent.transform.parent.GetComponent<Image>().color = Color.yellow;
                    nodeParent.GetComponent<Image>().color = Color.red;
                    map.isReachable = true;
                    return;
                }
                nodes[x][y].gameObject.SetActive(true);
                nodes[x][y].transform.parent.GetComponent<Image>().color = Color.yellow;
                jumpPoints.Add(nodeParent);
                jumpPoints.Add(nodes[x][y]);
                if (nodes[x][y].Parent == null)
                    nodes[x][y].Parent = nodeParent;
            }
            else
                StraightJump(nodeParent, nodes[x][y], dir);
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
    private bool IsJumpPoint(Node node, Vector2Int dir)
    {
        return (
                (!IsWalkable(node.X + dir.y, node.Y + dir.x) && IsWalkable(node.X + dir.x + dir.y, node.Y + dir.y + dir.x))
                ||(!IsWalkable(node.X - dir.y, node.Y - dir.x) && IsWalkable(node.X + dir.x - dir.y, node.Y + dir.y - dir.x))
               )
               ||(node.X == end.X && node.Y == end.Y);
    }
}