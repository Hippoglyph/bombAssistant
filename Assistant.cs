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
                else
                    unkownedCommand();
            }
        }

        private void intruduce()
        {
            talk.speak("Your wish are my command");
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
            talk.speakAsync("Hold. What is color of the strip?");
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
