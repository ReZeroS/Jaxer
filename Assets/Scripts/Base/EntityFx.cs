using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityFx : MonoBehaviour
{
    protected SpriteRenderer sr;


    [Header("Flash Fx")]
    [SerializeField] private float hitDuration;

    [SerializeField] private Material hitMaterial;
    private Material originMaterial;

    [Header("Ailment color")]
    [SerializeField] private Color[] chillColor;

    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment Fx")]
    [SerializeField] private ParticleSystem igniteFx;

    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit Fx")]
    [SerializeField] private GameObject hitFx;

    [SerializeField] private GameObject hitCriticalFx;


    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMaterial = sr.material;
    }

    private void Update()
    {
    }


   


    public IEnumerator FlashFx()
    {
        sr.material = hitMaterial;

        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(hitDuration);
        sr.material = originMaterial;
        sr.color = currentColor;
    }


    public void RedColorBlinkFor(float time, float repeatRate)
    {
        InvokeRepeating(nameof(RedColorBlink), 0, repeatRate);
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    public void CancelColorFor(float time)
    {
        Invoke(nameof(CancelColor), time);
    }

    private void CancelColor()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }


    public void IgniteFxFor(float seconds)
    {
        igniteFx.Play();
        InvokeRepeating(nameof(IgniteColorFx), 0, .3f);
        Invoke(nameof(CancelColor), seconds);
    }

    public void ChillFxFor(float seconds)
    {
        chillFx.Play();
        InvokeRepeating(nameof(ChillColorFx), 0, .3f);
        Invoke(nameof(CancelColor), seconds);
    }

    public void ShockFxFor(float seconds)
    {
        shockFx.Play();
        InvokeRepeating(nameof(ShockColorFx), 0, .3f);
        Invoke(nameof(CancelColor), seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }


    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }


    public void CreateHitFx(Transform hitPoint, bool critical)
    {
        float zRot = Random.Range(-90, 90);
        float xPos = Random.Range(-.5f, .5f);
        float yPos = Random.Range(-.5f, .5f);

        Vector3 hitRotation = new Vector3(0, 0, zRot);
        GameObject hitPrefab = hitFx;
        if (critical)
        {
            hitPrefab = hitCriticalFx;
            float yRot = 0;
            zRot = Random.Range(-45, 45);
            int facingDir = GetComponent<Entity>().facingDir;
            if (facingDir == -1)
            {
                yRot = 180;
            }

            hitRotation = new Vector3(0, yRot, zRot);
        }

        // whether to make particle system follow the hit point
        GameObject newFx =
            Instantiate(hitPrefab, hitPoint.position + new Vector3(xPos, yPos), Quaternion.identity); // , hitPoint);
        newFx.transform.Rotate(hitRotation);

        Destroy(newFx, 0.5f);
    }


   
}