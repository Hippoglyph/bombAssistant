using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class ButtonModule
    {
        
        public static String ABORT = "abort";
        public static String DETONATE = "detonate";
        public static String HOLD = "hold";

        Speaker talk;
        Listener rec;
        String[] input;
        public ButtonModule(Speaker talk, Listener rec, String[] input)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            solve();
        }


        private void solve()
        {
            //talk.speakAsync("You have a " + input[1] + " " + input[0] + " that says " + input[2]);
            String text = input[2];
            String color = input[1];
            int NOFbatteries = -1;
            if (color.Equals(Listener.BLUE) && text.Equals(ABORT))
            {
                holdStrip();
                return;
            }
            if (text.Equals(DETONATE))
            {
                talk.speakAsync("How many batteries?");
                NOFbatteries = rec.getNumber();
                if (NOFbatteries > 1)
                {
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
            if (NOFbatteries == -1)
            {
                talk.speakAsync("How many batteries?");
                NOFbatteries = rec.getNumber();
            }
            if (NOFbatteries > 2)
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
            if (text.Equals(HOLD) && color.Equals(Listener.RED))
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
    }
}
