﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    class PoolObject
    {
        public Transform transform;
        public bool inUse = false;

        public PoolObject(Transform t) { transform = t; }

        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }

    public GameObject Prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public Vector3 defaultSpawnPos;
    public bool spawnImmediate;
    public Vector3 immediateSpawnPos;
    public Vector3 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;

    [System.Serializable]
    public struct YSpawnRange
    {
        public float min;
        public float max;
    }

    private void Awake()
    {
        Configure();
    }

    GameManager game;

    private void Start()
    {
        game = GameManager.Instance;
       
    }

    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverComfirmed;

    }

    private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverComfirmed;
    }

    private void Update()
    {
        if (game.GameOver) return;
        Shift();

        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
        }
    }

    public YSpawnRange ySpawnRange;

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }

        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    void OnGameOverComfirmed()
    {
        
        for(int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    void SpawnImmediate()
    {
        Transform t = GetPoolObject();

        if (!t)
        {
            return;
        }

        Vector3 pos = Vector3.zero;
        pos.x = immediateSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }

    void Spawn()
    {
        Transform t = GetPoolObject();

        if (!t)
        {
            return;
        }

        Vector3 pos = Vector3.zero;
        pos.x = defaultSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        
    }

    void Shift()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.position += -Vector3.right*shiftSpeed*Time.deltaTime;
            CheckDisposedObject(poolObjects[i]);
        }
    }

    void CheckDisposedObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < -defaultSpawnPos.x)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }

    Transform GetPoolObject()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
