using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMove : MonoBehaviour
{
    public float speed = 1;
    public Transform ant;
    private void Start()
    {
        //TODO:move to ant script
        BoundsInt bounds = SlotController.Instance.tileMap.cellBounds;
        Vector3Int centerCellPos = new Vector3Int(
            (bounds.xMax + bounds.xMin) / 2,
            (bounds.yMin + bounds.yMax) / 2,
            (bounds.zMax + bounds.zMin) / 2);
        ant.position = SlotController.Instance.tileMap.GetCellCenterWorld(centerCellPos);
    }
}
