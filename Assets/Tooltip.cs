using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Image tooltipBackground;
    public GameObject header;
    public GameObject content;
    public bool isTooltipActive = false;
    // Start is called before the first frame update
    void Start()
    {

    }



    public void ToggleButton()
    {
        isTooltipActive = !isTooltipActive;
        tooltipBackground.enabled = isTooltipActive;
        tooltipBackground.gameObject.GetComponent<ContentSizeFitter>().enabled = isTooltipActive;
        header.SetActive(isTooltipActive);
        content.SetActive(isTooltipActive);
    }
}
