using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.SkillSystem
{
    public static class SkillBindingManager
    {
        public static List<string> availableBindings;

        static SkillBindingManager()
        {
            availableBindings = new List<string>();
            Controls controls = new Controls();
            foreach (var binding in controls.Player.UseSkill.bindings)
            {
                availableBindings.Add(binding.ToDisplayString());
            }
        }

        public static string PopAvailableBinding()
        {
            if (availableBindings.Count == 0) return null;
            var binding = availableBindings[0];
            availableBindings.Remove(binding);
            return binding;
        }
    }
}
