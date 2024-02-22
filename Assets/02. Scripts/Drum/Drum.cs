using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;


public class Drum : MonoBehaviour, iHitalbe
{
    public int Health = 3;
    public GameObject drumEffect;
    private float explosionForce = 30f;
    private int explosionRange = 5;
    private bool hasExploded = false;



    public void Hit(int damage)
    {
        if (hasExploded) return;
        Health -= damage;

        drumAction();

    }

    private void drumAction()
    {
        if (Health <= 0 && !hasExploded)
        {
            hasExploded = true;

            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * explosionForce, ForceMode.Impulse);
            // rb.AddTorque(new Vector3(1, 10, 1) * explosionForce);
            GameObject drum = Instantiate(drumEffect);
            drum.transform.position = this.transform.position;
            ItemObjectFactory.Instance.MakePercent(transform.position);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, LayerMask.GetMask("Monster") | LayerMask.GetMask("Player"));
            foreach (Collider collider in colliders)
            {
                iHitalbe hitalbe = collider.GetComponent<iHitalbe>();
                int damage = 70;
                if (hitalbe != null)
                {
                    hitalbe.Hit(damage);
                }
            }
            Collider[] drumcollider = Physics.OverlapSphere(transform.position, explosionRange, LayerMask.GetMask("drum"));
            foreach (Collider col in drumcollider)
            {
                iHitalbe iHitalbe = col.GetComponent<iHitalbe>();
                int Damage = 10;
                if (iHitalbe != null)
                {
                    iHitalbe.Hit(Damage);
                }
            }

            StartCoroutine(DestroyDrum_Coroutine());

        }
    }


    private IEnumerator DestroyDrum_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);

    }
}
