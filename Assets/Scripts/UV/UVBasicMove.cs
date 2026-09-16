using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVBasicMove : MonoBehaviour {

    public Renderer m_Target;
    public string[] m_TexName = new string[1] {"_MainTex"};
    protected int m_Modifier = 1;
    protected float m_Amplifier = 1f;
    protected Material[] m_Materials;


    public virtual void Awake()
    {
        if (!m_Target)
            m_Target = GetComponent<Renderer>();

        m_Materials = new Material[m_Target.materials.Length];
        if (m_Target)
        {
            for (int i = 0; i < m_Target.materials.Length; i++)
            {
                m_Materials[i] = m_Target.materials[i];
            }
        }
    }

    void OnDestroy() {
        for (int i = 0; i < m_Materials.Length; i++)
        {
            if (m_Materials[i] != null)
                Destroy(m_Materials[i]);
        }
    }

    public virtual void AccelMovement(float amplifier) {
        m_Amplifier = amplifier;
    }

    public void ResetAmplifier() {
        m_Amplifier = 1f;
    }
}
