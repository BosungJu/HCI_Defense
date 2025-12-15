using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Tower : IDataKey
{
    [SerializeField] private int key;
    [JsonProperty("name")]        [SerializeField] private string name;
    [JsonProperty("damage")]      [SerializeField] private int damage;
    [JsonProperty("attackSpeed")] [SerializeField] private float attackSpeed;
    [JsonProperty("sellPrice")]   [SerializeField] private int sellPrice;
    [JsonProperty("towerType")]   [SerializeField] private TowerType towerType;
    [JsonProperty("fantasyType")] [SerializeField] private FantasyType fantasyType;
    [JsonProperty("towerTier")]   [SerializeField] private int towerTier;

    public int Key { get => key; set => key = value; }
    public string Name { get => name; private set => name = value; }
    public int Damage { get => damage; private set => damage = value; }
    public float AttackSpeed { get => attackSpeed; private set => attackSpeed = value; }
    public int SellPrice { get => sellPrice; private set => sellPrice = value; }
    public TowerType TowerType { get => towerType; private set => towerType = value; }
    public int TowerTier { get => towerTier; private set => towerTier = value; }
    public FantasyType FantasyType { get => fantasyType; private set => fantasyType = value; }

    [SerializeField] private List<Animation> towerPrefab;
    public List<Animation> TowerPrefab { get => towerPrefab; private set => towerPrefab = value; }

    [JsonConstructor]
    public Tower(int key, string name, int damage, float attackSpeed, int sellPrice,
                 TowerType towerType, FantasyType fantasyType, int towerTier)
    {
        Key = key;
        Name = name;
        Damage = damage;
        AttackSpeed = attackSpeed;
        SellPrice = sellPrice;
        TowerType = towerType;
        FantasyType = fantasyType;
        TowerTier = towerTier;

        InitailzedData();
    }

    public void InitailzedData()
    {
        // TODO animation load (if needed)
        AnimationClip clip = Resources.Load<AnimationClip>($"Tower/Animation/GunPlay");
    }
}

public enum TowerType
{
    Medieval,
    Modern,
    Fantasy
}

public enum FantasyType
{
    None,
    Fire,
    Ice
}