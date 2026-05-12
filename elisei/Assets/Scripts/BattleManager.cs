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
    public Animator healAnim;
    public Animator AtakAnim;

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


                if (pAct == GameAction.Attack)
                {
                    if (AtakAnim != null)
                    {
                        // Прячем игрока
                        player.GetComponent<SpriteRenderer>().enabled = false;

                        // Включаем и запускаем анимацию
                        AtakAnim.gameObject.SetActive(true);
                        Animator realAnimator = AtakAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("PlayAttack");

                        // ЗАПУСК ОЖИДАНИЯ:
                        // Вместо 1.0f поставь длительность своей анимации в секундах
                        StartCoroutine(FinishAnimation(AtakAnim.gameObject, 1.3f));
                    }
                }

                if (pAct == GameAction.Jump)
                {
                    if (healAnim != null)
                    {
                        // Принудительно берем компонент заново, чтобы исключить "битую" ссылку
                        Animator realAnimator = healAnim.GetComponent<Animator>();

                        if (realAnimator != null && realAnimator.runtimeAnimatorController != null)
                        {
                            realAnimator.SetTrigger("PlayHeal");
                            Debug.Log("Триггер отправлен успешно!");
                        }
                        else
                        {
                            Debug.LogError("Контроллер внезапно исчез с объекта!");
                        }
                    }

                }
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
    IEnumerator FinishAnimation(GameObject animObj, float delay)
    {
        // Ждем столько секунд, сколько длится твоя анимация
        yield return new WaitForSeconds(delay);

        // 1. Выключаем объект с анимацией (снимаем галочку)
        animObj.SetActive(false);

        // 2. Возвращаем видимость основному игроку (ставим галочку обратно)
        player.GetComponent<SpriteRenderer>().enabled = true;
    }

}