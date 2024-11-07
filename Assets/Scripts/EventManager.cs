using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    //public delegate void StartAction();
    //public static event StartAction StartAction;
    //事件系统
    public static event Action StartAction;
    public static event Action StopAction;
    //按钮
    public Button saveButton;
    public Button startButton;
    public Button againButton;
    public Button clearButton;
    //引用
    private Map map;
    private List<List<Node>> nodes;
    //成员变量
    public bool status = false;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            Instance = null;
        }
        Instance = this;
    }
    private void Start()
    {
        map = GameObject.Find("Canvas/Map").GetComponent<Map>();
        nodes = map.nodes;

        saveButton = GameObject.Find("Canvas/UI/Save").GetComponent<Button>();
        startButton = GameObject.Find("Canvas/UI/Start").GetComponent<Button>();
        againButton = GameObject.Find("Canvas/UI/Again").GetComponent<Button>();
        clearButton = GameObject.Find("Canvas/UI/Clear").GetComponent<Button>();
        saveButton.onClick.AddListener(this.OnSaveButtonClick);
        startButton.onClick.AddListener(this.OnStartButtonClick);
        againButton.onClick.AddListener(this.OnAgainButtonClick);
        clearButton.onClick.AddListener(this.OnClearButtonClick);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnStartButtonClick();
        }
    }
    public void OnSaveButtonClick()
    {
        if (!status)
        {
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
                print("The map is saved!");
            }
        }
    }
    public void OnStartButtonClick()
    {
        if (!status)
        {
            if (map.SetStartAndEnd())
            {
                OnSaveButtonClick();
                status = true;
                StartAction();
            }
        }
    }
    public void OnAgainButtonClick()
    {
        StopAction();
        for (int i = 1; i <= map.Width; i++)
        {
            for (int j = 1; j <= map.Height; j++)
            {
                nodes[i][j].gameObject.SetActive(false);
            }
        }
        map.PaintMapPrefab();
        status = false;
    }
    public void OnClearButtonClick()
    {
        if (!status)
        {
            if (map.mapPrefab != null)
            {
                for (int i = 1; i <= map.Width; i++)
                {
                    for (int j = 1; j <= map.Height; j++)
                    {
                        nodes[i][j].transform.parent.GetComponent<Image>().color = Color.yellow;
                    }
                }
            }
            else
                print("No mapPrefab!");
        }
        else
            print("Already start!");
    }
}
