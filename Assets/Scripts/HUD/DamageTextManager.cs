using System;
using TMPro;
using UnityEngine;

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
        }
    }

    private readonly string _damageTextPath = "Prefabs/DamageText";
    
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
    }
    

    private GameObject _damageTextPrefab;

    private void LoadDamageTextPrefab()
    {
        _damageTextPrefab = Resources.Load<GameObject>(_damageTextPath);
    }
    
    public void CreateDamageText(Vector3 hitPoint, float Damage)
    {
        if (_damageTextPrefab == null)
        {
            LoadDamageTextPrefab();   
        }
        
        Canvas canvas = FindAnyObjectByType<Canvas>();
        GameObject damageText = Instantiate(_damageTextPrefab, hitPoint, Quaternion.identity, canvas.transform);
        damageText.GetComponent<DamageText>().Init($"{(int)Damage}");
    }
}
