using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private WorldLocation _location;
    public WorldLocation location
    {
        get => _location;
        set
        {
            transform.position = value.location;
            _location = location;
        }
    }

    private void OnEnable()
    {
        _location = new WorldLocation(transform.position);
    }

    public virtual void onSpawn()
    {

    }
}
