using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BialskyShooter.AI
{
    public class TeamMember : MonoBehaviour
    {
        public Guid MemberId { get; private set; }
        public Guid TeamId { get; private set; }

        public void Init(Guid memberId, Guid teamId)
        {
            MemberId = memberId;
            TeamId = teamId;
        }
    }
}
