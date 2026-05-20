using UnityEngine;


[System.Serializable]
public class TalkLine
{
    public string speaker;

    [TextArea(2,5)]
    public string message;
}