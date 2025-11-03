using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public bool Puzzle1 = false;
    public bool Puzzle2 = false;
    public bool Puzzle3 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockPuzzle(int arg)
    {
        if (arg == 1) 
        {
        Puzzle1 =  true;
        }

        if (arg == 2)
        {
            Puzzle2 = true;
        }

        if (arg == 3)
        {
            Puzzle3 = true; 
        }

        if (Puzzle1 && Puzzle2 & Puzzle3 == true)

        {
            Debug.Log("hOURRA");
        }

    }
}
