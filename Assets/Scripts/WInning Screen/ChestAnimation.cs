using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    [SerializeField] private Transform topChest; 
    [SerializeField] private Transform bottomChest; 
    [SerializeField] private ParticleSystem coinParticles; 
    [SerializeField] private float openSpeed = 2f; 
    [SerializeField] private float maxOpenAngle = 70f; 
    [SerializeField] private float particleDelay = 0.5f; 

    private bool isOpening = false;
    private bool isOpened = false;
    private float currentAngle = 0f;

    void Update()
    {
        if (isOpening && !isOpened)
        {
            currentAngle += Time.deltaTime * openSpeed * maxOpenAngle;
            topChest.localRotation = Quaternion.Euler(-currentAngle, 0f, 0f);

            if (currentAngle >= maxOpenAngle)
            {
                currentAngle = maxOpenAngle;
                isOpening = false;
                isOpened = true;

                Invoke(nameof(PlayParticles), particleDelay);
            }
        }
    }

    public void OpenChest()
    {
        if (!isOpening && !isOpened)
        {
            isOpening = true;
        }
    }

    private void PlayParticles()
    {
        if (coinParticles != null)
        {
            coinParticles.Play();
        }
    }
}
