using AudioName;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetCoin();
            Destroy(gameObject);
        }
    }

    public void GetCoin()
    {
        ResultManager.Instance.AddCoin();
        AudioManager.Instance.PlaySE(SEName.COIN_SE_NAME);
    }
}
