using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    IceFloor, // 기본 얼음 타일
    IceWall, // 얼음벽
    Bumper // 얼음벽 높은 버전
}

[Serializable]
public class TilePlacement
{
    public Vector2Int position;
    public TileType tileType;
}
