using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject ob;
    [SerializeField]
    Text txt;
    int t = 0;
    void Start()
    {
        StartCoroutine(timer());
        StartCoroutine(spawn());
    }
    IEnumerator spawn()
    {       
        while (true) {
            Instantiate(ob, transform.position + new Vector3(Random.Range(0,3), Random.Range(1, 3), Random.Range(0, 3)), Quaternion.identity);
            yield return new WaitForSeconds(0.01f);
        };
    }
    IEnumerator timer()
    {
        while (true)
        {
            t++;
            txt.text = t.ToString();
            yield return new WaitForSeconds(1f);
        };
    }
}
