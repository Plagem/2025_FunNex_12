using System;
using UnityEngine;

[Serializable]
public class StageData
{
    public TilePlacement[] tilesToPlace;
    public Vector2Int[] tilesToClear;
    public Vector2Int[] monsterSpawnPositions;
}
