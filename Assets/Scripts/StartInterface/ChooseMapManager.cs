using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMapManager : MonoBehaviour
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
    private void Start()
    {
        for (int i = 0; i < mapNames.Count; i++)
        {
            GameObject map = Instantiate(mapUIPrefab, transform);
            map.transform.Find("MapName").GetComponent<TextMeshProUGUI>().text = mapNames[i];
        }
        
    }
    public void OnMapChosen(GameObject go)
    {
        SceneManager.LoadScene(go.transform.Find("MapName").GetComponent<TextMeshProUGUI>().text.ToString());
    }
}
