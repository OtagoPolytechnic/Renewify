using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Modal : MonoBehaviour
{
    public Text title; //Setup of the components of the modal
    public Text subTitle;
    public Button confirm;
    public Button deny;

    public event System.Action OnAccept; // Event for accepting the modal


    public void Initialize(string title, string subTitle)
    {
        this.title.text = title; //Setting the title and subtitle of the modal
        this.subTitle.text = subTitle;
    }

    public void Decline()
    {
        Destroy(gameObject);
    }
    public void Accept()
    {
      Destroy(gameObject);
      OnAccept?.Invoke(); //When the accept button is clicked, the modal is destroyed and the action is invoked
    }


}
