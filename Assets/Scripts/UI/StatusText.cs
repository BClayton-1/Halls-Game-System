using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusText : MonoBehaviour
{
    private TextMeshProUGUI statusTextUI;


    void Start()
    {
        statusTextUI = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void DisplayStatus(string stTxt)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayStatusCor(stTxt));
    }

    IEnumerator DisplayStatusCor(string stTxt)
    {
        statusTextUI.text = stTxt;
        Color temp = statusTextUI.color;
        temp.a = 1f;
        statusTextUI.color = temp;
        yield return new WaitForSeconds(1);
        for (float i = 1; i > 0; i -= 0.02f)
        {
            temp.a = i;
            statusTextUI.color = temp;
            yield return new WaitForSeconds(0.05f);
        }
        temp.a = 0f;
        statusTextUI.color = temp;
    }

}
