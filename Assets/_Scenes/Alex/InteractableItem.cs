using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private string itemID;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemManager.Instance.SetItemState(itemID, true);
            gameObject.SetActive(false);
            Debug.Log($"Item {itemID} picked up");
        }
    }
}

