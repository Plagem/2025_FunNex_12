using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    
    static GameManager _instance = null;
    public static GameManager instance { get { Init();  return _instance; } } 
    GameManager() {}

    static void Init()
    {
        if (_instance == null)
        {
            GameObject gm = GameObject.Find("GameManager");
            if (gm == null)
            {
                gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
                DontDestroyOnLoad(gm);
            }
        }
    }

    [SerializeField] private GameObject AugmentSelectUIPrefab;
    
    enum enemyType
    {
        DashEnemy_small, // 물범
        DashEnemy_Big, // 바다코끼리
        ShootEnemy, // 문어
        ShieldBreaker, // 성게
        Boss // 범고래
    }
    [SerializeField] private GameObject[] monsterPrefabList;

    [SerializeField] private TileBase[] TileList;

    [SerializeField] private Canvas MainCanvas;
    
    private AugmentSelectUI _augmentSelectUI = null;
    private List<StageData> stages = new List<StageData>();
    private int currentStageIndex = 0;
    private int playerLives = 1; // 목숨 일단 한 개

    private void Start()
    {
        InitializeStages();
        StartStage(currentStageIndex);

        // 5초마다 자동으로 다음 스테이지로 넘어가기 (테스트용)
        InvokeRepeating(nameof(OnPlayerStageWin), 5f, 5f);
    }


    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void InitializeStages()
    {
        // Stage 0
        StageData stage0 = new StageData
        {
            tilesToPlace = new TilePlacement[]
            {
            new TilePlacement { position = new Vector2Int(0, 0), tileType = TileType.Bumper },
            new TilePlacement { position = new Vector2Int(1, 1), tileType = TileType.IceWall }
            },
            tilesToClear = new Vector2Int[]
            {
            new Vector2Int(2, 2),
            new Vector2Int(3, 2)
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
            new MonsterPlacement {position = new Vector2Int(5, 5), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            new MonsterPlacement {position = new Vector2Int(6, 6), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]}
            }
        };
        stages.Add(stage0);

        // Stage 1
        StageData stage1 = new StageData
        {
            tilesToPlace = new TilePlacement[]
            {
            new TilePlacement { position = new Vector2Int(2, 0), tileType = TileType.IceFloor },
            new TilePlacement { position = new Vector2Int(2, 1), tileType = TileType.IceFloor},
            new TilePlacement { position = new Vector2Int(2, 2), tileType = TileType.IceFloor}
            },
            tilesToClear = new Vector2Int[]
            {
            new Vector2Int(0, 0)
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(7, 7), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(8, 8), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            }
        };
        stages.Add(stage1);

        // Stage 2
        StageData stage2 = new StageData
        {
            tilesToPlace = new TilePlacement[]
            {
            new TilePlacement { position = new Vector2Int(3, 0), tileType = TileType.IceFloor},
            new TilePlacement { position = new Vector2Int(4, 0), tileType = TileType.IceFloor}
            },
            tilesToClear = new Vector2Int[]
            {
            new Vector2Int(1, 1),
            new Vector2Int(2, 2)
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(10, 5), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(10, 6), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(10, 7), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            }
        };
        stages.Add(stage2);
    }


    public void StartStage(int stageIndex)
    {
        Debug.Log("Kexi");
        
        if (!_augmentSelectUI)
        {
            _augmentSelectUI = Instantiate(AugmentSelectUIPrefab, MainCanvas.transform).GetComponent<AugmentSelectUI>();
        }
        _augmentSelectUI.ShowOption();
        
        if (stageIndex < 0 || stageIndex >= stages.Count)
        {
            Debug.LogWarning("스테이지 인덱스 오류");
            return;
        }

        currentStageIndex = stageIndex;
        StageData stage = stages[stageIndex];

        TilemapManager tilemapManager = FindFirstObjectByType<TilemapManager>();

        // 타일 제거
        foreach (var pos in stage.tilesToClear)
        {
            tilemapManager.ClearTileAt(pos.x, pos.y);
        }  

        // 타일 생성
        foreach (var placement in stage.tilesToPlace)
        {
            tilemapManager.SetTileAt(placement.position.x, placement.position.y, TileList[(int)placement.tileType], placement.tileType);
        }

        // 몬스터 생성
        SpawnMonsters(stage.monsterSpawnPositions);
    }

    private void SpawnMonsters(MonsterPlacement[] placements)
    {
        foreach (var placement in placements)
        {
            Vector3 worldPos = new Vector3(placement.position.x + 0.5f, placement.position.y + 0.5f, -1f);
            Instantiate(placement.monster, worldPos, Quaternion.identity);
        }
    }

    public void OnPlayerStageWin()
    {
        Debug.Log($"스테이지 {currentStageIndex} 클리어!");

        int nextStageIndex = currentStageIndex + 1;

        if (nextStageIndex < stages.Count)
        {
            Debug.Log($"다음 스테이지로 이동: {nextStageIndex}");
            StartStage(nextStageIndex);
        }
        else
        {
            OnGameWin();
        }
    }

    private void OnGameWin()
    {
        // 게임 클리어 처리
        // 예: 클리어 UI 띄우기, 메인화면 이동 등
        Debug.Log("축하합니다! 게임을 클리어했습니다.");
        // 예: SceneManager.LoadScene("VictoryScene");
    }

    public void OnPlayerDied()
    {
        playerLives--;
        Debug.Log($"플레이어 사망. 남은 목숨: {playerLives}");

        if (playerLives <= 0)
        {
            OnPlayerLose();
        }
        else
        {
            // 플레이어 리스폰 또는 UI 갱신 등
            Debug.Log("플레이어 리스폰 준비 중...");
        }
    }


    public void OnPlayerLose()
    {
        Debug.Log("패배!");
        // 리트라이 처리
    }
}
