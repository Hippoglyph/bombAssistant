using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class Assistant
    {
        Speaker talk;
        Listener rec;
        bool running;
        String[] input;
        public Assistant()
        {
            talk = new Speaker();
            rec = new Listener(this);
        }
        public void start()
        {
            intruduce();
            running = true;
            while (running)
            {
                int command = rec.listenForCommand();
                if (command == Listener.SETSPEEDCOMMAND)
                    setSpeakRateCommand();
                else if (command == Listener.EXITCOMMAND)
                    exit();
                else if (command == Listener.BUTTONCOMMAND)
                    new ButtonModule(talk, rec, input);
                else if (command == Listener.WIRECOMMAND)
                    new WireModule(talk, rec, input);
                else if (command == Listener.KEYPADCOMMAND)
                    new KeypadModule(talk, rec, input);
                else
                    unkownedCommand();
                Console.WriteLine();
            }
        }

        

        private void intruduce()
        {
            talk.speak("To your service");
        }

        private void exit()
        {
            talk.speak("Bye");
            running = false;
        }

        private void unkownedCommand()
        {   
            talk.speakAsync("Unknowned command, please repeat");
        }

        private void setSpeakRateCommand()
        {
            setSpeakRate();
            talk.speak("From now on I will speak in this rate, does this please you?");
        }

        public void setInput(String[] input)
        {
            this.input = input;
            Console.WriteLine();
            Console.Write("You:    ");
            foreach(String word in input)
            {
                Console.Write(word + " ");
            }
            Console.WriteLine();
        }

        private void setSpeakRate()
        {
            talk.setSpeakRate(int.Parse(input[1]));
        }
    }
}
