using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageTextManager : MonoBehaviour
{
    private static DamageTextManager _instance = null;
    public static DamageTextManager Instance { get { Init(); return _instance; } }
    DamageTextManager() { }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject dtm = GameObject.Find("DamageTextManager");
            if (dtm == null)
            {
                dtm = new GameObject("DamageTextManager");
                dtm.AddComponent<DamageTextManager>();
                DontDestroyOnLoad(dtm);
            }
            _instance = dtm.GetComponent<DamageTextManager>();
        }
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        LoadPrefab();
    }
    
    private GameObject _starEffectPrefab;
    private GameObject _damageTextPrefab;
    
    private readonly string _damageTextPath = "Prefabs/DamageText";
    private readonly string _starEffectPath = "Prefabs/StarEffect";

    private void LoadPrefab()
    {
        _damageTextPrefab = Resources.Load<GameObject>(_damageTextPath);
        _starEffectPrefab = Resources.Load<GameObject>(_starEffectPath);
    }
    
    public DamageText CreateDamageText(Canvas canvas)
    {
        if (_damageTextPrefab == null)
        {
            LoadPrefab();   
        }
        
        GameObject damageText = Instantiate(_damageTextPrefab, canvas.transform);

        return damageText.GetComponent<DamageText>();
    }

    public void SpawnStarEffects(Vector3 center)
    {
        int count = Random.Range(3, 5); // 3~4개

        for (int i = 0; i < count; i++)
        {
            // 초기 위치: 중심에서 약간 안쪽
            Vector3 spawnPos = center;

            // 바깥 방향 랜덤
            Vector2 dir2D = Random.insideUnitCircle.normalized;
            Vector3 direction = new Vector3(dir2D.x, dir2D.y, 0f);

            GameObject star = Instantiate(_starEffectPrefab, spawnPos, Quaternion.identity);

            float distance = Random.Range(0.5f, 1.0f);     // 튕겨 나가는 거리
            float fadeTime = Random.Range(0.3f, 0.5f);     // 지속 시간

            star.GetComponent<StarEffect>().Init(direction, distance, fadeTime);
        }
    }
}
