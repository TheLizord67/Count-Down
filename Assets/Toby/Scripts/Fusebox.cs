using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Fusebox : MonoBehaviour
{
    [SerializeField] public List<GameObject> connections;
    [SerializeField] public bool off;
    public Sprite onSprite, offSprite;
    [SerializeField] public SpriteRenderer _renderer;
    [SerializeField] public GameObject sparky;
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
        }
        else
        {
            _renderer.sprite = onSprite;
        }
    }

    public IEnumerator SparksFly()
    {
        sparky.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sparky.SetActive(false);
    }
}
