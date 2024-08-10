using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{

    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> targets = new();

    // Update is called once per frame
    void Update()
    {
        if (canGrow)
        {
            // lerp 会越来越慢
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTime(true);

            CreateHotKey(collision);


            // targets.Add(collision.transform);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("not enough keycode list");
            return;
        }
        
        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2),
            Quaternion.identity);
        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        BlackholeHotkeyController hotkeyController = newHotkey.GetComponent<BlackholeHotkeyController>();
        hotkeyController.SetUpHotKey(chooseKey, collision.transform, this);
    }


    public void AddTarget(Transform target) => targets.Add(target);

}