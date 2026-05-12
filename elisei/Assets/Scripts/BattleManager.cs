using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public PlayerController player;
    public BossController boss;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI bossHPText;
    public int x=0;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            PlayerPrefs.SetInt("idScene",SceneManager.GetActiveScene().buildIndex);
    }
    public void StartRound()
    {
            StartCoroutine(PlayRound());
    }

    public void lvl()
    {
        Debug.Log(x);
        SceneManager.LoadScene(PlayerPrefs.GetInt("idScene"));
    }

    IEnumerator PlayRound()
    {
        int turns = player.mySequence.Count;
        while (player.hp > 0 && boss.hp > 0)
        {
            for (int i = 0; i < turns; i++)
            {
                GameAction pAct = player.mySequence[i];
                GameAction bAct = boss.bossPattern[i % boss.bossPattern.Count];


                if (pAct == GameAction.Attack) StartCoroutine(player.AnimateAttack(false));
                if (pAct == GameAction.Jump) StartCoroutine(player.AnimateJump());

                if (bAct == GameAction.Attack) StartCoroutine(boss.AnimateAttack(true));
                if (bAct == GameAction.Jump) StartCoroutine(boss.AnimateJump());

                yield return new WaitForSeconds(0.7f);

                CalculateDamage(pAct, bAct);

                playerHPText.text = "Игрок: " + player.hp;
                bossHPText.text = "Босс: " + boss.hp;

                if (player.hp <= 0)
                {

                    Debug.Log(x);
                    SceneManager.LoadScene("Lose"); 

                }
                if (boss.hp <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }

                yield return new WaitForSeconds(0.8f);

            }
          
        }
        player.Clear();
    }

    void CalculateDamage(GameAction p, GameAction b)
    {
        int playerBaseDamage = 40;
        int bossBaseDamage = 40;
        int healAmount = 15;

        // --- ЛОГИКА ДЛЯ ИГРОКА (ПОЛУЧЕНИЕ УРОНА И ХИЛ) ---
        if (p == GameAction.Jump)
        {
            player.hp += healAmount; // Прыжок хилит
        }

        if (b == GameAction.Attack)
        {
            if (p == GameAction.Block)
            {
                // Под щитом урон не идет
            }
            else if (p == GameAction.Jump)
            {
                // Если босс бьет прыгающего игрока — урон х2
                player.hp -= bossBaseDamage * 2;
            }
            else
            {
                // Обычный урон
                player.hp -= bossBaseDamage;
            }
        }

        // --- ЛОГИКА ДЛЯ БОССА (ПОЛУЧЕНИЕ УРОНА И ХИЛ) ---
        if (b == GameAction.Jump)
        {
            boss.hp += healAmount; // Прыжок хилит
        }

        if (p == GameAction.Attack)
        {
            if (b == GameAction.Block)
            {
                // Под щитом урон не идет
            }
            else if (b == GameAction.Jump)
            {
                // Если игрок бьет прыгающего босса — урон х2 от обычного (40 * 2 = 80)
                // Но ты просил "боссу в два раза меньше", если это про обычную атаку, 
                // то х2 от "половины" — это будет просто полный урон (40).
                // Давай сделаем х2 от его текущего входящего урона:
                boss.hp -= (playerBaseDamage / 2) * 2;
            }
            else
            {
                // Обычная атака по боссу — всегда в 2 раза меньше базовой
                boss.hp -= playerBaseDamage / 2;
            }
        }
    }
    
}