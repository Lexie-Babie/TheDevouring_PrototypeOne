using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST } //defining the different states that the game can be in

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHealth;
    public BattleHUD enemyHealth;
    
    public BattleState state;

    public bool TakeDamage;

    public bool SetActive;


    //Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }


    //Instatiate is another term for spawn
    //PlayerGo = Player GameObject; EnemyGo = Enemy GameObject 
    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<Unit>();

        playerHealth.SetSlider(playerUnit);
        enemyHealth.SetSlider(enemyUnit);

        yield return new WaitForSeconds(1.0f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
 
    }

    IEnumerator PlayerAttack()
    {
        //Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        Debug.Log("TakeDamage"); 

        enemyHealth.SetHP(enemyUnit.currentHP);


        yield return new WaitForSeconds(0.5f);

        //Check if the enemy is dead
        if(isDead)
        {
            state = BattleState.WON;  //End the battle 
            enemyHealth.SetHP(enemyUnit.currentHP = 0);
            enemyUnit.GetComponent<SpriteRenderer>().enabled = false;
            EndBattle(); 
        } else
        {
            state = BattleState.ENEMYTURN;  //Enemy turn
            enemyHealth.SetHP(enemyUnit.currentHP);

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(EnemyTurn());
        }
        //Change state based on what happens 
    }

    IEnumerator EnemyTurn()
    {

        yield return new WaitForSeconds(0.5f); 
        
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHealth.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(0.5f);

        if(isDead)
        {
            state = BattleState.LOST;
            yield return new WaitForSeconds(2.0f);
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            print ("You won the battle");
        }
        else if(state == BattleState.LOST)
        {
            print("You were defeated");
            
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void PlayerTurn()
    {
        print("Choose an action");
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHealth.SetHP(playerUnit.currentHP);
        print("You feel renewed strength");

        yield return new WaitForSeconds(0.5f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerCook()
    {
        enemyUnit.TakeDamage(playerUnit.damage); 
        
        playerUnit.Cook(10);
        enemyHealth.SetHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(0.5f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)

            return;

        print("Press attack button");

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)

            return;

        print("Press heal button");

        StartCoroutine(PlayerHeal());
    }

    public void OnCookButton()
    {
        if (state != BattleState.PLAYERTURN)

            return;

        print("Press cook button");

        StartCoroutine(PlayerCook());
    }

}
