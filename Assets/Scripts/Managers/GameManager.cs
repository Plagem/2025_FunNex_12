using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject successImg;
    [SerializeField] private GameObject failImg;
    [SerializeField] private GameObject exitBtn;

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
            _instance = gm.GetComponent<GameManager>();
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

    [SerializeField] public Canvas MainCanvas;
    
    private AugmentSelectUI _augmentSelectUI = null;
    private List<StageData> stages = new List<StageData>();
    private int currentStageIndex = 0;

    private bool isSpawning = false;
    private GameObject Player;
    private int playerLives = 1; // 목숨 일단 한 개

    private void Start()
    {
        Player = FindAnyObjectByType<PlayerController>().gameObject;

        InitializeStages();
        StartStage(currentStageIndex);

        // 5초마다 자동으로 다음 스테이지로 넘어가기 (테스트용)
        //InvokeRepeating(nameof(OnPlayerStageWin), 5f, 5f);
        successImg.SetActive(false);
        failImg.SetActive(false);
        exitBtn.SetActive(false);

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (!isSpawning && FindAnyObjectByType<EnemyBase>() == null)
            {
                OnPlayerStageWin();
            }   
        }
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
            playerPosition = new Vector2Int(0, -4),
            tilesToPlace = new TilePlacement[]
            {
                new TilePlacement { position = new Vector2Int(-9, 3), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-8, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-7, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-6, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-5, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-4, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-3, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-2, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-1, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(-0, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(1, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(2, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(3, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(4, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(5, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(6, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(7, 3), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(7, 4), tileType = TileType.IceWall },
                new TilePlacement { position = new Vector2Int(8, 2), tileType = TileType.IceWall },
            },
            tilesToClear = new Vector2Int[]
            {
                new Vector2Int(-9, 4),
                new Vector2Int(-9, 2),
                new Vector2Int(-9, -1),
                new Vector2Int(-9, -2),
                new Vector2Int(-9, -5),
                new Vector2Int(-5, -5),
                new Vector2Int(-4, -5),
                new Vector2Int(3, -5),
                new Vector2Int(4, -5),
                new Vector2Int(8, -5),
                new Vector2Int(8, -1),
                new Vector2Int(8, 0),
                new Vector2Int(8, 3),
                new Vector2Int(8, 4)
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
            new MonsterPlacement {position = new Vector2Int(-4, -2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            new MonsterPlacement {position = new Vector2Int(0, 1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            new MonsterPlacement {position = new Vector2Int(4, -2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            }
        };
        stages.Add(stage0);


        // Stage 1
        StageData stage1 = new StageData
        {
            playerPosition = new Vector2Int(-7, -3),
            tilesToPlace = new TilePlacement[]
            {

            },
            tilesToClear = new Vector2Int[]
            {
                new Vector2Int(-2, 2),
                new Vector2Int(-1, 3),
                new Vector2Int(-1, 2),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 3),
                new Vector2Int(0, 2),
                new Vector2Int(0, 1),
                new Vector2Int(1, 2),
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(-5, 0), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-4, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-3, -2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-3, 0), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
            }
        };
        stages.Add(stage1);

        // Stage 2
        StageData stage2 = new StageData
        {
            playerPosition = new Vector2Int(6, -3),
            tilesToPlace = new TilePlacement[]
            {
                new TilePlacement { position = new Vector2Int(-3, 3), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-3, 2), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-2, 1), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-1, -2), tileType = TileType.Bumper},
            },
            tilesToClear = new Vector2Int[]
            {
               new Vector2Int(-2, 4),
               new Vector2Int(-2, 3),
               new Vector2Int(-1, 4),
               new Vector2Int(-0, 4),
               new Vector2Int(1, 4),
               new Vector2Int(1, 3),
               new Vector2Int(2, 4),
               new Vector2Int(2, 3),
               new Vector2Int(3, 4),
               new Vector2Int(-3, -5),
               new Vector2Int(-2, -5),
               new Vector2Int(-2, -4),
               new Vector2Int(-1, -5),
               new Vector2Int(-1, -4),
               new Vector2Int(0, -5),
               new Vector2Int(0, -4),
               new Vector2Int(1, -5),
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(-5, 0), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(-5, -1), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(-5, -2), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(3, -3), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(3, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(4, 0), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(5, 1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
            }
        };
        stages.Add(stage2);

        // Stage 3
        StageData stage3 = new StageData
        {
            playerPosition = new Vector2Int(-7, 3),
            tilesToPlace = new TilePlacement[]
            {
                new TilePlacement { position = new Vector2Int(-3, 1), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(-2, 0), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(-1, 0), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(0, 0), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(1, 0), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(-3, -3), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(-2, -3), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(-1, -3), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(0, -3), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(1, -3), tileType = TileType.Bumper},
                new TilePlacement { position = new Vector2Int(2, 1), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(2, -3), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(3, 2), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(3, 3), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(3, -4), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-1, -2), tileType = TileType.IceFloor},
            },
            tilesToClear = new Vector2Int[]
            {
               new Vector2Int(-1, -2),
               new Vector2Int(1, 1),
               new Vector2Int(3, -4),
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(5, 1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(5, -3), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
            }
        };
        stages.Add(stage3);
        

        // Stage 4
        StageData stage4 = new StageData
        {
            playerPosition = new Vector2Int(6,-3),
            tilesToPlace = new TilePlacement[]
            {
               new TilePlacement { position = new Vector2Int(-4, 3), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-3, 1), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-3, 0), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-3, -2), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-4, -3), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-4, -4), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-6, -5), tileType = TileType.IceWall},
            },
            tilesToClear = new Vector2Int[]
            {
               new Vector2Int(-4, 4),
               new Vector2Int(-3, 4),
               new Vector2Int(-3, 3),
               new Vector2Int(-3, 1),
               new Vector2Int(-2, 1),
               new Vector2Int(-2, 0),
               new Vector2Int(-1, 0),
               new Vector2Int(0, 0),
               new Vector2Int(1, 0),
               new Vector2Int(2, 1),
               new Vector2Int(2, 2),
               new Vector2Int(3, 2),
               new Vector2Int(3, 3),


               new Vector2Int(-2, -2),
               new Vector2Int(-1, -2),
               new Vector2Int(0, -2),
               new Vector2Int(1, -2),

                new Vector2Int(-3, -3),
                new Vector2Int(-3, -4),
               new Vector2Int(-2, -3),
               new Vector2Int(-1, -3),
               new Vector2Int(0, -3),
               new Vector2Int(1, -3),
               new Vector2Int(2, -3),

               new Vector2Int(1, -4),
               new Vector2Int(2, -4),
               new Vector2Int(3, -4),
               new Vector2Int(2, -5),
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(4, 0), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(5, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(6, 0), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(5, 1), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(-7, 2), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(-7, 0), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(-7, -2), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
            }
        };
        stages.Add(stage4);

        // Stage 5
        StageData stage5 = new StageData
        {
            playerPosition = new Vector2Int(0,-4),
            tilesToPlace = new TilePlacement[]
            {
               new TilePlacement { position = new Vector2Int(-9, 1), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-9, 0), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-9, -3), tileType = TileType.IceWall},
               new TilePlacement { position = new Vector2Int(-9, -4), tileType = TileType.IceWall},


                new TilePlacement { position = new Vector2Int(-8, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-8, -2), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-8, -1), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-8, 2), tileType = TileType.IceWall},

                new TilePlacement { position = new Vector2Int(-7, -5), tileType = TileType.IceWall},

                new TilePlacement { position = new Vector2Int(8, 1), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(8, -2), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(8, -3), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(8, -4), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(8, -5), tileType = TileType.IceWall},

                new TilePlacement { position = new Vector2Int(8, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(7, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(6, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(5, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(4, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(3, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(2, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(1, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(0, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-1, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-2, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-3, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-4, -5), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-5, -5), tileType = TileType.IceWall},

                new TilePlacement { position = new Vector2Int(-3, 3), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(-2, 2), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(1, 2), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(2, 3), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(3, 4), tileType = TileType.IceWall},


                new TilePlacement { position = new Vector2Int(-4, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-3, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-2, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-1, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(0, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(1, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(2, -4), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(3, -4), tileType = TileType.IceFloor},

                new TilePlacement { position = new Vector2Int(-4, -3), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-3, -3), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-2, -3), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-1, -3), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(0, -3), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(1, -3), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(2, -3), tileType = TileType.IceFloor},

                new TilePlacement { position = new Vector2Int(-3, -2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-2, -2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-1, -2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(0, -2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(1, -2), tileType = TileType.IceFloor},


                new TilePlacement { position = new Vector2Int(-3, 0), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-2, 0), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-1, 0), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(0, 0), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(1, 0), tileType = TileType.IceFloor},

                new TilePlacement { position = new Vector2Int(-3, 1), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-2, 1), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(-1, 1), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(0, 1), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(1, 1), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(2, 1), tileType = TileType.IceFloor},

                new TilePlacement { position = new Vector2Int(-3, 2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(2, 2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(3, 2), tileType = TileType.IceFloor},
                new TilePlacement { position = new Vector2Int(3, 3), tileType = TileType.IceFloor},

                
                new TilePlacement { position = new Vector2Int(7, 0), tileType = TileType.IceWall},
                new TilePlacement { position = new Vector2Int(7, -1), tileType = TileType.IceWall},
            },
            tilesToClear = new Vector2Int[]
            {
                    new Vector2Int(-9,1),
                    new Vector2Int(-9,0),
                    new Vector2Int(-9,-3),
                    new Vector2Int(-9,-4),

                    new Vector2Int(-8,2),
                    new Vector2Int(-8,-1),
                    new Vector2Int(-8,-2),
                    new Vector2Int(-8,-5),

                    new Vector2Int(-7,-5),
                    new Vector2Int(-5,-5),
                    new Vector2Int(-4,-5),
                    new Vector2Int(-3,-5),
                    new Vector2Int(-2,-5),
                    new Vector2Int(-1,-5),
                    new Vector2Int(0,-5),
                    new Vector2Int(1,-5),
                    new Vector2Int(2,-5),
                    new Vector2Int(3,-5),
                    new Vector2Int(4,-5),
                    new Vector2Int(5,-5),
                    new Vector2Int(6,-5),
                    new Vector2Int(7,-5),
                    new Vector2Int(8,-5),

                    new Vector2Int(8,-4),
                    new Vector2Int(8,-3),
                    new Vector2Int(8,-2),
                    new Vector2Int(7,-1),
                    new Vector2Int(7,0),
                    new Vector2Int(8,1),

                    new Vector2Int(-4,-4),
                    new Vector2Int(-4,-3),
                    new Vector2Int(-3,-2),
                    new Vector2Int(-3,0),
                    new Vector2Int(-3,1),
                    new Vector2Int(-3,2),
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(-6, 2), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(-7, -3), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(5, 2), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(6, -3), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
            }
        };
        stages.Add(stage5);

        // Stage 6
        StageData stage6 = new StageData
        {
            playerPosition = new Vector2Int(-7,0),
            tilesToPlace = new TilePlacement[]
            {

               new TilePlacement { position = new Vector2Int(-9, 3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-9, 1),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-9, 0),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-9, -3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-9, -4),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(-8, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-8, -2),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-8, -1),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-8, 2),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-8, 4),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(-7, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-6, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-5, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-4, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-3, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-2, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-1, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(0, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(1, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(2, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(3, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(4, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(5, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(6, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(7, -5),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(8, -5),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(-7, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-6, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-5, 4),tileType = TileType.IceFloor},
              new TilePlacement { position =  new Vector2Int(-4, 4),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(-4, 3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-3, 3),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(-2, 2),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(2, 2),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(3, 3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(4, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(5, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(6, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(7, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(7, 3),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(7, 0),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(7, -1),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(8, 2),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(8, 1),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(8, -2),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(8, -3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(8, -4),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(-3, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-2, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-1, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(0, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(1, 4),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(2, 4),tileType = TileType.IceFloor},
               
               new TilePlacement { position = new Vector2Int(1, 3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(0, 3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-1, 3),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-2, 3),tileType = TileType.IceFloor},

               new TilePlacement { position = new Vector2Int(0, 2),tileType = TileType.IceFloor},
               new TilePlacement { position = new Vector2Int(-1, 2),tileType = TileType.IceFloor},
            },
            tilesToClear = new Vector2Int[]
            {
               new Vector2Int(-9, 3),
               new Vector2Int(-9, 1),
               new Vector2Int(-9, 0),
               new Vector2Int(-9, -3),
               new Vector2Int(-9, -4),

               new Vector2Int(-8, -5),
               new Vector2Int(-8, -2),
               new Vector2Int(-8, -1),
               new Vector2Int(-8, 2),
               new Vector2Int(-8, 4),

               new Vector2Int(-7, -5),
               new Vector2Int(-6, -5),
               new Vector2Int(-5, -5),
               new Vector2Int(-4, -5),
               new Vector2Int(-3, -5),
               new Vector2Int(-2, -5),
               new Vector2Int(-1, -5),
               new Vector2Int(0, -5),
               new Vector2Int(1, -5),
               new Vector2Int(2, -5),
               new Vector2Int(3, -5),
               new Vector2Int(4, -5),
               new Vector2Int(5, -5),
               new Vector2Int(6, -5),
               new Vector2Int(7, -5),
               new Vector2Int(8, -5),

               new Vector2Int(-7, 4),
               new Vector2Int(-6, 4),
               new Vector2Int(-5, 4),
               new Vector2Int(-4, 4),

               new Vector2Int(-4, 3),
               new Vector2Int(-3, 3),

               new Vector2Int(-2, 2),
               new Vector2Int(2, 2),

               new Vector2Int(3, 3),
               new Vector2Int(4, 4),
               new Vector2Int(5, 4),
               new Vector2Int(6, 4),
               new Vector2Int(7, 4),
               new Vector2Int(7, 3),

               new Vector2Int(7, 0),
               new Vector2Int(7, -1),

               new Vector2Int(8, 2),
               new Vector2Int(8, 1),
               new Vector2Int(8, -2),
               new Vector2Int(8, -3),
               new Vector2Int(8, -4),

               new Vector2Int(1, 2),
               new Vector2Int(2, 3),
               new Vector2Int(3, 4),

            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(8, 2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(8, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(6, 1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(6, -3), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-7, -2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-7, 0), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-7, -2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-7, -2), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(5, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(-4, 0), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(-3, 2), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(-3, -3), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(2, -2), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                new MonsterPlacement {position = new Vector2Int(2, 2), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
            }
        };
        stages.Add(stage6);



        /*
        StageData stage5 = new StageData
        {
            playerPosition = new Vector2Int(-6, -1),
            tilesToPlace = new TilePlacement[]
            {
                new TilePlacement { position = new Vector2Int(-2, 5), tileType = TileType.Bumper},
            },
            tilesToClear = new Vector2Int[]
            {
                new Vector2Int(-2, 5),
                new Vector2Int(-1, 5),
                new Vector2Int(0, 5), 
                new Vector2Int(1, 5), 
                new Vector2Int(-9, -3),
                new Vector2Int(-9, -4),
                new Vector2Int(-8, -4),
                new Vector2Int(-8, -5),
                new Vector2Int(-7, -5),
                new Vector2Int(-3, -5),
                new Vector2Int(-2, -5),
                new Vector2Int(1, -5),
                new Vector2Int(2, -5),
                new Vector2Int(7, -5),
                new Vector2Int(8, -4),
                new Vector2Int(8, -3),
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
                new MonsterPlacement {position = new Vector2Int(-6, 2), monster = monsterPrefabList[(int)enemyType.ShieldBreaker]},
                new MonsterPlacement {position = new Vector2Int(-3, -1), monster = monsterPrefabList[(int)enemyType.ShieldBreaker]},

                new MonsterPlacement {position = new Vector2Int(-6, 6), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(-6, 7), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(-6, 8), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
                
                new MonsterPlacement {position = new Vector2Int(1, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_small]},
                new MonsterPlacement {position = new Vector2Int(2, -1), monster = monsterPrefabList[(int)enemyType.DashEnemy_Big]},
                new MonsterPlacement {position = new Vector2Int(3, -1), monster = monsterPrefabList[(int)enemyType.ShootEnemy]},
            }
        };

        for (int i = 5; i <= 14; i++)
        {
            for (int j = -9; j <= 8; j++)
            {
                stage5.tilesToPlace.Append(new TilePlacement { position = new Vector2Int(-i, j), tileType = TileType.IceFloor});
            }
        }
        stages.Add(stage5);
        */



        StageData finalStage = new StageData()
        {
            tilesToPlace = new TilePlacement[]
            {
               new TilePlacement { position = new Vector2Int(-9, 3), tileType = TileType.IceFloor}
            },
            tilesToClear = new Vector2Int[]
            {
               new Vector2Int(-9, 3)
            },
            monsterSpawnPositions = new MonsterPlacement[]
            {
            new MonsterPlacement {position = new Vector2Int(12, -1), monster = monsterPrefabList[(int)enemyType.Boss]}, 
            }
        };
        stages.Add(finalStage);
    }


    public void StartStage(int stageIndex)
    {
        isSpawning = true;

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
        Player.transform.position = new Vector3((stage.playerPosition.x + 0.5f) * 2.3717f, (stage.playerPosition.y + 0.5f) * 2.3574f, -1f);

        CameraManager camManager = FindFirstObjectByType<CameraManager>();
        if (camManager != null)
        {
            if (stageIndex == stages.Count - 1)
            {
                camManager.defaultSize = 14f;
                Camera.main.orthographicSize = 14f;
            }
        }

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

        isSpawning = false;
    }

    private void SpawnMonsters(MonsterPlacement[] placements)
    {
        foreach (var placement in placements)
        {
            Vector3 worldPos = new Vector3((placement.position.x + 0.5f)*2.3717f, (placement.position.y + 0.5f)*2.3574f, -1f);
            GameObject monster = Instantiate(placement.monster, worldPos, Quaternion.identity);
            if (monster.CompareTag("Boss"))
            {
                StartBossCombat(monster);
            }
        }
    }

    public BossHealthBar BossHealthBar;
    
    private void StartBossCombat(GameObject boss)
    {
        BossHealthBar.gameObject.SetActive(true);
        BossHealthBar.BossCombatStart();
    }

    public void OnPlayerStageWin()
    {
        SoundManager.Instance.Play(Define.SFX.Success);
        
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

    public void OnGameWin()
    {
        Debug.Log("축하합니다! 게임을 클리어했습니다.");
        successImg.SetActive(true);
        exitBtn.SetActive(true);
        Time.timeScale = 0f;  // 시간 멈춤
    }


    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
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
        failImg.SetActive(true);
        exitBtn.SetActive(true);
        Time.timeScale = 0f;
    }
}
