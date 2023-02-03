using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;



public class MoveTile : MonoBehaviour
{
    public Tile rootTile;
    public Tile stanTile;
    public Tile limitTile;

    public PlayerBehavior player;

    private Tilemap map;
    private GridLayout gridLayout;
    private Vector3Int cellPosition;

    private Vector2 rootPosition;
    private Vector2 stanPosition;

    private void Start()
    {
        map = GetComponent<Tilemap>();
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();

        ChangeTile(rootTile, rootPosition);
        ChangeTile(stanTile, stanPosition);
    }

    private bool CheckTileCanSet(Tile tile, Vector2 pos)
    {
        Tile targetTile = map.GetTile<Tile>(gridLayout.WorldToCell(pos));
        if (targetTile == tile || limitTile == tile)
            return false;
        else
            return true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(player.Key_Down))
        {
            if (CheckTileCanSet(rootTile, rootPosition + Vector2.down))
            {
                stanPosition += Vector2.up;
                rootPosition += Vector2.down;
                
                ChangeTile(rootTile, rootPosition);
                ChangeTile(stanTile, stanPosition);

                MoveOnLiftMode(stanPosition + Vector2.down, stanPosition);
                return;
            }
        }
        else if (Input.GetKeyDown(player.Key_Up))
        {
            if (CheckTileCanSet(rootTile, rootPosition + Vector2.up))
            {
                stanPosition += Vector2.down;
                rootPosition += Vector2.up;

                ChangeTile(rootTile, rootPosition);
                ChangeTile(stanTile, stanPosition);

                MoveOnLiftMode(stanPosition + Vector2.up, stanPosition);
                return;
            }
        }
        else if (Input.GetKeyDown(player.Key_Left))
        {
            if (CheckTileCanSet(rootTile, rootPosition + Vector2.left))
            {
                stanPosition += Vector2.right;
                rootPosition += Vector2.left;

                ChangeTile(rootTile, rootPosition);
                ChangeTile(stanTile, stanPosition);

                MoveOnLiftMode(stanPosition + Vector2.right, stanPosition);
                return;
            }
        }
        else if (Input.GetKeyDown(player.Key_Right))
        {
            if (CheckTileCanSet(rootTile, rootPosition + Vector2.right))
            {
                stanPosition += Vector2.left;
                rootPosition += Vector2.right;

                ChangeTile(rootTile, rootPosition);
                ChangeTile(stanTile, stanPosition);

                MoveOnLiftMode(stanPosition + Vector2.left, stanPosition);
                return;
            }
        }       
    }

    private void ChangeTile(Tile tile, Vector2 point)
    {        
        cellPosition = gridLayout.WorldToCell(point);
        Debug.Log(cellPosition);
        map.SetTile(cellPosition, tile);
    }

    public Vector2 startPosition;
    public float liftSpeed;
    private GameObject transparentBox = null;

    private bool isMoving = false;

    private bool liftMode = false;
    public bool LiftMode 
    {
        get
        {
            return liftMode;
        }
        set
        {
            liftMode = value;
            if (liftMode)
            {
                if (transparentBox == null)
                    transparentBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                transparentBox.transform.position = startPosition;

                player.transform.SetParent(transparentBox.transform);
                player.transform.localPosition = Vector2.up;
            }
        }
    }

    private void MoveOnLiftMode(Vector2 start, Vector2 to)
    {
        if (liftMode)
            StartCoroutine(MoveTransparentBox(start, to));
    }

    private IEnumerator MoveTransparentBox(Vector2 start, Vector2 to)
    {
        isMoving = true;
        float t = 0;
        transparentBox.transform.position = start;
        while (t < 1)
        {
            transparentBox.transform.position = Vector2.Lerp(start, to, t);
            t += Time.deltaTime * liftSpeed;
            yield return null;
        }
        transparentBox.transform.position = to;
        isMoving = false;
    }
}
