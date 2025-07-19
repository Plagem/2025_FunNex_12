using System;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    private static DamageTextManager _instance = null;
    public static DamageTextManager Instance { get { Init(); return _instance; } }

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
    

    public GameObject DamageTextPrefab;
    
    public void CreateDamageText(Vector3 hitPoint, float Damage)
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        GameObject damageText = Instantiate(DamageTextPrefab, hitPoint, Quaternion.identity, canvas.transform);
        damageText.GetComponent<DamageText>().Init($"{(int)Damage}");
    }
}
