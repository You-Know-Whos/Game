using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void GoAction();
    public static event GoAction goAction;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            goAction();
        }
    }

}
