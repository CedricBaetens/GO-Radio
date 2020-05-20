﻿using GameOverlay.Drawing;
using GameOverlay.Windows;
using GO_Radio.Classes;
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

        public bool Showed { get; private set; }


        private SoundNew _ActiveSound;

        private void Window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            //var padding = 16;
            //var infoText = new StringBuilder()
            //    .Append("FPS: ").Append(gfx.FPS.ToString().PadRight(padding))
            //    .Append("FrameTime: ").Append(e.FrameTime.ToString().PadRight(padding))
            //    .Append("FrameCount: ").Append(e.FrameCount.ToString().PadRight(padding))
            //    .Append("DeltaTime: ").Append(e.DeltaTime.ToString().PadRight(padding))
            //    .ToString();

            //gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, infoText);

            if (_ActiveSound != null)
                gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"{_ActiveSound.Name}");
            else
                gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"No active song");
        }

        public void Show()
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
            _Window.Create();

            Showed = true;
        }

        public void Hide()
        {
            _Window.Dispose();
            Showed = false;
        }

        public void DisplaySound(SoundNew sound)
        {
            _ActiveSound = sound;
        }
    }
}