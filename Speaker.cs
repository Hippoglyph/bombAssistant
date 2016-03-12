using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace BombAssistant
{
    class Speaker
    {
        SpeechSynthesizer synth;
        public Speaker()
        {
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
        }

        public void speakAsync(String phrase)
        {
            Console.WriteLine("Expert: " + phrase);
            synth.SpeakAsync(phrase);
        }

        public void speak(String phrase)
        {
            Console.WriteLine("Expert: " + phrase);
            synth.Speak(phrase);
        }

        public void setSpeakRate(int rate)
        {
            synth.Rate = rate;
        }
    }
}
