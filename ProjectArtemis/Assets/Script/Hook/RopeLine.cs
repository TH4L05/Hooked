using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RopeLine
{
    public Material lineMaterial;
    public float lineCurveWidthMultiplier = 1f;
    public float lineStartWidth = 1f;
    public float lineEndWidth = 1f;
    public Gradient lineGradient;
    public AnimationCurve lineCurve;
    public LineTextureMode lineTextureMode;
    public float materialTiling = 0.1f;
    public float materialColorStrength = 0f;
    public float materialDistortionAmount = 0.05f;
}
