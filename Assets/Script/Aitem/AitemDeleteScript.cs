using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AitemDeleteScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
        {
        if (collision.gameObject.CompareTag("Player"))
            {
            // é©ï™é©êgÇè¡ãé
            Destroy(gameObject);
            }
   
        }

    }
