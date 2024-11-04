using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
            yield return null;
            foreach (var node in openSet)
            {
                Jump(node, node.dir);
                //if (node.forcedNeighbor != null)
                //{
                //    Jump(node.forcedNeighbor, node.dir);
                //}




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
        print(end.reachablePath.Count);
        if (!map.isReachable)
            print("NoPath!!!");
        else
            print("Over");
    }
    private void Jump(Node node, Vector2Int dir)
    {
        if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 2)//斜向
        {
            Node xNode = StraightJump(node, dir.Settt(dir.x, 0));
            Node yNode = StraightJump(node, dir.Settt(0, dir.y));
            if (xNode != null)
            {
                xNode.dir = node.dir.Settt(dir.x, 0);
                node.children.Add(xNode);
                jumpPoints.Add(xNode);
            }
            if (yNode != null)
            {
                yNode.dir = node.dir.Settt(0, dir.y);
                node.children.Add(yNode);
                jumpPoints.Add(yNode);
            }

            int x = node.X + dir.x; int y = node.Y + dir.y;
            if (IsWalkable(x, y))
            {
                Node sltNode = SlantJump(nodes[x][y], dir);
                if (sltNode != null)
                {
                    sltNode.dir = dir;
                    node.children.Add(sltNode);
                    jumpPoints.Add(sltNode);
                }
            }
        }
        else if (Mathf.Abs(dir.x) + Mathf.Abs(dir.y) == 1)//直向
        {
            Node sNode = StraightJump(node, dir);
            if (sNode != null)
            {
                sNode.dir = dir;
                node.children.Add(sNode);
                jumpPoints.Add(sNode);
            }
        }
        else //start
        {
            foreach (Vector2Int dirrr in new Vector2Int[]{ Vector2Int.zero.Settt(-1, -1), Vector2Int.zero.Settt(1, -1),//上
                                                   Vector2Int.zero.Settt(-1, 1), Vector2Int.zero.Settt(1, 1) //下
                                                       })
            {

            }
        }
        if (node.forcedNeighbor != null)
        {
            node.forcedNeighbor.dir = dir.Settt(node.forcedNeighbor.X - node.X, node.forcedNeighbor.Y - node.Y);
            node.children.Add(node.forcedNeighbor);
            jumpPoints.Add(node.forcedNeighbor);
        }
    }
    private Node SlantJump(Node node, Vector2Int dir)
    {
        Vector2Int diagonal_01 = new Vector2Int(node.X, node.Y + dir.y);
        Vector2Int diagonal_02 = new Vector2Int(node.X, node.Y - dir.y);
        bool isWalkable01 = IsWalkable(diagonal_01.x, diagonal_01.y);
        bool isWalkable02 = IsWalkable(diagonal_02.x, diagonal_02.y);
        if (!isWalkable01 || !isWalkable02)
        {
            //node.forcedNeighbor = 
        }

        Node xNode = StraightJump(node, dir.Settt(dir.x, 0));
        Node yNode = StraightJump(node, dir.Settt(0, dir.y));
        if (xNode != null || yNode != null)
        {
            return node;
        }
        else
        {
            int x = node.X + dir.x; int y = node.Y + dir.y;
            if (IsWalkable(x, y))
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
            if (IsJumpPoint(strNode, dir) && jumpPoints.Contains(nodes[x][y]))//这里不清楚是不是引用传递所以直接用nodes
            {
                return strNode;
            }
            else
            {
                return StraightJump(strNode, dir);
            }
        }
        return null;
    }














    private void ElderJump(Node nodeParent, Node node, Vector2Int dir)
    {
        //Notice：判断方式有问题
        if (/*!node.IsChecked && */(IsJumpPoint(node, dir.Settt(0, dir.y)) || IsJumpPoint(node, dir.Settt(dir.x, 0))))
        {
            if (node.X == end.X && node.Y == end.Y)
            {
                if (!end.reachablePath.Contains(nodeParent))
                    end.reachablePath.Add(nodeParent);
                //print(nodeParent.X + ", " + nodeParent.Y);
                //nodeParent.GetComponent<Image>().color = Color.cyan;
                //map.isReachable = true;
                //node.IsChecked = false;
                return;
            }
            //node.gameObject.SetActive(true);
            //node.transform.parent.GetComponent<Image>().color = Color.yellow;
            jumpPoints.Add(node);
            if (node.Parent == null)
                node.Parent = nodeParent;
        }
        //node.IsChecked = true;

        ElderStraightJump(node, node, dir.Settt(0, dir.y));
        ElderStraightJump(node, node, dir.Settt(dir.x, 0));

        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsWalkable(x, y)/* && !nodes[x][y].IsChecked*/)
            ElderJump(nodeParent, nodes[x][y], dir);
    }
    private void ElderStraightJump(Node nodeParent, Node node, Vector2Int dir)
    {
        int x = node.X + dir.x; int y = node.Y + dir.y;
        if (IsWalkable(x, y)/* && !nodes[x][y].IsChecked*/)
        {
            //nodes[x][y].transform.parent.GetComponent<Image>().color = Color.gray;
            //nodes[x][y].IsChecked = true;
            if (IsJumpPoint(nodes[x][y], dir))
            {
                //nodeParent.gameObject.SetActive(true);
                //nodeParent.transform.parent.GetComponent<Image>().color = Color.yellow;
                if (x == end.X && y == end.Y)
                {
                    if (!end.reachablePath.Contains(nodeParent))
                        end.reachablePath.Add(nodeParent);
                    print(nodeParent.X + ", " + nodeParent.Y);
                    //nodeParent.GetComponent<Image>().color = Color.red;
                    //map.isReachable = true;
                    //nodes[x][y].IsChecked = false;
                    return;
                }
                //nodes[x][y].gameObject.SetActive(true);
                //nodes[x][y].transform.parent.GetComponent<Image>().color = Color.yellow;
                jumpPoints.Add(nodeParent);
                jumpPoints.Add(nodes[x][y]);
                if (nodes[x][y].Parent == null)
                    nodes[x][y].Parent = nodeParent;
            }
            else ElderStraightJump(nodeParent, nodes[x][y], dir);
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
        //TODO：添加强迫邻居，到达终点
        //逻辑要改；还要设置强迫邻居，设置时注意重复情况，例如障碍物为一个点时；还有多个强迫邻居的情况
        return (
                (!IsWalkable(node.X + dir.y, node.Y + dir.x) && IsWalkable(node.X + dir.x + dir.y, node.Y + dir.y + dir.x))
                ||(!IsWalkable(node.X - dir.y, node.Y - dir.x) && IsWalkable(node.X + dir.x - dir.y, node.Y + dir.y - dir.x))
               )
               ||(node.X == end.X && node.Y == end.Y);
    }
}