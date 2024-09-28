using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{

    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<string> keyCodeList;
    

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotkeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisappear = true;
    
    
    public int amountOfAttacks = 4;
    public float cloneCooldown = 0.3f;
    private float cloneAttackTimer;
    

    private List<Transform> targets = new();
    private List<GameObject> createdHotkeys = new();


    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float maxCount, float growSp, float shrinkSp, int attackCount, 
        float cloneAttackCool, float blackholeDuration)
    {
        maxSize = maxCount;
        growSpeed = growSp;
        shrinkSpeed = shrinkSp;
        amountOfAttacks = attackCount;
        cloneCooldown = cloneAttackCool;
        blackholeTimer = blackholeDuration;
        if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }
    
    
    

    // Update is called once per frame
    void Update()
    {
        blackholeTimer -= Time.deltaTime;
        cloneAttackTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity; // ensure the follow logic only execute once
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishedBlackholeAbility();
            }
        }
        
        

        if (InputManager.instance.padUpJustPressed)
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            // lerp 会越来越慢
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        DestroyHotkeys();
        cloneAttackReleased = true;
        canCreateHotkeys = false;
        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneCooldown;

            if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
            {
                SkillManager.instance.crystalSkill.CreateCrystal();
                SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                int randomIndex = Random.Range(0, targets.Count);
                SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex].transform, new Vector2(2, 0));
            }
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke(nameof(FinishedBlackholeAbility), 0.5f);
            }
        }
    }

    private void FinishedBlackholeAbility()
    {
        DestroyHotkeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        enemy?.FreezeTime(false);
    }


    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("not enough keycode list");
            return;
        }

        if (!canCreateHotkeys)
        {
            return;
        }
        
        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2),
            Quaternion.identity);
        createdHotkeys.Add(newHotkey);
        string chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        BlackholeHotkeyController hotkeyController = newHotkey.GetComponent<BlackholeHotkeyController>();
        hotkeyController.SetUpHotKey(chooseKey, collision.transform, this);
    }


    private void DestroyHotkeys()
    {
        if (createdHotkeys.Count <= 0)
        {
            return;
        }

        foreach (GameObject createdHotkey in createdHotkeys)
        {
            Destroy(createdHotkey);
        }
    }


    public void AddTarget(Transform target) => targets.Add(target);

}