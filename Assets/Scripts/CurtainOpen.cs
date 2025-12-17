using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class CurtainOpen : MonoBehaviour
{
    public Animator curtainAnim;
    public bool _open = false;
    public float TimeToOpen = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curtainAnim = GetComponent<Animator>();
        _open = false;
        StartCoroutine(Anim());
       
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void Awake()
    {

    }
    private IEnumerator Anim()
    {
        if (_open == false )
        {
            Debug.Log("JNJNFNJD");
            yield return new WaitForSeconds(1f);
            _open = true;
            curtainAnim.SetBool("Launch", true);
        }
        
    }
}
