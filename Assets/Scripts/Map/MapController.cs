using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject MapPrefab;
    private List<GameObject> currentMaps = new List<GameObject>();
    private List<Vector3> currentMapPos; 
    public Action ChangeMap;
    public Vector3 playerMapPosition = Vector3.zero;
    public NavMeshSurface meshSurface;
    public List<Transform> spawners = new List<Transform>();
    public MonsterManager monsterManager;

    /// <summary>
    /// 맵 초기설정
    /// </summary>
    private void Init()
    {
        meshSurface = GetComponent<NavMeshSurface>();
        currentMapPos = new List<Vector3>()
        {
            playerMapPosition,
            playerMapPosition + Vector3.forward*16f,
            playerMapPosition + Vector3.back*16f,
            playerMapPosition + Vector3.right*16f,
            playerMapPosition + Vector3.left*16f,
            playerMapPosition + new Vector3(1,0,1)*16f,
            playerMapPosition + new Vector3(-1,0,1)*16f,
            playerMapPosition + new Vector3(1,0,-1)*16f,
            playerMapPosition + new Vector3(-1,0,-1)*16f
        };

        foreach (var map in currentMapPos)
        {
            GameObject newMap = Instantiate(MapPrefab, map, Quaternion.identity, this.transform);
            currentMaps.Add(newMap);
            MapInfo mapInfo = newMap.GetComponent<MapInfo>();
            mapInfo.Init();
        }
        ChangeMap += ChangeMaps;
        meshSurface.BuildNavMesh();
        monsterManager.spawners = spawners;

    }
    private void Awake()
    {
        Init();

    }

    /// <summary>
    /// 캐릭터 이동시 맵 이동로직
    /// </summary>
    private void ChangeMaps()
    {
        currentMapPos = new List<Vector3>()
        {
            playerMapPosition,
            playerMapPosition + Vector3.forward*16f,
            playerMapPosition + Vector3.back*16f,
            playerMapPosition + Vector3.right*16f,
            playerMapPosition + Vector3.left*16f,
            playerMapPosition + new Vector3(1,0,1)*16f,
            playerMapPosition + new Vector3(-1,0,1)*16f,
            playerMapPosition + new Vector3(1,0,-1)*16f,
            playerMapPosition + new Vector3(-1,0,-1)*16f
        };

        List<Vector3> changePos = currentMapPos.Except(currentMaps.Select(x => x.transform.position)).ToList();
        List<GameObject> changeMap = currentMaps.Where(obj => !currentMapPos.Contains(obj.transform.position)).ToList();

        for (int i = 0; i < changeMap.Count; i++)
        {
            if(changeMap.Count > i)
                changeMap[i].transform.position = changePos[i];
        }
    }
}
