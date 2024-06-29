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
    private EasyGrid<SlotData> _showGrid = new EasyGrid<SlotData>(25, 14);
    private void Start()　
    {
        //设置grid覆盖屏幕
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, Camera.main.nearClipPlane));
        grid.transform.position = new Vector3(bottomLeft.x,bottomLeft.y,bottomLeft.z);
        
        //显示默认方格
        for (int i = 0; i < _showGrid.Width; i++)
        {
            for (int j = 0; j < _showGrid.Height; j++)
            {
                tileMap.SetTile(new Vector3Int(i,j,0),whiteTile);
            }
        }
    }

    private Vector3Int WorldPosToGridPos(Vector3 worldPos)
    {
        Vector3Int gridPos = new Vector3Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.y),
            Mathf.RoundToInt(worldPos.z));
        return gridPos;
    }

    public TileBase GetCurrentPosTileBase(Vector3 worldPos)
    {
        return tileMap.GetTile(WorldPosToGridPos(worldPos));
    }
}
