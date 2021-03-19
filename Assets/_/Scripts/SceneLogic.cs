using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace WoodCutterTZ
{ 
    public class SceneLogic : MonoBehaviour
    {
        public static SceneLogic Instance;

        public event Action TreeUpdated;

        public Home Home = null;
        [SerializeField] private int treesToSpawn = 0;
        [SerializeField] private GameObject treePrefab = null;

        [SerializeField] private int logsToSpawn = 0;
        [SerializeField] private GameObject logPrefab = null;
        // Start is called before the first frame update
        void Awake()
        {
            if(Instance!=null) Debug.LogError("2 singletons");
            Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < treesToSpawn; i++)
            {
                SpawnTree();
            }
            for (int i = 0; i < logsToSpawn; i++)
            {
                SpawnLog();
            }
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.Escape)) Application.Quit();
        }


        [ContextMenu("SpawnTree")]
        public void SpawnTree()
        {
            float SpawnRange = 13;
            Instantiate(treePrefab, transform.position + new Vector3(Random.Range(-SpawnRange, SpawnRange), 0, Random.Range(-SpawnRange, SpawnRange)), transform.rotation);
            TreeUpdated?.Invoke();
        }

        [ContextMenu("SpawnLog")]
        public void SpawnLog()
        {
            float SpawnRange = 13;
            Instantiate(logPrefab, transform.position + new Vector3(Random.Range(-SpawnRange, SpawnRange), 0, Random.Range(-SpawnRange, SpawnRange)), transform.rotation);
            TreeUpdated?.Invoke();
        }

        public void SceneRestart()
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
