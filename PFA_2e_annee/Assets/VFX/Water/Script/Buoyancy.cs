using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{

  

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag= 1f;

    public float airDrag = 0f;
    public float airAngularDrag= 0.05f;

    public float floatingPower = 15f;

    public float waterHeight = 0f;

    public float test = 1f;
    public float vroom = 1f;

    Rigidbody m_Rigidbody;

    

    bool underwater;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        float waveHeight = 1.0f+WaveManager.instance.GetWaveHeight(transform.position.x);
        float difference = transform.position.y - waterHeight+waveHeight+test;

        if(difference<waveHeight+vroom)
        {
            m_Rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), transform.position, ForceMode.Force);
            
            if(!underwater)
            {
                underwater = true;
                SwitchState(true);
            }
        }
             
        else if(underwater)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if(isUnderwater)
        {
            m_Rigidbody.drag = underWaterDrag;
            m_Rigidbody.angularDrag= underWaterAngularDrag;
        }
        else
        {
            m_Rigidbody.drag = airDrag;
            m_Rigidbody.angularDrag = airAngularDrag;
        }    
    }   
}
