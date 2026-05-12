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
                        player.GetComponent<SpriteRenderer>().enabled = false;

                        AtakAnim.gameObject.SetActive(true);
                        Animator realAnimator = AtakAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("PlayAttack");

                        StartCoroutine(FinishAnimation(AtakAnim.gameObject, 1.3f));
                    }
                }

                if (pAct == GameAction.Jump)
                {
                    if (healAnim != null)
                    {
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

        if (p == GameAction.Jump)
        {
            player.hp += healAmount;
        }

        if (b == GameAction.Attack)
        {
            if (p == GameAction.Block)
            {

            }
            else if (p == GameAction.Jump)
            {
                player.hp -= bossBaseDamage * 2;
            }
            else
            {
                player.hp -= bossBaseDamage;
            }
        }

        if (b == GameAction.Jump)
        {
            boss.hp += healAmount; 
        }

        if (p == GameAction.Attack)
        {
            if (b == GameAction.Block)
            {

            }
            else if (b == GameAction.Jump)
            {
                boss.hp -= (playerBaseDamage / 2) * 2;
            }
            else
            {
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