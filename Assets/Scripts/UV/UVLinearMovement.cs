using UnityEngine;
using System.Collections;

public class UVLinearMovement : UVBasicMove {

    [SerializeField] Vector2 m_Speed = new Vector2(0f, 0f);
    Vector2 m_Value = Vector2.zero;
    int m_ModifierY = 1;


    public override void Awake() {
        base.Awake();

        for (int i = 0; i < m_Materials.Length; i++)
        {
            if (!m_Materials[i])
                this.enabled = false;
        }


        if (m_Speed.x < 0f)
            m_Modifier = -1;
        if (m_Speed.y < 0f)
            m_ModifierY = -1;
    }

    void Update() {
        m_Value += new Vector2(m_Speed.x * m_Modifier, m_Speed.y * m_ModifierY) * Time.deltaTime * m_Amplifier;
        if (m_Value.x > 1f)
            m_Value.x -= 1f;
        if (m_Value.y > 1f)
            m_Value.y -= 1f;

        for (int i = 0; i < m_Materials.Length; i++)
        {
            for (int j=0; j<m_TexName.Length; j++)
                m_Materials[i].SetTextureOffset(m_TexName[j], new Vector2(m_Value.x * m_Modifier, m_Value.y * m_ModifierY));
        }
    }
}
