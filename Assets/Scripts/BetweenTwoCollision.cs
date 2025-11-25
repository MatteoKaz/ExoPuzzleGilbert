using System.Collections;
using UnityEngine;

public class BetweenTwoCollision : MonoBehaviour
{
    bool hitTop = false;
    bool hitBottom = false;
    bool isCrushed = false;
    Coroutine separationWatcher = null;

    [Header("Colliders")]
    public Collider characterCollider;
    public Collider cubeCollider;
    public string groundTag = "Ground";
    public string cubeTag = "CrushCube";

    void Start()
    {

        if (characterCollider != null && cubeCollider != null)
            Physics.IgnoreCollision(characterCollider, cubeCollider, false);
    }

    void OnCollisionStay(Collision col)
    {

        foreach (var contact in col.contacts)
        {

            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                hitBottom = true;


            if (Vector3.Dot(contact.normal, Vector3.down) > 0.5f)
                hitTop = true;
        }


        if (hitTop && hitBottom && !isCrushed && col.collider.CompareTag(cubeTag))
        {
            OnCrush();
        }
    }

    void LateUpdate()
    {
        hitTop = false;
        hitBottom = false;
    }

    void OnCrush()
    {
        if (isCrushed) return;
        isCrushed = true;

        Debug.Log("OnCrush: ignore collision with cube");


        Physics.IgnoreCollision(characterCollider, cubeCollider, true);


        if (separationWatcher != null) StopCoroutine(separationWatcher);
        separationWatcher = StartCoroutine(WatchForSeparationAndReactivate());
    }

    IEnumerator WatchForSeparationAndReactivate()
    {

        yield return null;


        while (true)
        {

            Vector3 direction;
            float distance;
            bool isOverlapping = Physics.ComputePenetration(
                characterCollider, characterCollider.transform.position, characterCollider.transform.rotation,
                cubeCollider, cubeCollider.transform.position, cubeCollider.transform.rotation,
                out direction, out distance
            );

            if (!isOverlapping)
            {

                Physics.IgnoreCollision(characterCollider, cubeCollider, false);
                isCrushed = false;
                separationWatcher = null;
                Debug.Log("Separation detected: collision with cube re-enabled");
                yield break;
            }


            yield return new WaitForFixedUpdate();
        }
    }


    void OnDisable()
    {
        if (characterCollider != null && cubeCollider != null)
            Physics.IgnoreCollision(characterCollider, cubeCollider, false);
    }

    void OnDestroy()
    {
        if (characterCollider != null && cubeCollider != null)
            Physics.IgnoreCollision(characterCollider, cubeCollider, false);
    }
}
