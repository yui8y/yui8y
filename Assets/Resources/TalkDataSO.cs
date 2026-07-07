using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Talk Data")]
public class TalkDataSO : ScriptableObject
{
    public List<TalkLine> lines;
}