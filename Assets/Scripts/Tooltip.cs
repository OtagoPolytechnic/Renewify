using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private  Image tooltipBackground;
    [SerializeField] private  GameObject header;
    [SerializeField] private  GameObject content;
    [SerializeField] private Image toggle;
    [SerializeField] private bool isTooltipActive = false;
    // Start is called before the first frame update
    void Start()
    {
        ToggleButton();

    }


    /// <summary>
    /// Toggles the tooltip on and off
    /// </summary>
    public void ToggleButton()
    {
        isTooltipActive = !isTooltipActive;

        tooltipBackground.enabled = isTooltipActive;

        header.SetActive(isTooltipActive);
        content.SetActive(isTooltipActive);
        toggle.GetComponent<Image>().color = isTooltipActive ? Color.gray :Color.yellow;
    }


    /// <summary>
    ///     Sets the title of the tooltip
    /// </summary>
    /// <param name="title">The Title of the tooltip</param>
    public void SetTitle(string title)
    {
        header.GetComponent<TMP_Text>().text = title;
    }
    /// <summary>
    ///     Sets the content of the tooltip
    /// </summary>
    /// <param name="content">The content paragraph of the tooltip</param>
    public void SetContent(string content)
    {
        this.content.GetComponent<TMP_Text>().text = content;
    }
}
