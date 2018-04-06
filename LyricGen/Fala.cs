using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace LyricGen
{
    public static class Fala
    {
        public static void teste()
        {

            PrepareBase.readAllFiles();
            PrepareBase.mountList();

            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            //synth.Speak("Frase de exemplo");

            synth.Volume = 100;

            int i;
            for (i = 0; i < PrepareBase.listSelectedVerses.Length; i++)
            {
                synth.Speak(PrepareBase.listSelectedVerses[i]);

            }
            
        }

    }
}
