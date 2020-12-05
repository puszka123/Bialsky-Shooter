using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Fuzzy.Library;

namespace BialskyShooter.AI
{
    public class MovementSystem
    {
        public MamdaniFuzzySystem inferenceSystem;
        private Dictionary<FuzzyVariable, double> result;

        public MovementSystem()
        {
            FuzzyTerm fsNear = new FuzzyTerm("Near", new TriangularMembershipFunction(0f, 0f, 5f));
            FuzzyTerm fsMedium = new FuzzyTerm("Medium", new TrapezoidMembershipFunction(0f, 5f, 10f, 20f));
            FuzzyTerm fsFar = new FuzzyTerm("Far", new TriangularMembershipFunction(10f, 20f, 20f));

            // Right Distance (Input)
            FuzzyVariable lvRight = new FuzzyVariable("RightDistance", 0, 120);
            lvRight.Terms.Add(fsNear);
            lvRight.Terms.Add(fsMedium);
            lvRight.Terms.Add(fsFar);

            // Left Distance (Input)
            FuzzyVariable lvLeft = new FuzzyVariable("LeftDistance", 0, 120);
            lvLeft.Terms.Add(fsNear);
            lvLeft.Terms.Add(fsMedium);
            lvLeft.Terms.Add(fsFar);

            // Front Distance (Input)
            FuzzyVariable lvFront = new FuzzyVariable("FrontalDistance", 0, 120);
            lvFront.Terms.Add(fsNear);
            lvFront.Terms.Add(fsMedium);
            lvFront.Terms.Add(fsFar);

            // Linguistic labels (fuzzy sets) that compose the angle
            FuzzyTerm fsVN = new FuzzyTerm("VeryNegative", new TriangularMembershipFunction(-40, -40, -35));
            FuzzyTerm fsN = new FuzzyTerm("Negative", new TrapezoidMembershipFunction(-40, -35, -25, -20));
            FuzzyTerm fsLN = new FuzzyTerm("LittleNegative", new TrapezoidMembershipFunction(-25, -20, -10, -5));
            FuzzyTerm fsZero = new FuzzyTerm("Zero", new TrapezoidMembershipFunction(-10, 5, 5, 10));
            FuzzyTerm fsLP = new FuzzyTerm("LittlePositive", new TrapezoidMembershipFunction(5, 10, 20, 25));
            FuzzyTerm fsP = new FuzzyTerm("Positive", new TrapezoidMembershipFunction(20, 25, 35, 40));
            FuzzyTerm fsVP = new FuzzyTerm("VeryPositive", new TriangularMembershipFunction(35, 40, 40));

            // Angle
            FuzzyVariable lvAngle = new FuzzyVariable("Angle", -50, 50);
            lvAngle.Terms.Add(fsVN);
            lvAngle.Terms.Add(fsN);
            lvAngle.Terms.Add(fsLN);
            lvAngle.Terms.Add(fsZero);
            lvAngle.Terms.Add(fsLP);
            lvAngle.Terms.Add(fsP);
            lvAngle.Terms.Add(fsVP);

            inferenceSystem = new MamdaniFuzzySystem();

            inferenceSystem.Input.Add(lvFront);
            inferenceSystem.Input.Add(lvLeft);
            inferenceSystem.Input.Add(lvRight);
            inferenceSystem.Output.Add(lvAngle);

            foreach (var rule in MovementRules.GetAngleRules())
            {
                inferenceSystem.Rules.Add(inferenceSystem.ParseRule(rule));
            }
        }

        public void Calculate(float frontDistance, float rightDistance, float leftDistance, out float angle)
        {
            FuzzyVariable right = inferenceSystem.InputByName("RightDistance");
            FuzzyVariable left = inferenceSystem.InputByName("LeftDistance");
            FuzzyVariable frontal = inferenceSystem.InputByName("FrontalDistance");

            Dictionary<FuzzyVariable, double> inputValues = new Dictionary<FuzzyVariable, double>();
            inputValues.Add(right, rightDistance);
            inputValues.Add(left, leftDistance);
            inputValues.Add(frontal, frontDistance);
            result = inferenceSystem.Calculate(inputValues);

            if (result == null || (result != null && result.Keys.Count == 0)) angle = -999.0f;
            FuzzyVariable value = inferenceSystem.OutputByName("Angle");
            angle = float.IsNaN((float)result[value]) ? -999.0f : (float)result[value];
        }
    }
}
