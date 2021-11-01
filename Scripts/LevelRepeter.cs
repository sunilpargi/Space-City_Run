using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRepeter : MonoBehaviour
{
    public GameObject newEnvirnment;
    public GameObject currentEnvirnment;
    private GameObject oldEnvirnment;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Start"))
        {
            if (oldEnvirnment != null)
            {
                Destroy(oldEnvirnment);
            }
           
        }

        if (other.CompareTag("Middle"))
        {
            SpawnLevel();
        }
    }

    private void SpawnLevel()
    {
        oldEnvirnment = currentEnvirnment;
        currentEnvirnment = Instantiate(newEnvirnment, currentEnvirnment.transform.GetChild(4).transform.position, Quaternion.identity) as GameObject;
    }
}
