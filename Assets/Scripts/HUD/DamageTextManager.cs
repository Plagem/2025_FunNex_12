using System;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    private static DamageTextManager _instance = null;

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

    public static DamageTextManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }

    public GameObject DamageTextPrefab;
    
    public void CreateDamageText(Vector3 hitPoint, float Damage)
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        GameObject damageText = Instantiate(DamageTextPrefab, hitPoint, Quaternion.identity, canvas.transform);
        damageText.GetComponent<TMP_Text>().SetText($"{(int)Damage}");
    }
}
