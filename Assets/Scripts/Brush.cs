using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class Brush : MonoBehaviour
{
    public void OnNodeUIClick(GameObject go)
    {
        go.GetComponent<Image>().color = BrushManager.Instance.NodeTypeDict[BrushManager.Instance.nodeType];
    }
}