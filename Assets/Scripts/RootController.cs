using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

using Cinemachine;
using Unity.VisualScripting;

using static UnityEditor.PlayerSettings;

public class Root : MonoBehaviour
{
    public Tile root;
    public Tile stem;
    public Tile limit;

    public Vector2 startPosition;
    public Vector2 rootPosition;
    public Vector2 stemPosition;

    private bool canInstall = false;

    private Tilemap map;
    private GridLayout gridLayout;
    private Vector3Int cellPosition;

    private void Start()
    {
        map = GetComponent<Tilemap>();
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
    }

    public bool IsTileCanInstall(Tile tile, Vector2 pos)
    {
        Tile target = map.GetTile<Tile>(gridLayout.WorldToCell(pos));
        if (tile == target || tile == limit)
            return false;
        else
            return true;
    }

    public void ChangeTile(Tile tile, Vector2 point)
    {
        cellPosition = gridLayout.WorldToCell(point);
        map.SetTile(cellPosition, tile);
    }

    public void PressDown()
    {
        canInstall = IsTileCanInstall(root, rootPosition + Vector2.down);
        if (!canInstall) 
            return;

        stemPosition += Vector2.up; // reverse
        rootPosition += Vector2.down;

        // Change Tile
        ChangeTile(root, rootPosition);
        ChangeTile(stem, stemPosition);
    }

    public void PressUp()
    {
        canInstall = IsTileCanInstall(root, rootPosition + Vector2.up);
        if (!canInstall) 
            return;

        stemPosition += Vector2.down; 
        rootPosition += Vector2.up;

        ChangeTile(root, rootPosition);
        ChangeTile(stem, stemPosition);
    }

    public void PressRight()
    {
        canInstall = contoller.IsTileCanInstall(contoller.root, rootPosition + Vector2.right);
        if (!canInstall) 
            return;

        stemPosition += Vector2.left;
        rootPosition += Vector2.right;

        contoller.ChangeTile(contoller.root, rootPosition);
        contoller.ChangeTile(contoller.stem, stemPosition);
    }

    public void PressLeft()
    {
        canInstall = contoller.IsTileCanInstall(contoller.root, rootPosition + Vector2.left);
        if (!canInstall) 
            return;

        stemPosition += Vector2.right;
        rootPosition += Vector2.left;

        contoller.ChangeTile(contoller.root, rootPosition);
        contoller.ChangeTile(contoller.stem, stemPosition);
    }

    public void Reset()
    {
       
    }
}