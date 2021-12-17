using System;
using System.Collections.Generic;
using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace KohSnowflake
{
    class Program
    {
        static void Main(string[] args)
        {
            var videoMode = new VideoMode(
                width: 800,
                height: 600,
                bpp: 24
            );

            var window = new RenderWindow(videoMode, "KOH Snowflake");

            window.Resized += (sender, eventArgs) => { window.SetView(new View(new FloatRect(0, 0, window.Size.X, window.Size.Y))); };

            window.Closed += (sender, eventArgs) => { window.Close(); };

            Clock clock = new Clock();

            window.SetFramerateLimit(60);

            int n = 0;

            int depth = 2;

            float scaleX = 350;
            float scaleY = 400;

            Vector3 a;
            a.X = -0.433f * scaleX;
            a.Y = -0.25f * scaleY;
            a.Z = 0.0f;
            Vector3 b;
            b.X = 0.0f * scaleX;
            b.Y = 0.5f * scaleY;
            b.Z = 0.0f;
            Vector3 c;
            c.X = 0.433f * scaleX;
            c.Y = -0.25f * scaleY;
            c.Z = 0.0f;

            List<Vector3> points = new();

            foreach (var vector3 in dividLine(a, b, depth)) points.Add(vector3);
            foreach (var vector3 in dividLine(b, c, depth)) points.Add(vector3);
            foreach (var vector3 in dividLine(c, a, depth)) points.Add(vector3);

            var convexShape = new ConvexShape((uint)points.Count);
            convexShape.FillColor = Color.White;
            for (int i = 0; i < points.Count; i++)
            {
                convexShape.SetPoint((uint)i, new Vector2f(points[i].X, points[i].Y));
            }

            // convexShape.Scale    = new Vector2f(400, 400);
            convexShape.Position = new Vector2f(400, 300);

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();

                window.Draw(convexShape);

                window.Display();
            }

            Console.WriteLine("Exiting");
        }

        private static IEnumerable<Vector3> dividLine(Vector3 a, Vector3 b, int Depth)
        {
            if (Depth == 0)
            {
                // yield return new Vector3(a.X, a.Y, a.Z);
                yield return new Vector3(b.X, b.Y, b.Z);
                // glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), &vertices, GL_DYNAMIC_DRAW);
                // glDrawArrays(GL_LINES, 0, 2);
            }
            else
            {
                Vector3 v1 = mix(a, b, 1.0f / 3.0f);
                Vector3 v3 = mix(a, b, 2.0f / 3.0f);
                Vector3 v2 = caculatev2(v1, v3);
                foreach (var vector3 in dividLine(a, v1, Depth - 1)) yield return vector3;
                foreach (var vector3 in dividLine(v1, v2, Depth - 1)) yield return vector3;
                foreach (var vector3 in dividLine(v2, v3, Depth - 1)) yield return vector3;
                foreach (var vector3 in dividLine(v3, b, Depth - 1)) yield return vector3;
            }
        }

        private static Vector3 mix(Vector3 a, Vector3 b, float length)
        {
            Vector3 v;
            v.X = a.X + (b.X - a.X) * length;
            v.Y = a.Y + (b.Y - a.Y) * length;
            v.Z = 0.0f;
            return v;
        }

        private static Vector3 caculatev2(Vector3 a, Vector3 b)
        {
            Vector3 v;
            v.X = b.X - a.X;
            v.Y = b.Y - a.Y;
            v.Z = 0.0f;
            Vector3 v2;
            v2.X =  (float)(v.X * Math.Cos(Radians(60.0f)) - v.Y * Math.Sin(Radians(60.0f)));
            v2.Y =  (float)(v.X * Math.Sin(Radians(60.0f)) + v.Y * Math.Cos(Radians(60.0f)));
            v2.X += a.X;
            v2.Y += a.Y;
            v2.Z =  0.0f;
            return v2;
        }

        private static double Radians(double angle)
        {
            return Math.PI * angle / 180.0f;
        }
    }
}