using UnityEngine;
using System.Collections;

public class UVCircularMovement : UVBasicMove {

    [SerializeField] float m_Speed = 0f;
    [SerializeField] float m_Radius = 0f;

    float m_Value = 0f;
    float m_RadiusAmplifier = 1f;


    public override void Awake() {
        base.Awake();

        for (int i = 0; i < m_Materials.Length; i++)
        {
            if (!m_Materials[i])
                this.enabled = false;
        }

        if (m_Speed == 0)
            this.enabled = false;

        if (m_Speed < 0f)
            m_Modifier = -1;
    }

    void Update() {
        m_Value += Time.deltaTime * m_Amplifier * m_Speed * m_Modifier;
        if (m_Value > 2f)
            m_Value -= 2f;

        for (int i = 0; i < m_Materials.Length; i++)
        {
            for (int j=0; j<m_TexName.Length; j++)
                m_Materials[i].SetTextureOffset(m_TexName[j], new Vector2(Mathf.Sin(m_Value * Mathf.PI) * m_Modifier * m_Radius * m_RadiusAmplifier, Mathf.Cos(m_Value * Mathf.PI) * m_Modifier * m_Radius * m_RadiusAmplifier));
        }
    }

    public override void AccelMovement(float amplifier) {
        m_Amplifier = amplifier;
        m_RadiusAmplifier = Mathf.Max(amplifier/2f, 1f);
    }
}
