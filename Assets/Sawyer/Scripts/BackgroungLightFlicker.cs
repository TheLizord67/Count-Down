using UnityEngine;

public class BackgroungLightFlicker : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Yellow(bool yellow)
    {
        if (yellow)
        {
            mySprite.color = onColor;
        }
        else
        {
            mySprite.color = offColor;
        }
    }
}
