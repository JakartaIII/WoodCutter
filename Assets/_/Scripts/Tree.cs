using UnityEngine;

namespace WoodCutterTZ
{
    public class Tree : MonoBehaviour, IInteractable
    {
        private int health = 3;

        [SerializeField] private GameObject logPrefab = null;

        public void Interact()
        {
            health--;
            if (health <= 0) TreeDestroyed();
        }

        private void TreeDestroyed()
        {
            for (int i = 0; i < 3; i++)
            {
                float SpawnRange = 1;
                Instantiate(logPrefab, transform.position + new Vector3(Random.Range(-SpawnRange,SpawnRange), 0,Random.Range(-SpawnRange, SpawnRange)), transform.rotation);
            }
            Destroy(gameObject);
        }

        public InteractableType GetInteractableType()
        {
            return InteractableType.Tree;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}