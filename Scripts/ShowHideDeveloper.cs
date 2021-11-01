using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideDeveloper : MonoBehaviour
{
    public bool show = true;
    public bool helpshow = true;
    public bool gameovershow = true;
    public AudioClip clickClip;
    public void ShowHideInfoPanel()
    {
        if (show)
        {

            GetComponent<Animator>().SetTrigger("Show");
            show = false;
        }

        else
        {
            GetComponent<Animator>().SetTrigger("Hide");
            show = true;
        }
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
    }

    public void ShowHideHelpPanel()
    {
        if (helpshow)   
        {

            GetComponent<Animator>().SetTrigger("Show");
            helpshow = false;
        }

        else
        {
            GetComponent<Animator>().SetTrigger("Hide");
            helpshow = true;
        }
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
    }

    public void ShowHideGameoverPanel()
    {
        if (gameovershow)
        {

            GetComponent<Animator>().SetTrigger("Show");
            gameovershow = false;
        }

        else
        {
            GetComponent<Animator>().SetTrigger("Hide");
            gameovershow = true;
        }
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
    }

    public void Test()
    {
        Debug.Log(1);
    }
}
