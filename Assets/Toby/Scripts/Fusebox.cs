using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Fusebox : MonoBehaviour
{
    [SerializeField] public List<GameObject> connections;
    [SerializeField] public bool off;
    public Sprite onSprite, offSprite;
    [SerializeField] public SpriteRenderer _renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (off)
        {
            _renderer.sprite = offSprite;
            foreach (var connection in connections)
            {
                connection.SetActive(true);
            }
        }
        else
        {
            _renderer.sprite = onSprite;
            foreach (var connection in connections)
            {
                connection.SetActive(false);
            }
        }
    }
}
