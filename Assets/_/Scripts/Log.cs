using UnityEngine;

namespace WoodCutterTZ
{
    public class Log : MonoBehaviour, IInteractable
    { 

        public void Interact()
        {
            Destroy(gameObject);
        }

        public InteractableType GetInteractableType()
        {
            return InteractableType.Log;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}