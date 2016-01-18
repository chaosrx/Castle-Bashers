using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public float RegenAmount;
    public float currentHealth=0;
    public float maxhp;
    private bool isInvincible;
    private Player player;
    private MoveController moveController;
    
    public Vector3 damageTextOffset;
    public AudioClip hitSound;


    // Use this for initialization
    void Start()
    {
        hitSound = Resources.Load("hurt2") as AudioClip;
        player = GetComponent<Player>();
        moveController = GetComponent<MoveController>();
        currentHealth = maxhp; 
        damageTextOffset = new Vector3(0, 2, 0);
    }

    
    public void Update_Maxhp()
    {
        Debug.Log("You are using Update_Maxhp() which is an outdated function");
        /*
        if (player)
        {
            maxhp = startingHealth + player.GetStrength() * 10 + player.GetStamina() * 30 + player.CCI.Class_info[player.GetClassID()].accessory[player.GetAccessoriesLV()].maxhp;
            Debug.Log("UPDATE_MAXHP SET HP TO " + maxhp);
        }
        else
            maxhp = startingHealth;
        */
    }

    public void Updata_Maxhp_withFullRegen()
    {
        //Update_Maxhp();
        Full_Regen();
    }

    public void Full_Regen()
    {
        currentHealth = maxhp;
    }

    public void Regen()
    {
        currentHealth += (RegenAmount + player.GetStamina()*3);
        if (currentHealth > maxhp)
        {
            currentHealth = maxhp;
        }
    }


    public virtual void takeDamage(float dmg, float knockback = 4, float flinch = 5)
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position, 1);
        Debug.Log("takingDamage");

        //Rounding damage up to the nearest int for a clean display. It may make some situations easier in the early game
        //but considering the nature of a hack and slash, that shouldn't be an issue. Will keep an eye on the effects.
        dmg = Mathf.CeilToInt(dmg);
        currentHealth -= dmg;
        createFloatingText(dmg);
        Destroy(Instantiate(Resources.Load("Particles/ProjectileExplosion"), gameObject.transform.position, Quaternion.identity), 2f);

        //    player.ModifyKBCount(knockback);
        //    if (knockback > 0)
        //        player.ResetKB();

        //    player.ModifyFlinchCount(flinch);
        //    if (flinch > 0)
        //        player.ResetFlinch();

        //    if (moveController)
        //    {
        //        if (player.GetKnockable())
        //        {
        //            moveController.SetKnockback(true);
        //            player.ModifyKBCount(0, 0);
        //        }
        //        else if (player.GetFlinchable())
        //        {
        //            moveController.SetFlinch(true);
        //            player.ModifyFlinchCount(0, 0);
        //        }
        //    }
        if (currentHealth <= 0)
        {
            Death();
        }
        
    
    }

    public void PlayerDown()
    {
        GetComponent<Player>().setDown(true);
        //use other object to check if all players down, if so then Death() + lose level
        GameManager.Notifications.PostNotification(new Message(this.gameObject, MessageTypes.PLAYER_DEATH));

        //Death();
    }

    public virtual void Death()
    {
        //death animation
        //end level

        // Down the player if it was a player that died
        if(GetComponent<Player>())
        {
            PlayerDown();
        }

        // Reward all players with experience if an enemy died
        if (GetComponent<Enemy>())
        {
            Enemy enemy = GetComponent<Enemy>();
            if (currentHealth <= 0)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject character in players)
                {
                    character.GetComponent<Experience>().AddExperience(enemy.experienceAmount);
                }
            }
        }

        // Drop loot
        if (GetComponent<DropLoot>())
        {
            GetComponent<DropLoot>().DropItem();
        }

        Destroy(gameObject);
    }

    void createFloatingText(float f)
    {

        GameObject floatText = Instantiate(Resources.Load("FloatingText")) as GameObject;
        floatText.GetComponent<TextMesh>().text = "" + f;
        floatText.transform.position = gameObject.transform.position + damageTextOffset;
    }
/*
    public float GetStartingHealth()
    {
        return startingHealth;
    }
    */
    public virtual float GetCurrentHealth()
    {
        return gameObject.GetComponent<Health>().currentHealth;
    }

    public void AddHealth(float healthAmount)
    {
        currentHealth = currentHealth + healthAmount;
        if(currentHealth > maxhp)
        {
            currentHealth = maxhp;
        }
    }

    public float GetMaxHP()
    {
        return maxhp;
    }

    public void SetMaxHP(float f)
    {
        maxhp = f;
    }

    public void SetCurrentHP(float f)
    {
        currentHealth = f;
    }
}
