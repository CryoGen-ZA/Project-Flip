using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/New Card Info")]
public class CardInfoSO : ScriptableObject
{
    public string themeName;
    public Sprite cardGraphicFront;
    public Sprite cardGraphicBack;
    public Sprite[] cardIcons;
}
