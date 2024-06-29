using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AntMove : MonoBehaviour
{
    //rotate angle
    private const float RotateSpeed = 90f;

    public TileBase white;
    public TileBase black;
    private bool _needRotate = true;
    private SlotController _slotController;
    private Vector3Int _currentSlotPos;
    private Vector3Int _nextSlotPos;
    private Vector3Int _direction = Vector3Int.up;
    private float _rotateSpeed = 2f;
    private float _moveSpeed = 1f;

    private void Start()
    {
        _slotController = SlotController.Instance;
        //set ant initialize pos in grid center
        BoundsInt bounds = SlotController.Instance.tileMap.cellBounds;
        Vector3Int centerCellPos = new Vector3Int(
            (bounds.xMax + bounds.xMin) / 2,
            (bounds.yMin + bounds.yMax) / 2,
            (bounds.zMax + bounds.zMin) / 2);

        transform.position = _slotController.tileMap.GetCellCenterWorld(centerCellPos);
        
        GetNextSlotDir();
        _currentSlotPos = centerCellPos;
        _nextSlotPos = _currentSlotPos + _direction;
    }

    private void Update()
    {
        if ((_slotController.tileMap.GetTile(GetAntIntPos()) == white|| black) && _needRotate)
        {
            _needRotate = false;
            StartCoroutine(RotateAnt());
        }
    }

    private TileBase GetCurrentPosTile()
    {
        return _slotController.tileMap.GetTile(_currentSlotPos);
    }

    /// <summary>
    /// get next slot tile based current slot tile
    /// </summary>
    private void GetNextSlotDir()
    {
        if (GetCurrentPosTile() == _slotController.whiteTile)
        {
            if (_direction == Vector3Int.up)
            {
                _direction = Vector3Int.right;
            }
            else if (_direction == Vector3Int.down)
            {
                _direction = Vector3Int.left;
            }
            else if (_direction == Vector3Int.left)
            {
                _direction = Vector3Int.up;
            }
            else if (_direction == Vector3Int.right)
            {
                _direction = Vector3Int.down;
            }
        }
        else if (GetCurrentPosTile() == _slotController.blackTile)
        {
            if (_direction == Vector3Int.up)
            {
                _direction = Vector3Int.left;
            }
            else if (_direction == Vector3Int.down)
            {
                _direction = Vector3Int.right;
            }
            else if (_direction == Vector3Int.left)
            {
                _direction = Vector3Int.down;
            }
            else if (_direction == Vector3Int.right)
            {
                _direction = Vector3Int.up;
            }
        }
    }

    /// <summary>
    /// make ant move towards ant's front
    /// </summary>
    private IEnumerator MoveToNextSlot()
    {
        var targetPos = _slotController.tileMap.GetCellCenterWorld(_nextSlotPos);
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        while (elapsedTime<_moveSpeed)
        {
            transform.position= Vector3.Lerp(startPos,targetPos,elapsedTime/_moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        
        if (_slotController.tileMap.GetTile<TileBase>(_currentSlotPos) == _slotController.whiteTile)
        {
            _slotController.tileMap.SetTile(_currentSlotPos,_slotController.blackTile);
        }
        else
        {
            _slotController.tileMap.SetTile(_currentSlotPos,_slotController.whiteTile);
        }
        _currentSlotPos = _nextSlotPos;
        GetNextSlotDir();
        _nextSlotPos += _direction;
        _needRotate = true;
    }
    

    private IEnumerator RotateAnt()
    {
        float totalRotation = 0f;
        float rotationDirection = 1f;
        if (_slotController.tileMap.GetTile<TileBase>(_currentSlotPos) == white)
        {
            rotationDirection = 1f;
        }
        else if (_slotController.tileMap.GetTile<TileBase>(_currentSlotPos) == black)
        {
            rotationDirection = -1f;
        }
        while (totalRotation < 90f)
        {
            float rotationAmount = RotateSpeed * Time.deltaTime * _rotateSpeed*rotationDirection;
            totalRotation += Mathf.Abs(rotationAmount);
            transform.Rotate(0, 0, -rotationAmount);
            yield return null;
        }

        StartCoroutine(MoveToNextSlot());
    }

    /// <summary>
    /// get ant integer pos
    /// </summary>
    /// <returns></returns>
    private Vector3Int GetAntIntPos()
    {
        Vector3Int antMapPos = _slotController.tileMap.WorldToCell(transform.position);
        return antMapPos;
    }
}