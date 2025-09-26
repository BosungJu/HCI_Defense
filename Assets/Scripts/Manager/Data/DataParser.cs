using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class DataParser
{
    public const string MonsterTablePath = "Data/MonsterData";
    public const string MonsterWaveTablePath = "Data/MonsterWaveData";
    
    /// <summary>
    /// T 타입의 클래스를 JSON 파일에서 읽어와 Dictionary 형태로 반환합니다.
    /// </summary>
    /// <typeparam name="T">abstract IDataKey</typeparam>
    /// <param name="dataPath">json파일의 경로 위에 변수로 선언.</param>
    /// <returns></returns>
    public static Dictionary<int, T> GetDataTable<T>(string dataPath) where T : IDataKey
    {
        TextAsset textAsset = Resources.Load<TextAsset>(dataPath);
        if (textAsset != null)
        {
            using (StringReader reader = new StringReader(textAsset.text))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var datas = serializer.Deserialize<Dictionary<int, T>>(jsonReader);

                    // key 값을 클래스에 할당
                    foreach (var keyValuePair in datas)
                    {
                        keyValuePair.Value.Key = keyValuePair.Key;
                    }
                    return datas;
                }
            }
        }

        Debug.LogError($"File does not exist or could not be loaded: {dataPath}");
        return null;
    }
}