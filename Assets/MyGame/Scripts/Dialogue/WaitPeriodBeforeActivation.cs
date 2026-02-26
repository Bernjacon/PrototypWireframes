using UnityEngine;
using System.Collections;
public class WaitPeriodBeforeActivation : MonoBehaviour
{
    [SerializeField] float waitingPeiod;
    [SerializeField] GameObject[] toActivate;
    void Start()
    {
        StartCoroutine(WaitingPeriod());
    }
    public IEnumerator WaitingPeriod()
    {
        yield return new WaitForSeconds(waitingPeiod);
        foreach (GameObject go in toActivate)
        {
            if (go != null) go.SetActive(true);
        }
    }
}
