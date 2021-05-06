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
        void DisplayPlayingSound();
        void DisplayStoppedSound();
        void DisplayPauzedSound();
    }

    public class Overlay : IOverlay
    {
        private GraphicsWindow _Window;

        public bool Showed { get; private set; }

        private Graphics _Gfx = new Graphics();


        private Sound _LoadedSound;
        private string _SoundState;

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

            // Sound
            //if (_LoadedSound != null)
            //    gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"{_LoadedSound.Id.ToString("0000")} {_LoadedSound.Name}");
            //else
            //    gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 20, "");

            //// Soundstate
            //gfx.DrawTextWithBackground(gfx.CreateFont("Consolas", 14), gfx.CreateSolidBrush(255, 0, 0), gfx.CreateSolidBrush(0, 255, 0), 20, 50, _SoundState ?? "");
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
                _LoadedSound = sound;

            Draw();
        }
        public void DisplayPlayingSound()
        {
            if (Showed == false)
                return;

            if (_LoadedSound != null)
                _SoundState = "Playing";

            Draw();
        }
        public void DisplayStoppedSound()
        {
            if (Showed == false)
                return;

            if (_LoadedSound != null)
                _SoundState = "Stopped";

            Draw();
        }
        public void DisplayPauzedSound()
        {
            if (Showed == false)
                return;

            if (_LoadedSound != null)
                _SoundState = "Pauzed";

            Draw();
        }

        private void Draw()
        {
            _Gfx.BeginScene();
            _Gfx.ClearScene();

            // Sound
            if (_LoadedSound != null)
                _Gfx.DrawTextWithBackground(_Gfx.CreateFont("Consolas", 14), _Gfx.CreateSolidBrush(255, 0, 0), _Gfx.CreateSolidBrush(0, 255, 0), 20, 20, $"{_LoadedSound.Id.ToString("0000")} {_LoadedSound.Name}");
            else
                _Gfx.DrawTextWithBackground(_Gfx.CreateFont("Consolas", 14), _Gfx.CreateSolidBrush(255, 0, 0), _Gfx.CreateSolidBrush(0, 255, 0), 20, 20, "");

            // Soundstate
            _Gfx.DrawTextWithBackground(_Gfx.CreateFont("Consolas", 14), _Gfx.CreateSolidBrush(255, 0, 0), _Gfx.CreateSolidBrush(0, 255, 0), 20, 50, _SoundState ?? "");

            _Gfx.EndScene();
        }
    }
}
