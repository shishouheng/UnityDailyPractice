using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlotController : MonoBehaviour
{
    #region singleton
    private static SlotController _instance;
    public static SlotController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SlotController>();
            }

            return _instance;
        }
    }
    #endregion
    public TileBase whiteTile;
    public TileBase blackTile;
    public Tilemap tileMap;
    public Grid grid;
    public Vector2Int tileMapSize = new Vector2Int(21, 14);
    private void Start()　
    {
        //设置grid覆盖屏幕
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.main.nearClipPlane));
        grid.transform.position = new Vector3(bottomLeft.x,bottomLeft.y,bottomLeft.z);
        
        //显示默认方格
        for (int i = 0; i < tileMapSize.x; i++)
        {
            for (int j = 0; j < tileMapSize.y; j++)
            {
                tileMap.SetTile(new Vector3Int(i,j,0),whiteTile);
            }
        }
        
        
    }
}
