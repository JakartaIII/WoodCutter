using UnityEngine;

namespace WoodCutterTZ
{
    public class Home : MonoBehaviour, IInteractable
    { 

        public void Interact()
        { 
        }

        public InteractableType GetInteractableType()
        {
            return InteractableType.Home;
        }

        Vector3 IInteractable.GetPosition()
        {
            return transform.position;
        }
         
    }
}