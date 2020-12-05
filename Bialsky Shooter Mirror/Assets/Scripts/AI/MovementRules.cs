using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.AI
{
    public static class MovementRules
    {
        public static string[] GetAngleRules()
        {
            List<string> rules = new List<string>();
            rules.Add("if (FrontalDistance is Far) and (LeftDistance is Far) then (Angle is LittleNegative)");
            rules.Add("if (FrontalDistance is Far) and (RightDistance is Far) then (Angle is LittlePositive)");
            rules.Add("if (FrontalDistance is Far) and (LeftDistance is Medium) and (RightDistance is Near) then (Angle is LittleNegative)");
            rules.Add("if (FrontalDistance is Far) and (LeftDistance is Near) and (RightDistance is Medium) then (Angle is LittlePositive)");
            rules.Add("if (FrontalDistance is Far) and (LeftDistance is Medium) and (RightDistance is Medium) then (Angle is Zero)");
            rules.Add("if (FrontalDistance is Far) and (LeftDistance is Near) and (RightDistance is Near) then (Angle is Zero)");

            rules.Add("if (FrontalDistance is Medium) and (LeftDistance is Far) then (Angle is Negative)");
            rules.Add("if (FrontalDistance is Medium) and (RightDistance is Far) then (Angle is Positive)");
            rules.Add("if (FrontalDistance is Medium) and (LeftDistance is Medium) and (RightDistance is Near) then (Angle is Negative)");
            rules.Add("if (FrontalDistance is Medium) and (LeftDistance is Near) and (RightDistance is Medium) then (Angle is Positive)");
            rules.Add("if (FrontalDistance is Medium) and (LeftDistance is Medium) and (RightDistance is Medium) then (Angle is Zero)");
            rules.Add("if (FrontalDistance is Medium) and (LeftDistance is Near) and (RightDistance is Near) then (Angle is Zero)");

            rules.Add("if (FrontalDistance is Near) and (LeftDistance is Far) then (Angle is VeryNegative)");
            rules.Add("if (FrontalDistance is Near) and (RightDistance is Far) then (Angle is VeryPositive)");
            rules.Add("if (FrontalDistance is Near) and (LeftDistance is Medium) and (RightDistance is Near) then (Angle is VeryNegative)");
            rules.Add("if (FrontalDistance is Near) and (LeftDistance is Near) and (RightDistance is Medium) then (Angle is VeryPositive)");
            rules.Add("if (FrontalDistance is Near) and (LeftDistance is Medium) and (RightDistance is Medium) then (Angle is Zero)");
            rules.Add("if (FrontalDistance is Near) and (LeftDistance is Near) and (RightDistance is Near) then (Angle is Zero)");
            return rules.ToArray();
        }
    }
}
