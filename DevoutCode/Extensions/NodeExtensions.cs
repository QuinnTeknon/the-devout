using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Devout.DevoutCode.Extensions;

public static class NodeExtensions {
    public static Node2D Define(this Node2D node, Color color, float whiteness = 0, int rotation = 0, Creature? target = null) {
        color.V = (float) (1.0 + whiteness * 14);
        node.Modulate = color;
        node.RotationDegrees += rotation;
        //node.CreateTween().TweenProperty(node, "modulate.v", 1f, 0.25f).From(1.0 + whiteness * 14);
        if (target?.GetCreatureNode() != null)
            node.GlobalPosition = target.GetCreatureNode().VfxSpawnPosition;
        return node;
    }
}