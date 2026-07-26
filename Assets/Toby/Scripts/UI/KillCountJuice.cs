using Unity.VisualScripting;
using UnityEngine;

public class KillCountJuice : MonoBehaviour
{

    [SerializeField] private float shrinkSpeed, inflateAmount;

    private Vector3 shrink, inflate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shrink = new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed);
        inflate = new Vector3(inflateAmount, inflateAmount, inflateAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.y > 1)
            transform.localScale -= shrink * Time.deltaTime;

        if (transform.localScale.y < 1)
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void Inflate()
    {
        transform.localScale += inflate;
        Debug.Log("inflated");
    }

}
