using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JPSForPreGenPath : MonoBehaviour
{
    private Map map;
    private List<List<Node>> nodes;

    private HashSet<Node> jumpPoints = new HashSet<Node>();
    private HashSet<Node> openSet = new HashSet<Node>();
    private HashSet<Node> closedSet = new HashSet<Node>();
    private Node start;
    private Node end;
    private float time = 0;



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
        jumpPoints.Clear();
        openSet.Clear();
        closedSet.Clear();
    }
    IEnumerator Go()
    {
        float time1 = time;
        jumpPoints.Add(start);
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            yield return null;
            foreach (var node in openSet)
            {
                Jump(node, node.dir);
                closedSet.Add(node);//跳完一个节点就会把这个节点放入closeSet
            }
            //更新openSet使之=jumpPoints-closedSet
            openSet.Clear();
            foreach (var node in jumpPoints)
            {
                openSet.Add(node);
            }
            foreach (var node in closedSet)
                if (openSet.Contains(node))
                    openSet.Remove(node);
        }
        if (!map.isReachable)
            print("NoPath!!!");
        else
            print("Over");
        print(time - time1);
    }
    private void Jump(Node node, Vector2Int dir)
    {
        if (node.X == end.X && node.Y == end.Y)
        {
            map.isReachable = true;
            return;
        }
        if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2)//斜向
        {
            SlantJump_HasForcedNeighbor(node, dir);
            Node xNode = StraightJump(node, dir.Settt(dir.x, 0));
            Node yNode = StraightJump(node, dir.Settt(0, dir.y));
            if (xNode != null)
            {
                SetJumpPoints(node, xNode, node.dir.Settt(dir.x, 0));
            }
            if (yNode != null)
            {
                SetJumpPoints(node, yNode, node.dir.Settt(0, dir.y));
            }

            int x = node.X + dir.x; int y = node.Y + dir.y;
            if (IsWalkable(x, y))
            {
                Node sltNode = SlantJump(nodes[x][y], dir);
                if (sltNode != null)
                {
                    SetJumpPoints(node, sltNode, dir);
                }
            }
        }
        else if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 1)//直向
        {
            Node sNode = StraightJump(node, dir);
            if (sNode != null)
            {
                SetJumpPoints(node, sNode, dir);
            }
        }
        else //start
        {
            foreach (Vector2Int dirEach in new Vector2Int[]{ Vector2Int.zero.Settt(-1, -1), Vector2Int.down, Vector2Int.zero.Settt(1, -1),//上
                                                           Vector2Int.zero.Settt(-1, 0), Vector2Int.zero.Settt(1, 0),//中
                                                           Vector2Int.zero.Settt(-1, 1), Vector2Int.up, Vector2Int.zero.Settt(1, 1)//下
                                                         })
            {
                Jump(node, dirEach);
            }
        }
        if (node.forcedNeighbor.Count != 0)
        {
            foreach (Node forcedNeighbor in node.forcedNeighbor)
            {
                SetJumpPoints(node, forcedNeighbor, dir.Settt(forcedNeighbor.X - node.X, forcedNeighbor.Y - node.Y));
            }
        }
    }
    private Node SlantJump(Node node, Vector2Int dir)
    {
        if (node.X == end.X && node.Y == end.Y)
        {
            map.isReachable = true;
            return node;
        }
        SlantJump_HasForcedNeighbor(node, dir);
        Node xNode = StraightJump(node, dir.Settt(dir.x, 0));
        Node yNode = StraightJump(node, dir.Settt(0, dir.y));
        if (xNode != null || yNode != null || node.forcedNeighbor.Count != 0)
        {
            return node;
        }
        else
        {
            int x = node.X + dir.x; int y = node.Y + dir.y;
            if (IsWalkable(x, y) && (IsWalkable(node.X, y) || IsWalkable(x, node.Y)))
            {
                return SlantJump(nodes[x][y], dir);
            }
        }
        return null;
    }
    private Node StraightJump(Node node, Vector2Int dir)//注意事项：必须要传直的方向；并不判断起点
    {
        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsWalkable(x, y))
        {
            Node strNode = nodes[x][y];
            if (IsJumpPoint(strNode, dir)/* && jumpPoints.Contains(nodes[x][y])//这里不清楚是不是引用传递所以直接用nodes*/)//这个好像没有用
            {
                if (strNode.X == end.X && strNode.Y == end.Y)
                    map.isReachable = true;
                return strNode;
            }
            else
            {
                return StraightJump(strNode, dir);
            }
        }
        return null;
    }
    private bool IsJumpPoint(Node node, Vector2Int dir)
    {
        //设置时注意重复情况
        if (!IsWalkable(node.X + dir.y, node.Y + dir.x) && IsWalkable(node.X + dir.x + dir.y, node.Y + dir.y + dir.x) && IsWalkable(node.X + dir.x, node.Y + dir.y))
        {
            node.forcedNeighbor.Add(nodes[node.X + dir.x + dir.y][node.Y + dir.y + dir.x]);
        }
        if (!IsWalkable(node.X - dir.y, node.Y - dir.x) && IsWalkable(node.X + dir.x - dir.y, node.Y + dir.y - dir.x) && IsWalkable(node.X + dir.x, node.Y + dir.y))
        {
            node.forcedNeighbor.Add(nodes[node.X + dir.x - dir.y][node.Y + dir.y - dir.x]);
        }
        if (node.forcedNeighbor.Count > 0 || (node.X == end.X && node.Y == end.Y))
        {
            return true;
        }
        else
            return false;
        //return (
        //        (!IsWalkable(node.X + dir.y, node.Y + dir.x) && IsWalkable(node.X + dir.x + dir.y, node.Y + dir.y + dir.x))
        //        ||(!IsWalkable(node.X - dir.y, node.Y - dir.x) && IsWalkable(node.X + dir.x - dir.y, node.Y + dir.y - dir.x))
        //       )
        //        || (node.X == end.X && node.Y == end.Y);
    }
    private void SetJumpPoints(Node node, Node nodeChild, Vector2Int dir)
    {
        nodeChild.dir = dir;
        node.children_J.Add(nodeChild);
        jumpPoints.Add(nodeChild);
        nodeChild.gameObject.SetActive(true);
    }
    private void SlantJump_HasForcedNeighbor(Node node, Vector2Int dir)
    {
        if (!IsWalkable(node.X, node.Y - dir.y) || !IsWalkable(node.X - dir.x, node.Y))//判断该节点有无强迫邻居
        {
            if (!IsWalkable(node.X, node.Y - dir.y) && IsWalkable(node.X + dir.x, node.Y - dir.y) && IsWalkable(node.X + dir.x, node.Y))
            {
                node.forcedNeighbor.Add(nodes[node.X + dir.x][node.Y - dir.y]);
            }
            if (!IsWalkable(node.X - dir.x, node.Y) && IsWalkable(node.X - dir.x, node.Y + dir.y) && IsWalkable(node.X, node.Y + dir.y))
            {
                node.forcedNeighbor.Add(nodes[node.X - dir.x][node.Y + dir.y]);
            }
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
