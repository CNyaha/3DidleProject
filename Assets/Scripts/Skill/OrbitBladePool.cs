using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// OrbitBlade 풀링 비어있는 blade에 프리팹을 넣고 비활성화 시 다시 Queue에 등록 만약 blade프리팹이 모자르다면 새로 만들고 오브젝트가 모자르다면 생성시킵니다.
public class OrbitBladePool : MonoBehaviour
{
    private static OrbitBladePool instance;
    public static OrbitBladePool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("OrbitBladePool").AddComponent<OrbitBladePool>();
            }
            return instance;
        }
    }


    public GameObject bladePrefab;
    public int initalPoolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < initalPoolSize; i++)
        {
            GameObject blade = Instantiate(bladePrefab, this.transform);
            blade.SetActive(false);
            pool.Enqueue(blade);
        }
    }

    // Queue에서 오브젝트를 하나 Dequeue를 한 후 반환합니다.
    public GameObject GetBlade()
    {
        if (pool.Count > 0)
        {
            GameObject blade = pool.Dequeue();
            blade.SetActive(true);
            return blade;
        }
        else
        {
            GameObject newBlade = Instantiate(bladePrefab);
            return newBlade;
        }
    }

    public void ReturnBlade(GameObject blade)
    {
        blade.SetActive(false);
        pool.Enqueue(blade);
    }

}
