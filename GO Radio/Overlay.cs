using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio
{
    class Overlay
    {
        private GraphicsWindow _Window;

        public Overlay()
        {
            var gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };
            _Window = new GraphicsWindow(0, 0, 800, 600, gfx)
            {
                FPS = 60,
                IsTopmost = true,
                IsVisible = true
            };
            _Window.DrawGraphics += Window_DrawGraphics;
        }

        private void Window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            var padding = 16;
            var infoText = new StringBuilder()
                .Append("FPS: ").Append(gfx.FPS.ToString().PadRight(padding))
                .Append("FrameTime: ").Append(e.FrameTime.ToString().PadRight(padding))
                .Append("FrameCount: ").Append(e.FrameCount.ToString().PadRight(padding))
                .Append("DeltaTime: ").Append(e.DeltaTime.ToString().PadRight(padding))
                .ToString();
            gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, infoText);
        }

        public void Show()
        {
            _Window.Create();
            _Window.Join();
        }
    }
}
