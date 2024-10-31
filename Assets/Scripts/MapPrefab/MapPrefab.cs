using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapPrefab : ScriptableObject
{
    public List<Row> row;
}
[Serializable]
public class Row
{
    public List<BrushManager.NodeType> column;
}