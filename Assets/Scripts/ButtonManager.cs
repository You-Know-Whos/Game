using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button saveButton;
    public Button startButton;
    public Button againButton;
    public Button clearButton;
    private Map map;
    private List<List<Node>> nodes;


    private void Start()
    {
        map = GetComponent<Map>();
        nodes = map.nodes;
        saveButton.onClick.AddListener(this.OnSaveButtonClick);
    }
    public void OnSaveButtonClick()
    {
        if (map.status == 0)
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
}
