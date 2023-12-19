using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingGeometry : MonoBehaviour
{
    struct MainSection
    {
        float Chord;
        float Sweep;
        float Dihedral;
        float Twist;

        public MainSection(float chord, float sweep, float dihedral, float twist) {
            Chord = chord;
            Sweep = sweep;
            Dihedral = dihedral;
            Twist = twist;
        }
    }




    [SerializeField] public float rootChord;
    [SerializeField] public float midChord;
    [SerializeField] public float tipChord;
    [Space]
    [SerializeField] public float innerHalfSpan;
    [SerializeField] public float outerHalfSpan;
    [Space]
    [SerializeField] public float innerSweep;
    [SerializeField] public float outerSweep;


    public float innerHalfArea { get; private set; }
    public float outerHalfArea { get; private set; }

    public float totalArea { get; private set; }
    public float totalSpan { get; private set; }

    public float y_inner { get; private set; }
    public float y_outer { get; private set; }
    public float x_inner { get; private set; }
    public float x_outer { get; private set; }
    public float c_inner { get; private set; }
    public float c_outer { get; private set; }


    public float c_bar { get; private set; }
    public float x_bar { get; private set; }
    public float y_bar { get; private set; }





    public void CalculateGeometry()
    {
        // Calculate areas
        innerHalfArea = innerHalfSpan * (rootChord + midChord) * 0.5f;
        outerHalfArea = outerHalfSpan * (midChord + tipChord) * 0.5f;

        totalArea = (innerHalfArea + outerHalfArea) * 2;
        totalSpan = (innerHalfSpan + outerHalfSpan)*2;


        // Calculate MAC properties
        float l_i = midChord / rootChord;
        float l_o = tipChord / midChord;

        float sweepRatio_in = Mathf.Tan(innerSweep * Mathf.Deg2Rad);
        float sweepRatio_out = Mathf.Tan(outerSweep * Mathf.Deg2Rad);

        y_inner = innerHalfSpan * 0.33333333f * (1 + 2*l_i)/(1 + l_i);
        y_outer = outerHalfSpan * 0.33333333f * (1 + 2*l_o)/(1 + l_o) + innerHalfSpan;

        c_inner = 0.6666667f * rootChord * (1 + l_i + l_i*l_i) / (1 + l_o);
        c_outer = 0.6666667f * midChord * (1 + l_o + l_o*l_o) / (1 + l_o);

        x_inner = y_inner*sweepRatio_in + c_inner*0.25f;
        x_outer = y_outer*sweepRatio_out + c_outer*0.25f + innerHalfSpan*sweepRatio_in;

        
        c_bar = (c_inner * innerHalfArea + c_outer * outerHalfArea) / (innerHalfArea + outerHalfArea);
        x_bar = (x_inner * innerHalfArea + x_outer * outerHalfArea) / (innerHalfArea + outerHalfArea);
        y_bar = (y_inner * innerHalfArea + y_outer * outerHalfArea) / (innerHalfArea + outerHalfArea);
    }
}
