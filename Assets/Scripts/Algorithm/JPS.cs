using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JPS : MonoBehaviour
{
    private Map map;
    private List<List<Node>> nodes;

    private List<Node> jumpPoints = new List<Node>();
    //private HashSet<Node> openSet = new HashSet<Node>();
    private HashSet<Node> closedSet = new HashSet<Node>();
    public Node start;
    public Node end;
    private Vector2Int[] dirs = new Vector2Int[] { Vector2Int.zero.Settt(-1, -1), Vector2Int.zero.Settt(1, -1),//上
                                                   Vector2Int.zero.Settt(-1, 1), Vector2Int.zero.Settt(1, 1) };//下
    //private Vector2Int[] dirs = new Vector2Int[] { Vector2Int.zero.Settt(-1, -1), Vector2Int.down, Vector2Int.zero.Settt(1, -1),//上
    //                                               Vector2Int.left, Vector2Int.right,//中
    //                                               Vector2Int.zero.Settt(-1, 1), Vector2Int.up, Vector2Int.one };//下



    //暂时不用openSet，之后再看需不需要
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

                start.H = (Mathf.Abs(start.X - end.X) + Mathf.Abs(start.Y - end.Y)) * 10;
                start.gText.text = start.G.ToString();
                start.hText.text = start.H.ToString();
                start.fText.text = start.F.ToString();
                end.gText.text = end.G.ToString();
                end.hText.text = end.H.ToString();
                end.fText.text = end.F.ToString();
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
        while (jumpPoints.Count > 0)
        {
            foreach (var node in jumpPoints)
            {
                foreach (var dir in dirs)
                {
                    Jump(node, node, dir);
                }
            }
        }
        yield return null;
    }
    private void Jump(Node nodeParent, Node node, Vector2Int dir)
    {
        StraightJump(nodeParent, node, dir.Settt(0, dir.y));
        StraightJump(nodeParent, node, dir.Settt(dir.x, 0));

        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsInMap(x, y) && IsWalkable(x, y))
        {
            if (IsJumpPoint(node))
            {
                node.Parent = nodeParent;
                jumpPoints.Add(node);
            }
            else
                Jump(nodeParent, nodes[x][y], dir);
        }
    }
    private void StraightJump(Node nodeParent, Node node, Vector2Int dir)
    {
        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsInMap(x, y) && IsWalkable(x, y))
        {
            if (IsJumpPoint(node))
            {
                node.Parent = nodeParent;
                jumpPoints.Add(node);
            }
            else
                StraightJump(nodeParent, nodes[x][y], dir);
        }
    }
    private bool IsInMap(int x, int y)
    {
        return x >= 1 && x <= map.Width && y >= 1 && y <= map.Height;
    }
    private bool IsWalkable(int x, int y)
    {
        return map.walkable[x, y] == 1;
    }
    private bool IsJumpPoint(Node node)
    {


        return true;
    }
    private void IsForcedNeighbor()
    {

    }
}
