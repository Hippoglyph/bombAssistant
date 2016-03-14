using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class Assistant
    {
        public static int UNKNOWED = -1;
        public static int FALSE = 0;
        public static int TRUE = 1;

        Speaker talk;
        Listener rec;
        bool running;
        String[] input;
        int strikes;
        int NOFBatteries;
        int lastDigitOdd;
        int CAR;
        int FRK;
        int hasVowel;
        public Assistant()
        {
            talk = new Speaker();
            rec = new Listener(this);
            init();
        }

        private void init()
        {
            strikes = 0;
            NOFBatteries = UNKNOWED;
            lastDigitOdd = UNKNOWED;
            CAR = UNKNOWED;
            FRK = UNKNOWED;
            hasVowel = UNKNOWED;
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
                    new ButtonModule(talk, rec, input, this);
                else if (command == Listener.WIRECOMMAND)
                    new WireModule(talk, rec, input, this);
                else if (command == Listener.KEYPADCOMMAND)
                    new KeypadModule(talk, rec, input);
                else if (command == Listener.SETSTRIKESCOMMAND)
                    setStrikesCommand();
                else if (command == Listener.SIMONSAYSCOMMAND)
                    new SimonSaysModule(talk, rec, input, this);
                else if (command == Listener.WHOSONFIRSTCOMMAND)
                    new WhosOnFirstModule(talk, rec, input);
                else if (command == Listener.MEMORYCOMMAND)
                    new MemoryModule(talk, rec, input);
                else if (command == Listener.RESETCOMMAND)
                    reset();
                else if (command == Listener.MAZESCOMMAND)
                    new MazesModule(talk, rec);
                else
                    unkownedCommand();
                Console.WriteLine();
            }
        }

        private void reset()
        {
            init();
            StringBuilder sb = new StringBuilder();
            foreach (String word in input)
                sb.Append(word + " ");
            String answer = sb.ToString().Trim();
            if (answer.Equals(Listener.WIN))
                talk.speakAsync("We fucking did it man. Mom get the camera!");
            else
                talk.speakAsync("You should have listen, better luck next time");
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

        private void setStrikesCommand()
        {
            setStrikes();
            talk.speakAsync("Strikes set to " + strikes);
        }

        public void setInput(String[] input)
        {
            this.input = input;
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

        private void setStrikes()
        {
            strikes = int.Parse(input[1]);
        }

        public int getStrikes()
        {
            return strikes;
        }

        public void setLastDigitOdd(int n)
        {
            lastDigitOdd = n;
        }

        public int getLastDigiOdd()
        {
            return lastDigitOdd;
        }

        public void setNOFBatteries(int n)
        {
            NOFBatteries = n;
        }

        public int getNOFBatteries()
        {
            return NOFBatteries;
        }

        public void setCARIndicator(int n)
        {
            CAR = n;
        }

        public int getCARIndicator()
        {
            return CAR;
        }

        public void setFRKIndicator(int n)
        {
            FRK = n;
        }

        public int getFRKIndicator()
        {
            return FRK;
        }

        public void setHasVowel(int n)
        {
            hasVowel = n;
        }

        public int getHasVowel()
        {
            return hasVowel;
        }
    }
}
