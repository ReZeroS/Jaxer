// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
//
// public class BattleSystem : MonoBehaviour
// {
//     public GameObject playerCharacter;
//     public GameObject enemyCharacter;
//     public UIManager uiManager;
//
//     void Start()
//     {
//         StartCoroutine(BattleSequence());
//     }
//
//     IEnumerator BattleSequence()
//     {
//         while (true) // 战斗循环
//         {
//             yield return StartCoroutine(PlayerTurn());
//             if (CheckBattleEnd()) yield break;
//
//             yield return StartCoroutine(EnemyTurn());
//             if (CheckBattleEnd()) yield break;
//         }
//     }
//
//     IEnumerator PlayerTurn()
//     {
//         Debug.Log("玩家回合开始");
//
//         // 等待玩家选择动作
//         yield return StartCoroutine(uiManager.WaitForPlayerInput());
//
//         string playerAction = uiManager.GetPlayerAction();
//
//         switch (playerAction)
//         {
//             case "Attack":
//                 yield return StartCoroutine(PerformAttack(playerCharacter, enemyCharacter));
//                 break;
//             case "UseItem":
//                 yield return StartCoroutine(UseItem());
//                 break;
//             case "CastSpell":
//                 yield return StartCoroutine(CastSpell());
//                 break;
//         }
//
//         Debug.Log("玩家回合结束");
//     }
//
//     IEnumerator EnemyTurn()
//     {
//         Debug.Log("敌人回合开始");
//
//         // 模拟敌人思考时间
//         yield return new WaitForSeconds(1.5f);
//
//         yield return StartCoroutine(PerformAttack(enemyCharacter, playerCharacter));
//
//         Debug.Log("敌人回合结束");
//     }
//
//     IEnumerator PerformAttack(GameObject attacker, GameObject target)
//     {
//         // 播放攻击动画
//         Animator attackerAnimator = attacker.GetComponent<Animator>();
//         attackerAnimator.SetTrigger("Attack");
//
//         // 等待动画播放到一半（假设是 0.5 秒）
//         yield return new WaitForSeconds(0.5f);
//
//         // 造成伤害
//         target.GetComponent<CharacterStats>().TakeDamage(10);
//
//         // 等待动画完成
//         yield return new WaitForSeconds(0.5f);
//
//         // 检查目标是否被击倒
//         if (target.GetComponent<CharacterStats>().currentHP <= 0)
//         {
//             yield return StartCoroutine(PerformDeathSequence(target));
//         }
//     }
//
//     IEnumerator UseItem()
//     {
//         Debug.Log("使用物品");
//         // 播放使用物品的动画
//         yield return new WaitForSeconds(1f);
//         playerCharacter.GetComponent<CharacterStats>().Heal(20);
//         Debug.Log("恢复了 20 点生命值");
//     }
//
//     IEnumerator CastSpell()
//     {
//         Debug.Log("开始施法");
//         // 播放施法动画
//         yield return new WaitForSeconds(1.5f);
//         
//         // 模拟一个需要蓄力的法术
//         float chargingTime = 0f;
//         while (chargingTime < 3f)
//         {
//             chargingTime += Time.deltaTime;
//             // 更新UI显示蓄力进度
//             uiManager.UpdateSpellChargingBar(chargingTime / 3f);
//             yield return null; // 每帧更新一次
//         }
//
//         Debug.Log("法术释放完成");
//         enemyCharacter.GetComponent<CharacterStats>().TakeDamage(30);
//     }
//
//     IEnumerator PerformDeathSequence(GameObject character)
//     {
//         Animator characterAnimator = character.GetComponent<Animator>();
//         characterAnimator.SetTrigger("Die");
//
//         // 等待死亡动画播放完毕
//         yield return new WaitForSeconds(2f);
//
//         character.SetActive(false);
//     }
//
//     bool CheckBattleEnd()
//     {
//         if (playerCharacter.GetComponent<CharacterStats>().currentHP <= 0)
//         {
//             StartCoroutine(EndBattle("Defeat"));
//             return true;
//         }
//         if (enemyCharacter.GetComponent<CharacterStats>().currentHP <= 0)
//         {
//             StartCoroutine(EndBattle("Victory"));
//             return true;
//         }
//         return false;
//     }
//
//     IEnumerator EndBattle(string result)
//     {
//         yield return new WaitForSeconds(2f); // 给玩家时间看到最后的战斗结果
//
//         if (result == "Victory")
//         {
//             Debug.Log("战斗胜利！");
//             // 显示胜利UI，播放胜利音效等
//         }
//         else
//         {
//             Debug.Log("战斗失败！");
//             // 显示失败UI，播放失败音效等
//         }
//
//         // 等待玩家确认
//         yield return StartCoroutine(uiManager.WaitForPlayerConfirmation());
//
//         // 返回到世界地图或重新开始
//         // SceneManager.LoadScene("WorldMap");
//     }
// }
//
// // 这些类在实际项目中应该分别放在不同的文件中
// public class CharacterStats : MonoBehaviour
// {
//     public int currentHP;
//     public void TakeDamage(int amount) { /* 实现受伤逻辑 */ }
//     public void Heal(int amount) { /* 实现治疗逻辑 */ }
// }
//
// public class UIManager : MonoBehaviour
// {
//     public IEnumerator WaitForPlayerInput() { yield return null; /* 实现等待玩家输入的逻辑 */ }
//     public string GetPlayerAction() { return "Attack"; /* 返回玩家选择的动作 */ }
//     public void UpdateSpellChargingBar(float progress) { /* 更新法术充能进度条 */ }
//     public IEnumerator WaitForPlayerConfirmation() { yield return null; /* 等待玩家确认 */ }
// }
