using UnityEngine;


[System.Serializable]
public class TalkLine
{
    public string speaker;

    [TextArea(2,3)]
    public string message;
}