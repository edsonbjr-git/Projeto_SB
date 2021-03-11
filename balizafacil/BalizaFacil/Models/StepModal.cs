using System;
using System.Collections.Generic;
using System.Text;

namespace BalizaFacil.Models
{
    [Flags]
    public enum StepModal
    {
        None = 0,
        WrongWay = 1,
        CurbTouch = 2,
        SensorDisconnected = 4,
        Conclusion = 8,
        LeftWay = 16,
        RightWay = 32,
        OkaySpot = 64,
        FinalizedStep = 128,

    }
}
