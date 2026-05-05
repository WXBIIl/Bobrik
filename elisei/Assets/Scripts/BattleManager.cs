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

        for (int i = 0; i < turns; i++)
        {
            GameAction pAct = player.mySequence[i];
            GameAction bAct = boss.bossPattern[i % boss.bossPattern.Count];

            player.myImage.sprite = (pAct == GameAction.Block) ? player.blockSprite : player.normalSprite;
            boss.myImage.sprite = (bAct == GameAction.Block) ? boss.blockSprite : boss.normalSprite;

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

            player.myImage.sprite = player.normalSprite;
            boss.myImage.sprite = boss.normalSprite;
        }
        player.Clear();
    }

    void CalculateDamage(GameAction p, GameAction b)
    {
        if (p == GameAction.Attack || p == GameAction.Jump)
        {
            if (b != GameAction.Block) { boss.hp -= 40; StartCoroutine(boss.FlashRed()); }
            else boss.hp -= 15;
        }
        if (b == GameAction.Attack || b == GameAction.Jump)
        {
            if (p != GameAction.Block) { player.hp -= 10; StartCoroutine(player.FlashRed()); }
            else player.hp -= 15;
        }
        if(b==GameAction.Jump)
        {
            if (p != GameAction.Attack)
            {
                boss.hp += 10;
            }
            else boss.hp -= 15;
        }
    }
    
}