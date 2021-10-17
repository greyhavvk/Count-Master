using UnityEngine;

public class FinishStickmanController : MonoBehaviour
{
    private void Update()
    {
        if (this.transform.parent.childCount == 1)
        {
            EventManager.winGame.Invoke();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishFloorUpTrigger"))
        {
            transform.parent = null;
            transform.GetComponent<FinishStickmanController>().enabled = false;
        }
        else if (other.CompareTag("Stop"))
        {
            EventManager.winGame.Invoke();
        }
    }
}
