using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ClassSystem
{
    [CreateAssetMenu(fileName = "Progression", menuName = "ScriptableObjects/Progression")]
    public class Progression : ScriptableObject
    {
        public Stat[] statsDefinitions;
        public ProgressionCreatureClass[] creatureClasses = new ProgressionCreatureClass[0];
        Dictionary<ClassType, Dictionary<StatType, float[]>> progressionBook;

        public int GetLevel(ClassType classType, float experiencePoints)
        {
            if (progressionBook == null) InitProgressionBook();
            int level = 1;
            foreach (var expToLevelUp in progressionBook[classType][StatType.LevelBasedOnExperience])
            {
                if (experiencePoints < expToLevelUp) return level;
                ++level;
            }
            return level;
        }

        public float GetStat(ClassType classType, StatType statType, int level)
        {
            if (progressionBook == null) InitProgressionBook();
            return progressionBook[classType][statType][level];
        }

        public Stat GetStatDefinition(StatType statType)
        {
            foreach (var stat in statsDefinitions)
            {
                if (stat.statType == statType) return stat;
            }
            throw new System.NullReferenceException();
        }

        private void InitProgressionBook()
        {
            progressionBook = new Dictionary<ClassType, Dictionary<StatType, float[]>>();
            foreach (var creatureClass in creatureClasses)
            {
                progressionBook[creatureClass.classType] = new Dictionary<StatType, float[]>();
                foreach (var stat in creatureClass.stats)
                {
                    progressionBook[creatureClass.classType][stat.stat] = stat.values;
                }
            }
        }
    }
}
