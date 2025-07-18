using System;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_PrintStat : MonoBehaviour
{
    public TMP_Text _statText;
    public BaseStatComponent _StatComponent;

    private void Update()
    {
        if (_StatComponent == null || _statText == null)
            return;

        StringBuilder sb = new StringBuilder();

        foreach (var attribute in _StatComponent.GetAttributes())
        {
            sb.AppendLine($"{attribute.Key} : {attribute.Value:0.0}");
        }

        _statText.text = sb.ToString();
    }
}
