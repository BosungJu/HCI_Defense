using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Monster : IDataKey
{
    [SerializeField] private int key;
    [JsonProperty("speed")][SerializeField] private float speed;
    [JsonProperty("health")][SerializeField] private int health;
    [JsonProperty("damage")][SerializeField] private int damage;
    [JsonProperty("reward")][SerializeField] private int reward;
    [JsonProperty("name")][SerializeField] private string name;

    public int Key { get => key; set => key = value; }
    public float Speed { get => speed; private set => speed = value; }
    public int Health { get => health; private set => health = value; }
    public int Damage { get => damage; private set => damage = value; }
    public int Reward { get => reward; private set => reward = value; }
    public string Name { get => name; private set => name = value; }

    [SerializeField] private List<Animation> monsterPrefab;
    public List<Animation> MonsterPrefab { get => monsterPrefab; private set => monsterPrefab = value; }

    [JsonConstructor]
    public Monster(int key, float speed, int health, int damage, int reward, string name)
    {
        Key = key;
        Speed = speed;
        Health = health;
        Damage = damage;
        Reward = reward;
        Name = name;

        InitailzedData();
    }

    public void InitailzedData()
    {
        // TODO animation load
        Resources.Load<Transform>($"MonsterPrefabs/{Name}");
    }
}
