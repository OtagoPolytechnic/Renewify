using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public GameObject windmillPrefab;
    private Animator windmillAnimator;


    // Start is called before the first frame update
    void Start()
    {
        windmillAnimator = windmillPrefab.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //bool isTileConnected = WirePlacement.Instance.isTileConnected(1);

        windmillAnimator.SetBool("connected". isTileConnected);
    }

    
}
