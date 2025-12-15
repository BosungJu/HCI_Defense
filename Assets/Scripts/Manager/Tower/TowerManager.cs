using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [Header("Scriptable Objects")]
    public TowerData towerData;

    [Header("Towers")]
    public List<TowerObject> Towers { get; set; } = new List<TowerObject>();


    [SerializeField] private int selectTower = -1;
    public int SelectTower
    {
        get => selectTower;
        set
        {
            if (selectTower != -1)
            {
                if (!Towers[value].UpgradeTower(Towers[selectTower]))
                {
                    selectTower = value;
                }
                else 
                {
                    selectTower = -1;
                }
            }
            else
            {
                selectTower = value;
            }
        }
    }
}