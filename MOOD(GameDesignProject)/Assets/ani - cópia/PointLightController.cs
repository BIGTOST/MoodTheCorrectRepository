using UnityEngine;

public class PointLightController : MonoBehaviour
{
    // Referência à luz anexada a este GameObject
    private Light pointLight;

    void Start()
    {
        // Obter a referência à luz
        pointLight = GetComponent<Light>();

        // Verificar se a luz é do tipo Point
        if (pointLight == null || pointLight.type != LightType.Point)
        {
            Debug.LogError("Este script deve ser anexado a uma luz do tipo Point.");
            enabled = false; // Desabilitar o script se não for uma luz do tipo Point
        }
    }

    // Método para definir a intensidade da luz para 0
    public void SetIntensityToZero()
    {
        if (pointLight != null && pointLight.type == LightType.Point)
        {
            pointLight.intensity = 0f;
        }
    }
}
