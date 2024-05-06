using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Image tooltipBackground;
    public GameObject header;
    public GameObject content;
    public Image toggle;
    public bool isTooltipActive = false;
    // Start is called before the first frame update
    void Start()
    {
        ToggleButton();

    }



    public void ToggleButton()
    {
        isTooltipActive = !isTooltipActive;

        tooltipBackground.enabled = isTooltipActive;

        header.SetActive(isTooltipActive);
        content.SetActive(isTooltipActive);
        toggle.GetComponent<Image>().color = isTooltipActive ? Color.gray :Color.yellow;
    }


    public void SetTitle(string title)
    {
        header.GetComponent<TMP_Text>().text = title;
    }
    public void SetContent(string content)
    {
        this.content.GetComponent<TMP_Text>().text = content;
    }
}
