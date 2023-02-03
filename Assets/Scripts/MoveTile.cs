using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;



public class MoveTile : MonoBehaviour
{
    Tilemap map;
    GridLayout gridLayout;
    Vector3Int cellPosition;

    public Tile rootTile;
    public Tile flowerTile;

    private Vector2 pos = Vector2.zero;
    private Vector2 mirrorPos = Vector2.up;

    private void Start()
    {
        map = GetComponent<Tilemap>();
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        ChangeTile(rootTile, Vector2.zero);
        ChangeTile(flowerTile, mirrorPos);
    }
    private bool CheckOtherTile(Tile tile, Vector2 pos)
    {

        if (tile== map.GetTile<Tile>(gridLayout.WorldToCell(pos)))
            return false;
        else
            return true;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (CheckOtherTile(rootTile, pos + Vector2.down))
            {
                mirrorPos += Vector2.up;
                pos += Vector2.down;
                
                ChangeTile(rootTile, pos);
                ChangeTile(flowerTile, mirrorPos);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CheckOtherTile(rootTile, pos + Vector2.up))
            {
                mirrorPos += Vector2.down;
                pos += Vector2.up;
                ChangeTile(rootTile, pos);
                ChangeTile(flowerTile, mirrorPos);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CheckOtherTile(rootTile, pos + Vector2.left))
            {
                mirrorPos += Vector2.right;
                pos += Vector2.left;
                ChangeTile(rootTile, pos);
                ChangeTile(flowerTile, mirrorPos);
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (CheckOtherTile(rootTile, pos + Vector2.right))
            {
                mirrorPos += Vector2.left;
                pos += Vector2.right;
                ChangeTile(rootTile, pos);
                ChangeTile(flowerTile, mirrorPos);
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
}
