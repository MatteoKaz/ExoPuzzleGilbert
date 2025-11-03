using System.Net.Sockets;
using UnityEngine;

public class Objet : MonoBehaviour
{
    [SerializeField] public int ObjectType = 1;
    [SerializeField] public GameObject Manager;
    private Vector3 Grow= new Vector3(25,25,25);
    private Vector3 Base = new Vector3(15, 15, 15);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Lerper()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool grosWing = Manager.GetComponent<PuzzleManager>().isVictory;
        if (grosWing)
        {
            transform.localScale = Vector3.Lerp(Base, Grow, Time.deltaTime * 4);
        }
    }

    
}
