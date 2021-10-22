﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseGenerator
{
    float[] GenerateNoiseMap(LayerChromosome map);
}
