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
    public Tile seed;

    public Tile root;
    public Tile stem;
    public List<Tile> obstacles = new List<Tile>();

    private Vector2 startPosition = Vector2.zero;
    public List<Vector2> rootPositions  = new List<Vector2>();
    public List<Vector2> stemPositions  = new List<Vector2>();

    private bool canInstall = false;
    
    private GridLayout gridLayout;
    private Vector3Int cellPosition = Vector3Int.zero;

    public Transform childCam;
    public Tilemap map;

    private void Start()
    {
        startPosition = transform.position;

        gridLayout = map.transform.parent.GetComponentInParent<GridLayout>();
        ChangeTile(seed, startPosition);

        rootPositions.Add(startPosition);
        stemPositions.Add(startPosition);
    }

    public bool IsTileCanInstall(Tile tile, Vector2 worldPos)
    {
        Tile target = map.GetTile<Tile>(gridLayout.WorldToCell(worldPos));

        if (target == root)
            return false; // cannot path myself

        foreach (var obstacle in obstacles)
        {
            if (target == obstacle)
                return false; // cannot install new tile...!
        }
        return true;
    }

    public void ChangeTile(Tile tile, Vector2 worldPos)
    {
        cellPosition = gridLayout.WorldToCell(worldPos);
        map.SetTile(cellPosition, tile);
    }

    public void PressDown()     => Move(Vector2.down);

    public void PressUp()       => Move(Vector2.up);

    public void PressRight()    => Move(Vector2.right);

    public void PressLeft()     => Move(Vector2.left);

    private void Move(Vector2 direction)
    {
        Vector2 rootPosition = rootPositions[rootPositions.Count - 1] + direction;
        Vector2 stemPosition = stemPositions[stemPositions.Count - 1] + direction * -1;

        canInstall = IsTileCanInstall(root, rootPosition);
        if (!canInstall)
            return;

        stemPositions.Add(stemPosition);
        rootPositions.Add(rootPosition);

        ChangeTile(root, rootPosition);
        ChangeTile(stem, stemPosition);
    }

    private void Undo()
    {
        Vector2 rootPosition = rootPositions[rootPositions.Count - 1];
        Vector2 stemPosition = stemPositions[stemPositions.Count - 1];

        ChangeTile(null, rootPosition);
        ChangeTile(null, stemPosition);

        stemPositions.RemoveAt(stemPositions.Count - 1);
        rootPositions.RemoveAt(rootPositions.Count - 1);
    }
}