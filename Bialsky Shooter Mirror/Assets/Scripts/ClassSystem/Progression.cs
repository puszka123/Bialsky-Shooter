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
        Dictionary<ClassType, Dictionary<ClassStatType, float[]>> progressionBook;

        public int GetLevel(ClassType classType, float experiencePoints)
        {
            if (progressionBook == null) InitProgressionBook();
            int level = 1;
            foreach (var expToLevelUp in progressionBook[classType][ClassStatType.LevelBasedOnExperience])
            {
                if (experiencePoints < expToLevelUp) return level;
                ++level;
            }
            return level;
        }

        public float GetStat(ClassType classType, ClassStatType statType, int level)
        {
            if (progressionBook == null) InitProgressionBook();
            var stats = progressionBook[classType][statType];
            if (level >= stats.Length) return stats[stats.Length - 1];
            else return stats[level];
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
    }
}
