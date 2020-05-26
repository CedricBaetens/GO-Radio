using GameOverlay.Drawing;
using GameOverlay.Windows;
using GO_Radio.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GO_Radio
{
    public interface IOverlay
    {
        bool Showed { get; }

        void Show();
        void Hide();
        void DisplayLoadedSound(Sound sound);
        void DisplayPlayingSound(Sound sound);
        void DisplayStoppedSound(Sound sound);
        void DisplayPauzedSound(Sound sound);
    }

    public class Overlay : IOverlay
    {
        private GraphicsWindow _Window;

        public bool Showed { get; private set; }

        private Graphics _Gfx = new Graphics();

        private void Window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            //var gfx = e.Graphics;
            ////var padding = 16;
            ////var infoText = new StringBuilder()
            ////    .Append("FPS: ").Append(gfx.FPS.ToString().PadRight(padding))
            ////    .Append("FrameTime: ").Append(e.FrameTime.ToString().PadRight(padding))
            ////    .Append("FrameCount: ").Append(e.FrameCount.ToString().PadRight(padding))
            ////    .Append("DeltaTime: ").Append(e.DeltaTime.ToString().PadRight(padding))
            ////    .ToString();

            ////gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, infoText);

            //if (_ActiveSound != null)
            //    gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"{_ActiveSound.Name}");
            //else
            //    gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"No active song");
        }

        public Overlay()
        {
            Show();
        }

        public void Show()
        {
            _Gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };
            _Window = new GraphicsWindow(0, 0, 800, 600, _Gfx)
            {
                FPS = 60,
                IsTopmost = true,
                IsVisible = true
            };
            //_Window.DrawGraphics += Window_DrawGraphics;
            _Window.Create();
            Showed = true;

        }
        public void Hide()
        {
            _Window.Dispose();
            Showed = false;
        }

        public void DisplayLoadedSound(Sound sound)
        {
            if (Showed == false)
                return;

            if (sound != null)
                Display((gfx) => gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"Loaded: {sound.Name}"));
        }
        public void DisplayPlayingSound(Sound sound)
        {
            if (Showed == false)
                return;

            if (sound != null)
                Display((gfx) => gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"Playing: {sound.Name}"));
        }
        public void DisplayStoppedSound(Sound sound)
        {
            if (Showed == false)
                return;

            if (sound != null)
                Display((gfx) => gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"Stopped: {sound.Name}"));
        }
        public void DisplayPauzedSound(Sound sound)
        {
            if (Showed == false)
                return;

            if (sound != null)
                Display((gfx) => gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"Pauzed: {sound.Name}"));
        }

        private void Display(Action<Graphics> func)
        {
            _Gfx.BeginScene();
            _Gfx.ClearScene();
            func(_Gfx);
            _Gfx.EndScene();
        }
    }
}
