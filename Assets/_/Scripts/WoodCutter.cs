using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace WoodCutterTZ
{
    public enum InteractableType
    {
        Tree,
        Home,
        Log
    }

    interface IInteractable
    {
        void Interact();
        InteractableType GetInteractableType();
        Vector3 GetPosition();

    }
    public class WoodCutter : MonoBehaviour
    {
        private NavMeshAgent agent = null;
        private Animator anim = null;
        private Home home = null;
        private void Start()
        {
            home = SceneLogic.Instance.Home;
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            SceneLogic.Instance.TreeUpdated += FindInteraction;
            FindInteraction();
        }

        private void FindInteraction()
        {
            const float radiusSearch = 100;
            IInteractable currentInteractable = PickInteractable(Physics.OverlapSphere(transform.position, radiusSearch));

            if (logs >= 3 || currentInteractable == null)
            {
                currentInteractable = home;
            }

            if (goForCoroutine != null) StopCoroutine(goForCoroutine);
            goForCoroutine = StartCoroutine(GoFor(currentInteractable));


        }

        private IInteractable PickInteractable(Collider[] overlapSphere)
        {
            float closestDistance = float.MaxValue;
            IInteractable currentInteractable = null;
            
            foreach (var hitCollider in overlapSphere)
            {

                IInteractable interactable = hitCollider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    float distance = Vector3.Distance(transform.position, interactable.GetPosition());
                    InteractableType interactableType = interactable.GetInteractableType();

                    if (interactableType == InteractableType.Home) continue;

                    if (currentInteractable == null)
                    {
                        currentInteractable = interactable;
                        closestDistance = distance;
                        continue;
                    }

                    if (currentInteractable.GetInteractableType() == InteractableType.Log)
                    {
                        if (interactableType == InteractableType.Log)
                            if (closestDistance > distance)
                            {
                                currentInteractable = interactable;
                                closestDistance = distance;
                                continue;
                            }
                    }
                    else
                    {
                        if (interactableType == InteractableType.Log)
                        {
                            currentInteractable = interactable;
                            closestDistance = distance;
                            continue;
                        }

                        if (closestDistance > distance)
                        {
                            currentInteractable = interactable;
                            closestDistance = distance;
                            continue;
                        }
                    }
                }
            }

            return currentInteractable;
        }

        private Coroutine goForCoroutine = null;

        private IEnumerator GoFor(IInteractable currentInteractable)
        {
            agent.isStopped = false;
            agent.SetDestination(currentInteractable.GetPosition());
            while (Vector3.Distance(transform.position, currentInteractable.GetPosition()) > 1)
            {
                anim.SetBool("Walk", true);
                yield return new WaitForSeconds(0.3f);
            }
            anim.SetBool("Walk", false);

            agent.SetDestination(transform.position);
            agent.isStopped = true;

            yield return new WaitForEndOfFrame();

            if (interactCorotune != null) StopCoroutine(interactCorotune);
            interactCorotune = StartCoroutine(Interact(currentInteractable));

            goForCoroutine = null;
            yield return null;
        }

        private Coroutine interactCorotune = null;

        private IEnumerator Interact(IInteractable interactable)
        {
            switch (interactable.GetInteractableType())
            {
                case InteractableType.Tree:
                    anim.SetBool("Cut", true);
                    anim.Play("WoodCut");
                    yield return new WaitForSeconds(1);
                    anim.SetBool("Cut", false);
                    interactable.Interact();

                    break;
                case InteractableType.Home:
                    ClearBranches();
                    break;
                case InteractableType.Log:
                    yield return new WaitForSeconds(1);
                    AddlogsOnSpine();
                    interactable.Interact();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return new WaitForEndOfFrame();
            FindInteraction();
            interactCorotune = null;
            yield return null;
        }

        #region Logs on the spine

        private int logs;

        [SerializeField] private Transform spineForLogs = null;
        [SerializeField] private GameObject logPrefab = null;

        private void AddlogsOnSpine()
        {
            logs++;

            Transform log = Instantiate(logPrefab, spineForLogs).transform;

            log.localPosition = new Vector3(0, 1 * logs * 0.25f, 0);

            log.localEulerAngles = new Vector3(0, Random.value * 45, 0);
        }

        private void ClearBranches()
        {
            foreach (Transform log in spineForLogs) Destroy(log.gameObject);
            logs = 0;
        }

        #endregion
    }
}