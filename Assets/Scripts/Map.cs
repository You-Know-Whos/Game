using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public int Width;
    public int Height;
    public int[,] walkable;
    public bool isReachable = false;

    public Node start;
    public Node end;

    public List<List<Node>> nodes;
    public GameObject NodePrefab;
    public MapPrefab mapPrefab;



    private void Awake()
    {
        GridLayoutGroup glg = GetComponent<GridLayoutGroup>();
        float MapWidth = GetComponent<RectTransform>().sizeDelta.x;
        float MapHeight = GetComponent<RectTransform>().sizeDelta.y;
        float nodeWidth = (float)((MapWidth - (glg.padding.left + glg.padding.right)) / (Width * 1.1 - 0.1));
        float nodeHeight = (float)((MapHeight - (glg.padding.top + glg.padding.bottom)) / (Height * 1.1 - 0.1));
        glg.cellSize = new Vector2(nodeWidth, nodeHeight);
        glg.spacing = new Vector2(nodeWidth*0.1f, nodeHeight*0.1f);

        //向地图添加node并设置大小
        nodes = new List<List<Node>>(Width + 1);
        nodes.Add(new List<Node>(Height + 1));
        for (int i = 1; i <= Width; i++)
        {
            nodes.Add(new List<Node>(Height + 1));
            nodes[i].Add(null);
            for (int j = 1; j <= Height; j++)
            {
                GameObject nodeUI = Instantiate(NodePrefab);
                nodeUI.transform.SetParent(transform);
                Node node = nodeUI.transform.Find("Node").GetComponent<Node>();
                node.X = i; node.Y = j;
                nodes[i].Add(node);
                node.GetComponent<RectTransform>().localScale = new Vector3(nodeWidth * 0.9f / 100, nodeHeight * 0.9f / 100, 1);
                node.gameObject.SetActive(false);
            }
        }
        //记录障碍物
        walkable = new int[Width + 1, Height + 1];
        for (int i = 1; i <= Width; i++)
        {
            for (int j = 1; j <= Height; j++)
            {
                walkable[i, j] = 1;
            }
        }
        PaintMapPrefab();
    }
    private void OnEnable()
    {
        EventManager.StartAction += DisableBrushButton;
        EventManager.StopAction += EnableBrushButton;
    }
    private void OnDisable()
    {
        EventManager.StartAction -= DisableBrushButton;
        EventManager.StopAction -= EnableBrushButton;
    }
    public void PaintMapPrefab()
    {
        if (mapPrefab != null)
        {
            for (int i = 1; i <= Width; i++)
            {
                for (int j = 1; j <= Height; j++)
                {
                    switch (mapPrefab.row[j - 1].column[i - 1])
                    {
                        case BrushManager.NodeType.None:
                            nodes[i][j].transform.parent.GetComponent<Image>().color = Color.yellow;
                            break;
                        case BrushManager.NodeType.Start:
                            nodes[i][j].transform.parent.GetComponent<Image>().color = Color.green;
                            break;
                        case BrushManager.NodeType.End:
                            nodes[i][j].transform.parent.GetComponent<Image>().color = Color.blue;
                            break;
                        case BrushManager.NodeType.Block:
                            nodes[i][j].transform.parent.GetComponent<Image>().color = Color.black;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    public bool SetStartAndEnd()
    {
        int hasStart = 0, hasEnd = 0;
        for (int i = 1; i <= Width; i++)
        {
            for (int j = 1; j <= Height; j++)
            {
                if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.green)
                {
                    start = nodes[i][j]; hasStart += 1;
                }
                else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.blue)
                {
                    end = nodes[i][j]; hasEnd += 1;
                }
                else if (nodes[i][j].transform.parent.GetComponent<Image>().color == Color.black)
                    walkable[i, j] = 0;
            }
        }
        if (hasStart == 1 && hasEnd == 1)
        {
            return true;
        }
        else
        {
            if (hasEnd > 1)
                print("Too many Ends!!!");
            else if (hasEnd < 1)
                print("The end haven't been set yet!!!");
            if (hasStart > 1)
                print("Too many starts!!!");
            else if (hasStart < 1)
                print("The start haven't been set yet!!!");
            return false;
        }
    }
    private void DisableBrushButton()
    {
        if (!EventManager.Instance.status)
        {
            for (int i = 1; i <= Width; i++)
            {
                for (int j = 1; j <= Height; j++)
                {
                    nodes[i][j].transform.parent.GetComponent<Button>().enabled = false;
                }
            }
        }
    }
    private void EnableBrushButton()
    {
        if (!EventManager.Instance.status)
        {
            for (int i = 1; i <= Width; i++)
            {
                for (int j = 1; j <= Height; j++)
                {
                    nodes[i][j].transform.parent.GetComponent<Button>().enabled = true;
                }
            }
        }
    }
}