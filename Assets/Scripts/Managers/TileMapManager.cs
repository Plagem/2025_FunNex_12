using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    public void ClearTileAtWorldPos(Vector3 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        tilemap.SetTile(cellPos, null);
    }

    public void ClearTileAt(int x, int y)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), null);
    }

    public TileBase GetTileAt(int x, int y)
    {
        return tilemap.GetTile(new Vector3Int(x, y, 0));
    }

    public void SetTileAt(int x, int y, TileBase tile)
    {
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        tilemap.SetTile(cellPos, tile);
    }
}
