using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TextController : MonoBehaviour
{
    [System.Serializable]
public class TalkData
{
    public string speaker;

    [TextArea(2,3)]
    public string message;
}
}
