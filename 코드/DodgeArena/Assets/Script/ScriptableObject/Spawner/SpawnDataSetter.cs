using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnDataSetter", menuName = "Spawner/SpawnDataSetter")]
public class SpawnDataSetter : ScriptableObject
{
    [SerializeField]
    private int risk;
    [SerializeField]
    private int returns;

    public SpawnData GenerateNewSpawnData()
    {
        SpawnData output = new SpawnData(risk, returns);
        return output;
    }
}
