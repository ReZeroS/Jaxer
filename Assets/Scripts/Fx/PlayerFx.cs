using Unity.Cinemachine;
using UnityEngine;

public class PlayerFx : EntityFx
{
    [Header("Screen Fx")]
    private CinemachineImpulseSource screenShakeSource;

    [SerializeField] private float shakeMultiplier;
    [SerializeField] private Vector3 shakeSwordImpact;
    [SerializeField] private Vector3 shakeHighDamageImpact;


    
    [Header("After image fx")]
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private float colorLoseRate;
    [SerializeField] private GameObject afterImagePrefab;
    private float afterImageCooldownTimer;


    [Header("Interact Env Fx")]
    [SerializeField] private ParticleSystem dustFx;

    protected override void Start()
    {
        base.Start();
        screenShakeSource = GetComponent<CinemachineImpulseSource>(); 
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }
    
    public void ScreenShakeForSwordImpact(int facingDir)
    {
        ScreenShake(facingDir, shakeSwordImpact);
    }

    public void ScreenShakeForHighDamageImpact(int facingDir)
    {
        ScreenShake(facingDir, shakeHighDamageImpact);
    }


    private void ScreenShake(int facingDir, Vector3 shakePower)
    {
        screenShakeSource.DefaultVelocity = new Vector3(shakePower.x * facingDir, shakePower.y) * shakeMultiplier;
        screenShakeSource.GenerateImpulse();
    }
    
    
    public void CreateAfterImageFx()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newFx = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newFx.GetComponent<AfterImageFx>().SetUpAfterImage(sr.sprite, colorLoseRate);
        }
    }
    
    public void PlayDustFx()
    {
        dustFx?.Play();
    }

}
