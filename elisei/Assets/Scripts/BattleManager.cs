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

    [Header("Анимации игрока")]
    public Animator healAnim;
    public Animator AtakAnim;
    public Animator blockAnim;

    [Header("Анимации Boss'а")]
    public Animator bBOSSAnim;
    public Animator bossBlockAnim;
    public Animator dragonAttackAnim;
    public Animator dragonHealAnim;

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

                if (pAct == GameAction.Heal)
                {
                    if (healAnim != null)
                    {

                        healAnim.gameObject.SetActive(true);
                        Animator realAnimator = healAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("PlayAttack");

                        StartCoroutine(FinishAnimation(healAnim.gameObject, 1.3f));
                    }
                }

               


                if (pAct == GameAction.Block)
                {
                    if (AtakAnim != null)
                    {
                        player.GetComponent<SpriteRenderer>().enabled = false;

                        blockAnim.gameObject.SetActive(true);
                        Animator realAnimator = blockAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("PlayBlock");

                        StartCoroutine(FinishAnimation(blockAnim.gameObject, 1f));
                    }
                }

                if (bAct == GameAction.Attack )
                {
                    if (dragonAttackAnim != null)
                    {

                        boss.GetComponent<SpriteRenderer>().enabled = false;


                        dragonAttackAnim.gameObject.SetActive(true);
                        Animator realAnimator = dragonAttackAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("BOSSAttack");

                        StartCoroutine(FinishAnimation(dragonAttackAnim.gameObject, 1.3f));
                    }
                }

                if (bAct == GameAction.Block && pAct == GameAction.Attack)
                {
                    if (bBOSSAnim != null)
                    {

                        bBOSSAnim.gameObject.SetActive(true);
                        Animator realAnimator = bBOSSAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("PlayBOSSAttack");

                        StartCoroutine(FinishAnimation(bBOSSAnim.gameObject, 1.3f));
                    }
                }
                else if(bAct == GameAction.Block && pAct != GameAction.Attack)
                {
                    if (bossBlockAnim != null)
                    {

                        bossBlockAnim.gameObject.SetActive(true);
                        Animator realAnimator = bossBlockAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("PlayBOSSAtt");

                        StartCoroutine(FinishAnimation(bossBlockAnim.gameObject, 1f));
                    }
                }


                if (bAct == GameAction.Heal)
                {
                    if (dragonHealAnim != null)
                    {

                        dragonHealAnim.gameObject.SetActive(true);
                        Animator realAnimator = dragonHealAnim.GetComponent<Animator>();
                        realAnimator.SetTrigger("BOSSHeal");

                        StartCoroutine(FinishAnimation(dragonHealAnim.gameObject, 1.4f));
                    }
                }

                yield return new WaitForSeconds(1.5f);

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

        if (p == GameAction.Heal)
        {
            player.hp += healAmount;
        }

        if (b == GameAction.Attack)
        {
            if (p == GameAction.Block)
            {

            }
            else if (p == GameAction.Heal)
            {
                player.hp -= bossBaseDamage * 2;
            }
            else
            {
                player.hp -= bossBaseDamage;
            }
        }

        if (b == GameAction.Heal)
        {
            boss.hp += healAmount; 
        }

        if (p == GameAction.Attack)
        {
            if (b == GameAction.Block)
            {

            }
            else if (b == GameAction.Heal)
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
        yield return new WaitForSeconds(0.3f);
        boss.GetComponent<SpriteRenderer>().enabled = true;
    }

}