using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    public class Experience : NetworkBehaviour
    {
        public event Action<float> serverOnExperienceGained;
        [SerializeField] float experiencePoints;

        public float ExperiencePoints { get { return experiencePoints; } }

        #region Server

        [Server]
        public void GainExperience(float additionalExperience)
        {
            experiencePoints += additionalExperience;
            serverOnExperienceGained?.Invoke(experiencePoints);
        }

        #endregion
    }
}
