using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    [SerializeField] public AudioSource audioSOurce;
    [SerializeField] public AudioClip collect;
    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSOurce = GetComponent<AudioSource>();
    }


    private void OnEnable() // its called before the start method
    {
        anim.SetTrigger("Spawn");
    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.GetCoin();
        audioSOurce.clip = collect;
        audioSOurce.Play();
        anim.SetTrigger("Collected");
     
    }
}
