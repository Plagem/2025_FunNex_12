using System.Collections.Generic;
using UnityEngine;

public class StatPassive : BasePassive
{
    public StatPassive(BaseStatComponent statComponent, ModifyInfo modifyInfo)
    {
        ModifyInfos = new List<ModifyInfo>();
        ModifyInfo tempModify = new ModifyInfo
        {
            ModifyType = modifyInfo.ModifyType,
            Magnitude = modifyInfo.Magnitude,
            TargetStat = modifyInfo.TargetStat
        };
        ModifyInfos.Add(tempModify);
    }
}
