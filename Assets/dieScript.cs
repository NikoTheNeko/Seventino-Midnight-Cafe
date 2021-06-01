using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieScript : MonoBehaviour
{
    public ParticleSystem par;
    // Start is called before the first frame update
    void Start()
    {
        var em = par.emission;
        em.enabled = true;
    }

    // Update is called once per frame
    public void dieNow()
    {
        var em = par.emission;
        em.enabled = true;
    }
}
