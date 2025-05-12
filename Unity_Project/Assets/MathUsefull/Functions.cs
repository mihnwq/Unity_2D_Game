using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

  public class Functions
  {


    public float getDerivativeWithTime(float value)
    {
        return value / Time.deltaTime;
    }
  }

