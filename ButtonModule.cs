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
        public static String PRESS = "press";

        Speaker talk;
        Listener rec;
        String[] input;
        Assistant assistant;
        public ButtonModule(Speaker talk, Listener rec, String[] input, Assistant lord)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            assistant = lord;
            solve();
        }


        private void solve()
        {
            //talk.speakAsync("You have a " + input[1] + " " + input[0] + " that says " + input[2]);
            String text = input[2];
            String color = input[1];
            if (color.Equals(Listener.BLUE) && text.Equals(ABORT))
            {
                holdStrip();
                return;
            }
            if (text.Equals(DETONATE))
            {
                if (assistant.getNOFBatteries() == Assistant.UNKNOWN)
                {
                    talk.speakAsync("How many batteries?");
                    assistant.setNOFBatteries(rec.getNumber());
                    if (assistant.getNOFBatteries() > 1)
                    {
                        quickRelease();
                        return;
                    }
                }
                else if (assistant.getNOFBatteries() > 1)
                {
                    quickRelease();
                    return;
                }
            }
            if (color.Equals(Listener.WHITE))
            {
                if (assistant.getCARIndicator() == Assistant.UNKNOWN)
                {
                    talk.speakAsync("Is there a lit indicator labeled CAR?");
                    if (rec.getYesNo())
                    {
                        assistant.setCARIndicator(Assistant.TRUE);
                        holdStrip();
                        return;
                    }
                    assistant.setCARIndicator(Assistant.FALSE);
                }
                else if(assistant.getCARIndicator() == Assistant.TRUE)
                {
                    holdStrip();
                    return;
                }
            }
            if (assistant.getNOFBatteries() == Assistant.UNKNOWN)
            {
                talk.speakAsync("How many batteries?");
                assistant.setNOFBatteries(rec.getNumber());
            }
            if (assistant.getNOFBatteries() > 2)
            {
                if (assistant.getFRKIndicator() == Assistant.UNKNOWN)
                {
                    talk.speakAsync("Is there a lit indicator labeled FRK");
                    if (rec.getYesNo())
                    {
                        assistant.setFRKIndicator(Assistant.TRUE);
                        quickRelease();
                        return;
                    }
                    assistant.setFRKIndicator(Assistant.FALSE);
                }
                else if(assistant.getFRKIndicator() == Assistant.TRUE)
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

            while (color.Equals(Listener.UNRECOGNIZED))
            {
                talk.speakAsync("Repeat!");
                color = rec.getColor();
            }

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
