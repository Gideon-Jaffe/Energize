using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InverterShotController : ShotControllerBase
{
    private Tilemap backTileMap;
    [SerializeField] private Tile backTile;

    void Start()
    {
        Init();
    }

    new void Init()
    {
        backTileMap = GameObject.Find("BackTileMap").GetComponent<Tilemap>();
        base.Init();
    }

    public override void CheckForExploded()
    {
        Vector3Int tileLocation = tilemap.WorldToCell(transform.position);
        Tile tile = (Tile)tilemap.GetTile(tileLocation);

        if (tile != null)
        {
            exploded = true;
            if (tile == tileToInteractWith)
            {
                MoveToBackTileAndAdjacent(tileLocation);
            }
            Destroy(gameObject);
            return;
        }

        tile = (Tile)backTileMap.GetTile(tileLocation);

        if (tile != null)
        {
            exploded = true;
            if (tile == backTile)
            {
                MoveToFrontTileAndAdjacent(tileLocation);
            }
            Destroy(gameObject);
            return;
        }
        
    }

    private void MoveToBackTileAndAdjacent(Vector3Int tileLocation)
    {
        if (tilemap.GetTile(tileLocation) != tileToInteractWith)
        {
            return;
        }
        tilemap.SetTile(tileLocation, null);
        backTileMap.SetTile(tileLocation, backTile);
        MoveToBackTileAndAdjacent(tileLocation + Vector3Int.right);
        MoveToBackTileAndAdjacent(tileLocation + Vector3Int.left);
        MoveToBackTileAndAdjacent(tileLocation + Vector3Int.up);
        MoveToBackTileAndAdjacent(tileLocation + Vector3Int.down);
    }

    private void MoveToFrontTileAndAdjacent(Vector3Int tileLocation)
    {
        if (backTileMap.GetTile(tileLocation) != backTile)
        {
            return;
        }
        tilemap.SetTile(tileLocation, tileToInteractWith);
        backTileMap.SetTile(tileLocation, null);
        MoveToFrontTileAndAdjacent(tileLocation + Vector3Int.right);
        MoveToFrontTileAndAdjacent(tileLocation + Vector3Int.left);
        MoveToFrontTileAndAdjacent(tileLocation + Vector3Int.up);
        MoveToFrontTileAndAdjacent(tileLocation + Vector3Int.down);
    
    }
}
