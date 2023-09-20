using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.IO;
using System.Linq;
using System.IO;
using TMPro;

public class GetText : MonoBehaviour
{
    [SerializeField]
    private string path;

    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        string readFromFilePath = Application.streamingAssetsPath + path + ".txt";

        string textToDisplay = File.ReadAllText(readFromFilePath);
        text.text = textToDisplay;
    }
}
