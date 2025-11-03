using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Collections;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

public class SlotObjet : MonoBehaviour
{
    [SerializeField] private int Sockettype = 1 ;
    [SerializeField] public GameObject _PM;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnEnable()
    {
       //m_TriggerContactMonitor.contactAdded += OnObjectadd;
        //m_TriggerContactMonitor.contactRemove -= 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Objet>().ObjectType == Sockettype)
        {
            _PM.GetComponent<PuzzleManager>().UnlockPuzzle(Sockettype);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
