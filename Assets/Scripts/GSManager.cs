using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class GSManager : MonoBehaviour
{
    void Start()
    {
        UnityGoogleSheet.LoadFromGoogle<int, SampleSheet.Data>((list, map) => {
            list.ForEach(x => {
                Debug.Log(x.intValue);
            });
        }, true); 

    }
}
