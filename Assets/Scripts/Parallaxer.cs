using UnityEngine;
using UnityEngine.Serialization;

public class Parallaxer : MonoBehaviour
{
    private class PoolObject
    {
        public readonly Transform transform;
        public bool inUse;

        public PoolObject(Transform t) { transform = t; }

        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }

    [FormerlySerializedAs("Prefab")] 
    public GameObject prefab;
    
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public Vector3 defaultSpawnPos;
    public bool spawnImmediate;
    public Vector3 immediateSpawnPos;
    // public Vector3 targetAspectRatio;

    private float _spawnTimer;
    // private float _targetAspect;
    private PoolObject[] _poolObjects;

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

    GameManager _game => GameManager.instance;

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
        if (_game.GameOver) return;
        Shift();

        _spawnTimer += Time.deltaTime;
        
        if (_spawnTimer <= spawnRate) return;
        
        Spawn();
        _spawnTimer = 0;
    }

    public YSpawnRange ySpawnRange;

    void Configure()
    {
        // _targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        _poolObjects = new PoolObject[poolSize];

        for (var i = 0; i < poolSize; i++)
        {
            var go = Instantiate(prefab);
            var t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            _poolObjects[i] = new PoolObject(t);
        }

        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    private void OnGameOverComfirmed()
    {
        
        for(var i = 0; i < _poolObjects.Length; i++)
        {
            _poolObjects[i].Dispose();
            _poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    private void SpawnImmediate()
    {
        var t = GetPoolObject();

        if (!t)
        {
            return;
        }

        var pos = Vector3.zero;
        pos.x = immediateSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }

    void Spawn()
    {
        var t = GetPoolObject();

        if (!t)
        {
            return;
        }

        var pos = Vector3.zero;
        pos.x = defaultSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        
    }

    private void Shift()
    {
        foreach (var t in _poolObjects)
        {
            t.transform.position += -Vector3.right * (shiftSpeed * Time.deltaTime);
            CheckDisposedObject(t);
        }
    }

    private void CheckDisposedObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x >= -defaultSpawnPos.x) return;
        
        poolObject.Dispose();
        poolObject.transform.position = Vector3.one * 1000;
    }

    private Transform GetPoolObject()
    {
        foreach (var t in _poolObjects)
        {
            if (t.inUse) continue;
            
            t.Use();
            return t.transform;
        }

        return null;
    }
}
