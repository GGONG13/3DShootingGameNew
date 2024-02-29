using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UIElements;


public class Drum : MonoBehaviour, iHitalbe
{
    public int Health = 3;
    public GameObject drumEffect;
    private float explosionForce = 30f;
    private int explosionRange = 5;
    private bool hasExploded = false;

    public List<Texture2D> DrumMaterial;
    void Start()
    {
 
        MeshRenderer mr = GetComponent<MeshRenderer>();
        int random = UnityEngine.Random.Range(0, DrumMaterial.Count);
        foreach (var drum in DrumMaterial)
        {
            mr.material.SetTexture("_MainTex", DrumMaterial[random]);
        }
    }   


    public void Hit(DamageInfo damageInfo)
    {
        if (hasExploded) return;
        Health -= damageInfo.Amount;

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
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, damage);
                if (hitalbe != null)
                {
                    hitalbe.Hit(damageInfo);
                }
            }
            Collider[] drumcollider = Physics.OverlapSphere(transform.position, explosionRange, LayerMask.GetMask("drum"));
            foreach (Collider col in drumcollider)
            {
                iHitalbe iHitalbe = col.GetComponent<iHitalbe>();
                int Damage = 10;
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                if (iHitalbe != null)
                {
                    iHitalbe.Hit(damageInfo);
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
