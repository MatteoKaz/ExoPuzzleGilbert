using UnityEngine;

using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public bool Puzzle1 = false;
    public bool Puzzle2 = false;
    public bool Puzzle3 = false;
    public bool isVictory = false;

    [Header("Door Settings")]
    public SlidingDoor door;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        
    }

    public void UnlockPuzzle(int arg)
    {
        if (arg == 1) 
        {
            Puzzle1 = true;
        }

        if (arg == 2)
        {
            Puzzle2 = true;
        }

        if (arg == 3)
        {
            Puzzle3 = true; 
        }

        if (Puzzle1 && Puzzle2 && Puzzle3 == true)
        {
            isVictory = true;
            Debug.Log("HOURRA - Énigme résolue !");
            
            if (door != null)
            {
                door.OpenDoor();
            }
        }
    }
}
