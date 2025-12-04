using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class TowerGenerate : IDataKey
{
    [SerializeField] private int key;
	[JsonProperty("medievalRate")] [SerializeField] private float medievalRate;
	[JsonProperty("modernRate")] [SerializeField] private float modernRate;
	[JsonProperty("fantasyRate")] [SerializeField] private float fantasyRate;
	[JsonProperty("tier1Rate")] [SerializeField] private float tier1Rate;
	[JsonProperty("tier2Rate")] [SerializeField] private float tier2Rate;
	[JsonProperty("tier3Rate")] [SerializeField] private float tier3Rate;
	[JsonProperty("tier4Rate")] [SerializeField] private float tier4Rate;

	public int Key { get => key; set => key = value; }
	public float MedievalRate { get => medievalRate; private set => medievalRate = value; }
	public float ModernRate { get => modernRate; private set => modernRate = value; }
	public float FantasyRate { get => fantasyRate; private set => fantasyRate = value; }
	public float Tier1Rate { get => tier1Rate; private set => tier1Rate = value; }
	public float Tier2Rate { get => tier2Rate; private set => tier2Rate = value; }
	public float Tier3Rate { get => tier3Rate; private set => tier3Rate = value; }
	public float Tier4Rate { get => tier4Rate; private set => tier4Rate = value; }

    [JsonConstructor]
    public TowerGenerate(int key, float medievalRate, float modernRate, float fantasyRate, float tier1Rate, float tier2Rate, float tier3Rate, float tier4Rate)
    {
        Key = key;
        MedievalRate = medievalRate;
        ModernRate = modernRate;
        FantasyRate = fantasyRate;	
        Tier1Rate = tier1Rate;
        Tier2Rate = tier2Rate;
        Tier3Rate = tier3Rate;
        Tier4Rate = tier4Rate;
    }
}
