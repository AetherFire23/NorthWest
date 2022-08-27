using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class CameraController : MonoBehaviour
{
    private PlayerScript _playerFacade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        this.transform.position = _playerFacade.transform.position.WithOffset(0,0,-10);

    }

    [Inject]
    public void Construct(PlayerScript playerFacade)
    {
        _playerFacade = playerFacade;
    }
}
