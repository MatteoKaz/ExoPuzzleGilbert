using UnityEngine;
using System.Collections;
using System.Globalization;


public class Death : MonoBehaviour
{
    private Vector3 RespawnLoc;
    [SerializeField] private GameObject Character;
    private float _deathTimer = 0.8f;
    private SpriteRenderer _sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RespawnLoc = Character.transform.position;
        RespawnLoc = new Vector3(RespawnLoc.x,RespawnLoc.y+1,RespawnLoc.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            Dead();
        }
    }

    public void Dead()
    {
        StartCoroutine(ShadowTimerControl());
        Character.transform.position = RespawnLoc;  
    }

    private IEnumerator ShadowTimerControl()
    {
        _sprite = Character.gameObject.GetComponent<SpriteRenderer>();
        _sprite.enabled = false;
        Character.gameObject.GetComponent<PlayerMovement>().speed = 0f;
        yield return new WaitForSeconds(_deathTimer);
        Character.transform.position = RespawnLoc;
        _sprite.enabled = true;
    }
}
