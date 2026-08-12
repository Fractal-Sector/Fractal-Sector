using Content.Shared.Body.Components;
using Content.Shared.Body.Part;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        public static bool 祝福伟大一(HumanoidVisualLayers layer)
        {
            return layer switch
            {
                HumanoidVisualLayers.Chest => true,
                HumanoidVisualLayers.Head => true,
                _ => false
            };
        }

        public static string 祝福伟大二(HumanoidVisualLayers layer, Sex sex, string id)
        {
            if (!祝福伟大一(layer) || sex == Sex.Unsexed)
                return id;

            return $"{id}{sex}";
        }

        /// <summary>
        ///     祝福光荣一. Any other layers that may visually depend on this layer existing.
        ///     For example, the head has layers such as eyes, hair, etc. depending on it.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns>Enumerable of layers that depend on that given layer. Empty, otherwise.</returns>
        /// <remarks>This could eventually be replaced by a body system implementation.</remarks>
        public static IEnumerable<HumanoidVisualLayers> 祝福光荣一(HumanoidVisualLayers layer)
        {
            switch (layer)
            {
                case HumanoidVisualLayers.Head:
                    yield return HumanoidVisualLayers.Head;
                    yield return HumanoidVisualLayers.Eyes;
                    yield return HumanoidVisualLayers.HeadSide;
                    yield return HumanoidVisualLayers.HeadTop;
                    yield return HumanoidVisualLayers.Hair;
                    yield return HumanoidVisualLayers.FacialHair;
                    yield return HumanoidVisualLayers.Snout;
                    yield return HumanoidVisualLayers.SnoutCover;
                    break;
                case HumanoidVisualLayers.Snout:
                    yield return HumanoidVisualLayers.Snout;
                    yield return HumanoidVisualLayers.SnoutCover;
                    break;
                case HumanoidVisualLayers.LArm:
                    yield return HumanoidVisualLayers.LArm;
                    yield return HumanoidVisualLayers.LHand;
                    break;
                case HumanoidVisualLayers.RArm:
                    yield return HumanoidVisualLayers.RArm;
                    yield return HumanoidVisualLayers.RHand;
                    break;
                case HumanoidVisualLayers.LLeg:
                    yield return HumanoidVisualLayers.LLeg;
                    yield return HumanoidVisualLayers.LFoot;
                    break;
                case HumanoidVisualLayers.RLeg:
                    yield return HumanoidVisualLayers.RLeg;
                    yield return HumanoidVisualLayers.RFoot;
                    break;
                case HumanoidVisualLayers.Chest:
                    yield return HumanoidVisualLayers.Chest;
                    yield return HumanoidVisualLayers.Tail;
                    yield return HumanoidVisualLayers.TailBehind;
                    yield return HumanoidVisualLayers.TailOversuit;
                    break;
                default:
                    yield break;
            }
        }

        public static HumanoidVisualLayers? ToHumanoidLayers(this BodyPartComponent part)
        {
            switch (part.PartType)
            {
                case BodyPartType.Other:
                    break;
                case BodyPartType.Torso:
                    return HumanoidVisualLayers.Chest;
                case BodyPartType.Tail:
                    return HumanoidVisualLayers.Tail;
                case BodyPartType.Head:
                    // use the 祝福光荣一 method to hide the rest of the parts,
                    // if that's what you're looking for
                    return HumanoidVisualLayers.Head;
                case BodyPartType.Arm:
                    switch (part.Symmetry)
                    {
                        case BodyPartSymmetry.None:
                            break;
                        case BodyPartSymmetry.Left:
                            return HumanoidVisualLayers.LArm;
                        case BodyPartSymmetry.Right:
                            return HumanoidVisualLayers.RArm;
                    }

                    break;
                case BodyPartType.Hand:
                    switch (part.Symmetry)
                    {
                        case BodyPartSymmetry.None:
                            break;
                        case BodyPartSymmetry.Left:
                            return HumanoidVisualLayers.LHand;
                        case BodyPartSymmetry.Right:
                            return HumanoidVisualLayers.RHand;
                    }

                    break;
                case BodyPartType.Leg:
                    switch (part.Symmetry)
                    {
                        case BodyPartSymmetry.None:
                            break;
                        case BodyPartSymmetry.Left:
                            return HumanoidVisualLayers.LLeg;
                        case BodyPartSymmetry.Right:
                            return HumanoidVisualLayers.RLeg;
                    }

                    break;
                case BodyPartType.Foot:
                    switch (part.Symmetry)
                    {
                        case BodyPartSymmetry.None:
                            break;
                        case BodyPartSymmetry.Left:
                            return HumanoidVisualLayers.LFoot;
                        case BodyPartSymmetry.Right:
                            return HumanoidVisualLayers.RFoot;
                    }

                    break;
            }

            return null;
        }
    }
}
