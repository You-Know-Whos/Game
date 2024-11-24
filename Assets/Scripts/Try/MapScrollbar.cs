using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapScrollbar : MonoBehaviour
{
    public GameObject mapUIPrefab;



    //方法1：手动记录地图名
    private List<string> mapNames = new List<string> {"SampleScene",
                                                      "A30multiply17",
                                                      "NoPath",
                                                      "Bucket",
                                                      "Easy",
                                                      "Diagonal",
                                                      "Huge"
                                                      };
    //private void Start()
    //{
    //    GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
    //    gridLayoutGroup.cellSize = new Vector2(GetComponent<RectTransform>().rect.height, GetComponent<RectTransform>().rect.height);
    //    for (int i = 0; i < mapNames.Count; i++)
    //    {
    //        GameObject map = Instantiate(mapUIPrefab, transform);
    //        map.transform.Find("MapName").GetComponent<TextMeshProUGUI>().text = mapNames[i];
    //    }

    //}
    public RectTransform content; // Content 对象的 RectTransform
    public GridLayoutGroup gridLayoutGroup; // Grid Layout Group 组件
    public int rowCount; // 行数
    public int columnCount; // 列数

    private float contentHeight;
    private float threshold;

    void Start()
    {
        // 计算 Content 对象的高度
        float cellHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
        contentHeight = cellHeight * rowCount;
        // 设置阈值，当 Content 对象的偏移量超过这个值时，重置位置
        threshold = contentHeight / 2;



        //for (int i = 0; i < mapNames.Count; i++)
        //{
        //    GameObject map = Instantiate(mapUIPrefab, transform);
        //    map.transform.Find("MapName").GetComponent<TextMeshProUGUI>().text = mapNames[i];
        //}
    }

    void Update()
    {
        // 获取 Content 对象的当前偏移量
        float offset = content.anchoredPosition.y;

        // 如果偏移量超过阈值，重置位置
        if (offset > threshold)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, offset - threshold);
        }
        else if (offset < -threshold)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, offset + threshold);
        }
    }
    public void OnMapChosen(GameObject go)
    {
        SceneManager.LoadScene(go.transform.Find("MapName").GetComponent<TextMeshProUGUI>().text.ToString());
    }
}

//public class LoopScrollViewWithGrid : MonoBehaviour
//{
//    public RectTransform content; // Content 对象的 RectTransform
//    public GridLayoutGroup gridLayoutGroup; // Grid Layout Group 组件
//    public int rowCount; // 行数
//    public int columnCount; // 列数

//    private float contentHeight;
//    private float threshold;

//    void Start()
//    {
//        // 计算 Content 对象的高度
//        float cellHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
//        contentHeight = cellHeight * rowCount;
//        // 设置阈值，当 Content 对象的偏移量超过这个值时，重置位置
//        threshold = contentHeight / 2;
//    }

//    void Update()
//    {
//        // 获取 Content 对象的当前偏移量
//        float offset = content.anchoredPosition.y;

//        // 如果偏移量超过阈值，重置位置
//        if (offset > threshold)
//        {
//            content.anchoredPosition = new Vector2(content.anchoredPosition.x, offset - threshold);
//        }
//        else if (offset < -threshold)
//        {
//            content.anchoredPosition = new Vector2(content.anchoredPosition.x, offset + threshold);
//        }
//    }
//}