using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    [CreateAssetMenu(fileName = "Progression", menuName = "ScriptableObjects/Progression")]
    public class Progression : ScriptableObject
    {
        public ClassStat[] statsDefinitions;
        public ProgressionCreatureClass[] creatureClasses = new ProgressionCreatureClass[0];
        public ProgressionCreatureModifier[] creatureModifiers = new ProgressionCreatureModifier[0];
        Dictionary<ClassType, Dictionary<ClassStatType, float[]>> progressionBook;
        Dictionary<CreatureType, Dictionary<ClassStatType, float[]>> progressionModifiersBook;

        public int GetLevel(ClassType classType, float experiencePoints)
        {
            if (progressionBook == null) InitProgressionBook();
            if (progressionModifiersBook == null) InitProgressionModifiersBook();
            int level = 1;
            foreach (var expToLevelUp in progressionBook[classType][ClassStatType.LevelBasedOnExperience])
            {
                if (experiencePoints < expToLevelUp) return level;
                ++level;
            }
            return level;
        }

        public float GetStat(ClassType classType, CreatureType creatureType, ClassStatType statType, int level)
        {
            if (progressionBook == null) InitProgressionBook();
            if (progressionModifiersBook == null) InitProgressionModifiersBook();
            var stats = progressionBook[classType][statType];
            float statModifier = 0f;
            var constraintedLevel = Mathf.Min(level - 1, stats.Length - 1);
            if (progressionModifiersBook[creatureType].ContainsKey(statType))
            {
                statModifier = progressionModifiersBook[creatureType][statType][constraintedLevel];
            }
            return stats[level-1] + statModifier;
        }

        public ClassStat GetStatDefinition(ClassStatType statType)
        {
            foreach (var stat in statsDefinitions)
            {
                if (stat.statType == statType) return stat;
            }
            throw new System.NullReferenceException();
        }

        private void InitProgressionBook()
        {
            progressionBook = new Dictionary<ClassType, Dictionary<ClassStatType, float[]>>();
            foreach (var creatureClass in creatureClasses)
            {
                progressionBook[creatureClass.classType] = new Dictionary<ClassStatType, float[]>();
                foreach (var stat in creatureClass.stats)
                {
                    progressionBook[creatureClass.classType][stat.stat] = stat.values;
                }
            }
        }

        private void InitProgressionModifiersBook()
        {
            progressionModifiersBook = new Dictionary<CreatureType, Dictionary<ClassStatType, float[]>>();
            foreach (var creatureType in creatureModifiers)
            {
                progressionModifiersBook[creatureType.creatureType] = new Dictionary<ClassStatType, float[]>();
                foreach (var stat in creatureType.stats)
                {
                    progressionModifiersBook[creatureType.creatureType][stat.stat] = stat.values;
                }
            }
        }
    }
}
