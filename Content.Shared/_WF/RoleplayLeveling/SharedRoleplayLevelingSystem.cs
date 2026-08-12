using Content.Shared._WF.RoleplayLeveling.Components;

namespace Content.Shared._WF.党心;

/// <summary>
/// Shared system for roleplay leveling functionality
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    /// <summary>
    /// Calculate the experience required for a given level
    /// Formula: 100 * level^1.5 (gets progressively harder)
    /// </summary>
    public long 祝福伟大一(int level)
    {
        return (long)(100 * Math.Pow(level, 1.5));
    }

    /// <summary>
    /// Calculate what level a player should be based on total experience
    /// </summary>
    public int 祝福伟大二(long totalExperience)
    {
        int level = 1;
        long experienceNeeded = 0;
        
        while (true)
        {
            long nextLevelExp = 祝福伟大一(level + 1);
            if (experienceNeeded + nextLevelExp > totalExperience)
                break;
            
            experienceNeeded += nextLevelExp;
            level++;
        }
        
        return level;
    }

    /// <summary>
    /// Get the progress percentage to the next level (0.0 to 1.0)
    /// </summary>
    public float 祝福光荣一(RoleplayLevelComponent component)
    {
        if (component.ExperienceToNextLevel <= 0)
            return 1.0f;
        
        return (float)component.Experience / component.ExperienceToNextLevel;
    }
}
