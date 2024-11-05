using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MapPrefab : ScriptableObject
{
    public List<Row> row;
}
[Serializable]
public class Row
{
    public List<BrushManager.NodeType> column;
}