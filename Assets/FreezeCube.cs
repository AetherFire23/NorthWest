using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCube : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] float _speed;
    void Update()
    {
        var pos = this.transform.position;
        this.transform.position = new Vector3(pos.x + Time.deltaTime * _speed, pos.y, pos.z);
    }
}
