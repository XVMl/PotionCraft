using System;
using Microsoft.Xna.Framework;
using PotionCraft.Content.System;

namespace PotionCraft.Content.UI.CraftUI;

public class ParabolaElement: PotionElement<BrewPotionState>
{
    private Vector2 startPos;
    private Vector2 endPos;
    
    private Vector2 middlePos;

    private Func<float,float> Parabola;

    public ParabolaElement(Vector2 startPos, Vector2 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        Init();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        var posy=Parabola?.Invoke(startPos.X) ?? startPos.Y;
        Top.Set(posy,0);
        Left.Set(startPos.X,0);
    }

    private void Init()
    {
        var width = startPos.X - endPos.X;
        var height =startPos.Y - endPos.Y;
        middlePos = new Vector2(startPos.X + width/2, startPos.Y+ height/2);
        Parabola = (x) =>
        {
            var parabola = CalculateParabolaCoefficients(startPos, middlePos, endPos);
            return parabola.a * x * x + parabola.b * x + parabola.c;
        };
    }
    
    // 计算抛物线系数a、b、c
    private static (float a, float b, float c) CalculateParabolaCoefficients(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        var matrices = new Matrix[1, 1, 1];
        var b = ((p1.Y - p3.Y) * (p1.X * p1.X - p2.X * p2.X) - (p1.Y - p2.Y) * (p1.X * p1.X - p3.X * p3.X)) /
                ((p1.X - p3.X) * (p1.X * p1.X - p2.X * p2.X) - (p1.X - p2.X) * (p1.X * p2.X - p3.X * p3.X));

        var a = ((p1.Y - p2.Y) - b * (p1.X - p2.X)) / (p1.X * p1.X - p2.X * p2.X);

        var c = p1.Y - a * p1.X * p1.X - b * p1.X;
        return (a, b, c);
    }
}