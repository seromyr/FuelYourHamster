using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterBallMechanic : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private int gameplayLevel;

    private bool isActive;
    public bool IsActive { get { return isActive; } }

    private void Awake()
    {
        TryGetComponent(out meshRenderer);
        Destroy(GetComponent<SphereCollider>());
    }

    private void OnEnable()
    {
        ResetSphere();
    }

    private void SetSphereLevel(int level)
    {
        switch (level)
        {
            case 0:
                meshRenderer.material = Resources.Load<Material>("Materials/HamsterBall 0");
                transform.localScale = Vector3.zero;
                isActive = false;
                break;

            case 1:
                meshRenderer.material = Resources.Load<Material>("Materials/HamsterBall 1");
                transform.localScale = Vector3.one * 4;
                isActive = true;
                break;

            case 2:
                meshRenderer.material = Resources.Load<Material>("Materials/HamsterBall 2");
                transform.localScale = Vector3.one * 5;
                isActive = true;
                break;
        }
    }

    public void ResetSphere()
    {
        SetSphereLevel(Player.main.HamsterBallLevel);
        gameplayLevel = Player.main.HamsterBallLevel;
        Debug.Log("Hamster ball reset");
    }

    public void DegradeSphere()
    {
        gameplayLevel--;

        if (gameplayLevel <= 0)
        {
            gameplayLevel = 0;
        }

        SetSphereLevel(gameplayLevel);
    }
}
