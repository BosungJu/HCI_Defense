using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [Header("Scriptable Objects")]
    public TowerData towerData;

    [Header("Towers")]
    public List<TowerObject> Towers { get; set; } = new List<TowerObject>();

    [Header("Gold UI")]
    public TMP_Text goldText;

    public TowerObject PossessionTowerObject
    {
        get;
        private set;
    }

    [SerializeField] private int gold = 30;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;

            goldText.SetText($"Gold: {gold}");
        }
    }
    public const int towerGenerateGold = 10;

    [SerializeField] private int selectTower = -1;
    public int SelectTower
    {
        get => selectTower;
        set
        {
            if (PossessionTowerObject != null)
            {
                return;
            }

            if (selectTower != -1 && selectTower != value)
            {
                if (!Towers[value].UpgradeTower(Towers[selectTower]))
                {
                    selectTower = value;
                }
                else 
                {
                    Destroy(Towers[selectTower].gameObject);
                    Towers[selectTower] = null;
                    selectTower = -1;
                }
            }
            else
            {
                selectTower = value;
            }

            foreach (TowerObject t in Towers)
            {
                if (selectTower != -1)
                {
                    t.TowerOutlineOnOff(t == Towers[selectTower]);
                }
            }
        }
    }

    public void PossessionTower()
    {
        if (selectTower != -1 && PossessionTowerObject == null)
        {
            Towers[selectTower].Possession();
            PossessionTowerObject = Towers[selectTower];
            selectTower = -1;
        }
    }

    public void OutPossessionTower()
    {
        if (PossessionTowerObject != null)
        {
            PossessionTowerObject.OutPossession();
            PossessionTowerObject = null;
            selectTower = -1;
        }
    }
}