using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PogoPandemonium
{
    public interface IBuff
    {
        void Apply(Player player);
        void Remove(Player player);
    }
}