using JetBrains.Annotations;
using UnityEngine;
using System.Collections;



public class PlayerAttack : MonoBehaviour
{
    public Animator _animator;
    public bool _Attack = false;
    public bool HasAttack = false;
    public PlayerMovement PM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
       PM = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Attack" , _Attack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyMovement>() != null)
        {
            if (HasAttack == false)
            {
                
                Debug.Log("J'attaque");
                StartCoroutine(Anim());
            }
          
           
        }
    }

    private IEnumerator Anim()
    {
        if (HasAttack == false)
        {
            PM.speed = 0f;
            HasAttack = true;
            _Attack = true;

            yield return new WaitForSeconds(0.75f);
            

            _Attack = false;
            yield return new WaitForSeconds(0.5f);
            PM.speed = -0.5f;
        }
    }
}
