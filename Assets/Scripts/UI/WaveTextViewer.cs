using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTextViewer : MonoBehaviour
{
    private TMP_Text waveText;

    public TMP_Text WaveText { get => waveText; set => waveText = value; }

    public void Start()
    {
        MonsterManager.Instance.waveEndEvent += UpdateWaveText;
        UpdateWaveText();
    }

    private void UpdateWaveText()
    {
        waveText.SetText($"Wave {MonsterManager.Instance.CurrentWave}");
    }
}
