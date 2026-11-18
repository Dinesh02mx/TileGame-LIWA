using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestXml : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(XmlManager.GetInstance().verification);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            XmlManager.updateXML("kl5",100);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
           Debug.LogError(XmlManager.ReadXml<int>("kl5"));
        }
    }
}
