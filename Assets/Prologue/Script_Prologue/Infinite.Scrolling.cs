using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    [Header("Scrolling Settings")]
    public float scrollSpeed = 2f;
    public Vector3 scrollDirection = new Vector3(-1, 0, 0);

    [Header("Loop Settings")]
    public Transform firstSection;
    public Transform secondSection;
    public float sectionWidth = 20f;

    void Start()
    {
        if (firstSection == null && transform.childCount > 0)
        {
            firstSection = transform.GetChild(0);
            Debug.Log($"FirstSection assigné automatiquement: {firstSection.name}");
        }
    }

    void Update()
    {
        if (firstSection == null || secondSection == null)
        {
            return;
        }

        firstSection.position += scrollDirection.normalized * scrollSpeed * Time.deltaTime;
        secondSection.position += scrollDirection.normalized * scrollSpeed * Time.deltaTime;

        float firstPos = GetPositionOnAxis(firstSection.position);
        float secondPos = GetPositionOnAxis(secondSection.position);

        if (scrollDirection.x != 0)
        {
            if (scrollDirection.x > 0 && firstPos > secondPos + sectionWidth)
            {
                RepositionSection(firstSection, secondSection, -sectionWidth);
            }
            else if (scrollDirection.x < 0 && firstPos < secondPos - sectionWidth)
            {
                RepositionSection(firstSection, secondSection, sectionWidth);
            }
        }
        else if (scrollDirection.z != 0)
        {
            if (scrollDirection.z > 0 && GetZPosition(firstSection.position) > GetZPosition(secondSection.position) + sectionWidth)
            {
                RepositionSection(firstSection, secondSection, -sectionWidth);
            }
            else if (scrollDirection.z < 0 && GetZPosition(firstSection.position) < GetZPosition(secondSection.position) - sectionWidth)
            {
                RepositionSection(firstSection, secondSection, sectionWidth);
            }
        }
    }

    float GetPositionOnAxis(Vector3 pos)
    {
        if (scrollDirection.x != 0) return pos.x;
        if (scrollDirection.z != 0) return pos.z;
        return pos.y;
    }

    float GetZPosition(Vector3 pos)
    {
        return pos.z;
    }

    void RepositionSection(Transform toMove, Transform reference, float offset)
    {
        Vector3 newPos = toMove.position;

        if (scrollDirection.x != 0)
        {
            newPos.x = reference.position.x + offset;
        }
        else if (scrollDirection.z != 0)
        {
            newPos.z = reference.position.z + offset;
        }

        toMove.position = newPos;
    }
}
