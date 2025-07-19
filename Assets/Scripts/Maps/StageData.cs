using System;
using UnityEngine;

[Serializable]
public class StageData
{
    public Vector2Int playerPosition;
    public TilePlacement[] tilesToPlace;
    public Vector2Int[] tilesToClear;
    public MonsterPlacement[] monsterSpawnPositions;
}
