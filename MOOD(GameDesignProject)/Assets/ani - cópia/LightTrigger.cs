using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Verificar se o objeto que entrou no trigger é o personagem
        if (other.CompareTag("New tag"))
        {
            // Encontrar todos os objetos do tipo PointLightController na cena
            PointLightController[] allPointLights = FindObjectsOfType<PointLightController>();

            // Percorrer todas as luzes encontradas e definir a intensidade para 0
            foreach (PointLightController pointLightController in allPointLights)
            {
                pointLightController.SetIntensityToZero();
            }
        }
    }
}
