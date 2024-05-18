using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    [CreateAssetMenu(menuName = "Cards/New Card Info")]
    public class CardInfoSO : ScriptableObject
    {
        public string themeName;
        public Sprite cardGraphicFront;
        public Sprite cardGraphicBack;
        public Sprite[] cardIcons;

        public Vector2 cardDimensions;
        public Vector2 cardPadding;
        public float flipTime = 1f;
        
        public AudioClip flipSfx;
        public AudioClip matchConfirmedSfx;
    }
}
