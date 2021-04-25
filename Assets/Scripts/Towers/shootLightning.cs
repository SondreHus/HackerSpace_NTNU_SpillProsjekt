﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootLightning : MonoBehaviour
{
    public float LightningRadius;
    public float damage;
    //public int maxTargets;
    private List<GameObject> ZappTargets = new List<GameObject>();
    private List<Ray> rays = new List<Ray>();
    public GameObject drainRay;
    private List<GameObject> lightningboltPool = new List<GameObject>();

    public LayerMask ShockLayers;
    //This script should be called by lightning tower using the shoot() function from the animation controller.


    public void shoot()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, LightningRadius, ShockLayers);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<HealthLogic>())
            {
               // addZap(hitCollider.gameObject);
                checkZap(hitCollider.gameObject);
            }
        }
        foreach (var zap in ZappTargets)
        {
            zap.GetComponent<HealthLogic>().DealDamage(damage);
        }
        ZappTargets.Clear();
    }




    private void addZap(GameObject target)
    {
        if (target.GetComponent<HealthLogic>() && !ZappTargets.Contains(target))
        {
            GameObject previousTarget = null;
            if (ZappTargets.Count > 0)
            {
                previousTarget = ZappTargets[ZappTargets.Count - 1];
            }
            else
            {
                previousTarget = this.gameObject;
            }
            lightningVFX(previousTarget.transform, target.transform);

            ZappTargets.Add(target);
            checkZap(target);


        }
    }

    private void checkZap(GameObject target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, LightningRadius, ShockLayers);
        foreach (var hitCollider in hitColliders)
        {
            if (!ZappTargets.Contains(hitCollider.gameObject) && hitCollider.GetComponent<HealthLogic>())
            {
                addZap(hitCollider.gameObject);
              //  checkZap(hitCollider.gameObject);
            }
        }
    }
    private void lightningVFX(Transform previousTarget, Transform newTarget)
    {
        GameObject ray = Instantiate(drainRay, previousTarget.position, previousTarget.rotation);
        ray.transform.SetParent(previousTarget.transform);

        //component[1] so we get the first child and not the parent itself.
        Transform target = ray.GetComponentsInChildren<Transform>()[1];
        target.SetParent(newTarget);
        target.localPosition = Vector3.zero;
        //Destroying effect after 0.3 seconds:
        Transform r = ray.transform;
        Destroy(r.gameObject, 0.3f);

        //rays.Add(new Ray(ray.transform, target));
    }
}
