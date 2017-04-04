using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfinding
{
    List<int> GetPath(int start, int finish, GameObject[] tiles, int map_size, List<int> obstacles);
}

