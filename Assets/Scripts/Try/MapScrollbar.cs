using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapScrollbar : MonoBehaviour
{
    public GameObject mapUIPrefab;



    //����1���ֶ���¼��ͼ��
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
    public RectTransform content; // Content ����� RectTransform
    public GridLayoutGroup gridLayoutGroup; // Grid Layout Group ���
    public int rowCount; // ����
    public int columnCount; // ����

    private float contentHeight;
    private float threshold;

    void Start()
    {
        // ���� Content ����ĸ߶�
        float cellHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
        contentHeight = cellHeight * rowCount;
        // ������ֵ���� Content �����ƫ�����������ֵʱ������λ��
        threshold = contentHeight / 2;



        //for (int i = 0; i < mapNames.Count; i++)
        //{
        //    GameObject map = Instantiate(mapUIPrefab, transform);
        //    map.transform.Find("MapName").GetComponent<TextMeshProUGUI>().text = mapNames[i];
        //}
    }

    void Update()
    {
        // ��ȡ Content ����ĵ�ǰƫ����
        float offset = content.anchoredPosition.y;

        // ���ƫ����������ֵ������λ��
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
//    public RectTransform content; // Content ����� RectTransform
//    public GridLayoutGroup gridLayoutGroup; // Grid Layout Group ���
//    public int rowCount; // ����
//    public int columnCount; // ����

//    private float contentHeight;
//    private float threshold;

//    void Start()
//    {
//        // ���� Content ����ĸ߶�
//        float cellHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
//        contentHeight = cellHeight * rowCount;
//        // ������ֵ���� Content �����ƫ�����������ֵʱ������λ��
//        threshold = contentHeight / 2;
//    }

//    void Update()
//    {
//        // ��ȡ Content ����ĵ�ǰƫ����
//        float offset = content.anchoredPosition.y;

//        // ���ƫ����������ֵ������λ��
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