using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    public void OnClick()
    {
        //transform.parent.GetComponent<ChooseMapManager>().OnMapChosen(gameObject);
        transform.parent.GetComponent<MapScrollbar>().OnMapChosen(gameObject);
    }
}