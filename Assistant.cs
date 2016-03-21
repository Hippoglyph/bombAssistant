using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class Assistant
    {
        public static int UNKNOWN = -1;
        public static int FALSE = 0;
        public static int TRUE = 1;

        Random random;

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
            random = new Random();
            talk = new Speaker();
            rec = new Listener(this);
            init();
        }

        private void init()
        {
            strikes = 0;
            NOFBatteries = UNKNOWN;
            lastDigitOdd = UNKNOWN;
            CAR = UNKNOWN;
            FRK = UNKNOWN;
            hasVowel = UNKNOWN;
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
                sayWin();
            else
                sayLose();
        }

        private void sayWin()
        {
            int i = random.Next(5);
            switch (i)
            {
                case 0:
                    talk.speakAsync("We fucking did it man. Mom get the camera!");
                    break;
                case 1:
                    talk.speakAsync("We did it, it is finally over!");
                    break;
                case 2:
                    talk.speakAsync("I am proud of you");
                    break;
                case 3:
                    talk.speakAsync("Easy! Call me again if you need any more help");
                    break;
                case 4:
                    talk.speakAsync("We did it. I would say I did most of the work");
                    break;
            }
        }

        private void sayLose()
        {
            int i = random.Next(5);
            switch (i)
            {
                case 0:
                    talk.speakAsync("You should have listen, better luck next time");
                    break;
                case 1:
                    talk.speakAsync("You should have been more clear with your instructions");
                    break;
                case 2:
                    talk.speakAsync("You are a disgrace to your family!");
                    break;
                case 3:
                    talk.speakAsync("Oh! Are you okey?");
                    break;
                case 4:
                    talk.speakAsync("I am sorry, Dave spilt some coffee in my CPU fan during the last module");
                    break;
            }
        }

        private void intruduce()
        {
            int i = random.Next(5);
            switch (i)
            {
                case 0:
                    talk.speak("To your service");
                    break;
                case 1:
                    talk.speak("What can I do for you?");
                    break;
                case 2:
                    talk.speak("Ready to roll out!");
                    break;
                case 3:
                    talk.speak("Trubble again?");
                    break;
                case 4:
                    talk.speak("Tell me what you see!");
                    break;
            }
            
        }

        private void exit()
        {
            int i = random.Next(5);
            switch (i)
            {
                case 0:
                    talk.speak("Bye");
                    break;
                case 1:
                    talk.speak("See ya");
                    break;
                case 2:
                    talk.speak("Goodbye");
                    break;
                case 3:
                    talk.speak("Exiting");
                    break;
                case 4:
                    talk.speak("au revoir");
                    break;
            }
            running = false;
        }

        private void unkownedCommand()
        {   
            talk.speakAsync("Unknown command, please repeat");
        }

        private void setSpeakRateCommand()
        {
            setSpeakRate();
            talk.speakAsync("From now on I will speak in this rate, does this please you?");
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
