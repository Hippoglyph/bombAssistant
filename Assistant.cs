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
                    buttonCommand();
                else if (command == Listener.WIRECOMMAND)
                    wireCommand();
                else
                    unkownedCommand();
                Console.WriteLine();
            }
        }

        

        private void intruduce()
        {
            talk.speak("Your wish are my command");
        }

        private void wireCommand()
        {
            /*
            talk.speak("You said: ");
            foreach (string s in input)
                talk.speak(s);
            */

            if (input.Length == 4)
                wireThree();
            else if (input.Length == 5)
                wireFour();
            else if (input.Length == 6)
                wireFive();
            else if (input.Length == 7)
                wireSix();
        }

        private int getNumberOfWireColor(String color)
        {
            int count = 0;
            foreach (String wire in input)
            {
                if (wire.Equals(color))
                    count++;
            }
            return count;
        }

        private void wireThree()
        {
            bool red = false;
            foreach (String wire in input)
            {
                if (wire.Equals(Listener.RED))
                {
                    red = true;
                    break;
                }
            }
            if (!red)
            {
                talk.speakAsync("Cut the second wire!");
                return;
            }
            if (input.Last().Equals(Listener.WHITE))
            {
                talk.speakAsync("Cut the last wire!");
                return;
            }
            if(getNumberOfWireColor(Listener.BLUE) > 1)
            {
                talk.speakAsync("Cut the last blue wire!");
                return;
            }
            talk.speakAsync("Cut the last wire!");
        }

        private void wireFour()
        {
            int reds = getNumberOfWireColor(Listener.RED);
            if(reds > 1)
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                {
                    talk.speakAsync("Cut the last red wire!");
                    return;
                }
            }
            if(input.Last().Equals(Listener.YELLOW) && reds == 0)
            {
                talk.speakAsync("Cut the last wire!");
                return;
            }
            if(getNumberOfWireColor(Listener.BLUE) == 1)
            {
                talk.speakAsync("Cut the first wire!");
                return;
            }
            if(getNumberOfWireColor(Listener.YELLOW) > 1)
            {
                talk.speakAsync("Cut the last wire!");
                return;
            }
            talk.speakAsync("Cut the second wire!");
        }

        private void wireFive()
        {
            if (input.Last().Equals(Listener.BLACK))
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                {
                    talk.speakAsync("Cut the fourth wire!");
                    return;
                }
            }
            if(getNumberOfWireColor(Listener.YELLOW) > 1 && getNumberOfWireColor(Listener.RED) == 1)
            {
                talk.speakAsync("Cut the first wire!");
                return;
            }
            if(getNumberOfWireColor(Listener.BLACK) == 0)
            {
                talk.speakAsync("Cut the second wire!");
                return;
            }
            talk.speakAsync("Cut the first wire!");
        }

        private void wireSix()
        {
            int yellows = getNumberOfWireColor(Listener.YELLOW);
            if(yellows == 0)
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                {
                    talk.speakAsync("Cut the fourth wire!");
                    return;
                }
            }
            if(yellows == 1 && getNumberOfWireColor(Listener.WHITE) > 1)
            {
                talk.speakAsync("Cut the fourth wire!");
                return;
            }
            if(getNumberOfWireColor(Listener.RED) == 0)
            {
                talk.speakAsync("Cut the last wire!");
                return;
            }
            talk.speakAsync("Cut the fourth wire!");
        }

        private void buttonCommand()
        {
            //talk.speakAsync("You have a " + input[1] + " " + input[0] + " that says " + input[2]);
            String text = input[2];
            String color = input[1];
            int NOFbatteries = -1;
            if (color.Equals(Listener.BLUE) && text.Equals(Listener.ABORT))
            {
                holdStrip();
                return;
            }
            if (text.Equals(Listener.DETONATE))
            {
                talk.speakAsync("How many batteries?");
                NOFbatteries = rec.getNumber();
                if (NOFbatteries > 1) {
                    quickRelease();
                    return;
                }
            }
            if (color.Equals(Listener.WHITE))
            {
                talk.speakAsync("Is there a lit indicator labeled CAR?");
                if (rec.getYesNo())
                {
                    holdStrip();
                    return;
                }
            }
            if(NOFbatteries == -1)
            {
                talk.speakAsync("How many batteries?");
                NOFbatteries = rec.getNumber();
            }
            if(NOFbatteries > 2)
            {
                talk.speakAsync("Is there a lit indicator labeled FRK");
                if (rec.getYesNo())
                {
                    quickRelease();
                    return;
                }
            }
            if (text.Equals(Listener.YELLOW))
            {
                holdStrip();
                return;
            }
            if(text.Equals(Listener.HOLD) && color.Equals(Listener.RED))
            {
                quickRelease();
                return;
            }
            holdStrip();
        }

        private void quickRelease()
        {
            talk.speakAsync("Click the button!");
        }

        private void holdStrip()
        {
            talk.speakAsync("Hold. What is the color of the strip?");
            String color = rec.getColor();
            if (color.Equals(Listener.BLUE))
                talk.speakAsync("Release when 4 in any position!");
            else if (color.Equals(Listener.WHITE))
                talk.speakAsync("Release when 1 in any position!");
            else if (color.Equals(Listener.YELLOW))
                talk.speakAsync("Release when 5 in any position!");
            else
                talk.speakAsync("Release when 1 in any position!");
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
        }

        private void setSpeakRate()
        {
            talk.setSpeakRate(int.Parse(input[1]));
        }
    }
}
