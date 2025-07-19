using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap basicTilemap;
    [SerializeField] private Tilemap iceWallTilemap;
    [SerializeField] private Tilemap BumperTilemap;

    public void ClearTileAtWorldPos(Vector3 worldPos)
    {
        basicTilemap.SetTile(basicTilemap.WorldToCell(worldPos), null);
        iceWallTilemap.SetTile(iceWallTilemap.WorldToCell(worldPos), null);
        BumperTilemap.SetTile(BumperTilemap.WorldToCell(worldPos), null);
    }

    public void ClearTileAt(int x, int y)
    {
        basicTilemap.SetTile(new Vector3Int(x, y, 0), null);
        iceWallTilemap.SetTile(new Vector3Int(x, y, 0), null);
        BumperTilemap.SetTile(new Vector3Int(x, y, 0), null);
    }

    public TileBase GetTileAt(int x, int y)
    {
        if (BumperTilemap.HasTile(new Vector3Int(x, y, 0)))
            return BumperTilemap.GetTile(new Vector3Int(x, y, 0));
        else if (iceWallTilemap.HasTile(new Vector3Int(x, y, 0)))
            return iceWallTilemap.GetTile(new Vector3Int(x, y, 0));
        return basicTilemap.GetTile(new Vector3Int(x, y, 0));
    }

    public void SetTileAt(int x, int y, TileBase tile, TileType tileType)
    {
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        switch (tileType)
        {
            case TileType.IceFloor:
                basicTilemap.SetTile(cellPos, tile);
                break;
            case TileType.IceWall:
                iceWallTilemap.SetTile(cellPos, tile);
                break;
            case TileType.Bumper:
                BumperTilemap.SetTile(cellPos, tile);
                break;

        }
    }
}
