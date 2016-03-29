using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class ComplicatedWiresModule
    {

        struct Wire
        {
            public bool led;
            public bool star;
            public string[] color;
        }

        public static string STAR = "star";
        public static string LED = "led";
        string[] colors;

        Speaker talk;
        Listener rec;
        Assistant assistant;
        string[] input;
        public ComplicatedWiresModule(Speaker talk, Listener rec, string[] input, Assistant lord)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            assistant = lord;
            colors = new string[] { Listener.BLACK, Listener.BLUE, Listener.GREEN, Listener.RED, Listener.WHITE, Listener.YELLOW };
            solve();
        }

        private void solve()
        {
            setBatteries();
            setLastDigitOdd();
            setParallelPort();
            Wire[] wires = getParsedInput();
            play(wires);
        }

        private void play(Wire[] wires)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Follow sequence: ");
            foreach(Wire wire in wires)
            {
                if (cut(wire))
                    sb.Append("cut, ");
                else
                    sb.Append("leave, ");
            }
            sb.Remove(sb.Length - 2, 2);
            talk.speakAsync(sb.ToString());
        }

        private bool cut(Wire wire)
        {
            bool led = wire.led;
            bool star = wire.star;
            string[] color = wire.color;
            if (color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && star && led)
                return false;
            else if (color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && star && !led)
                return ifPP();
            else if (color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && !star && led)
                return ifSE();
            else if (color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && !star && !led)
                return ifSE();
            else if (color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && star && led)
                return ifB();
            else if (color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && star && !led)
                return true;
            else if (color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && !star && led)
                return ifB();
            else if (color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && !star && !led)
                return ifSE();
            else if (!color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && star && led)
                return ifPP();
            else if (!color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && star && !led)
                return false;
            else if (!color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && !star && led)
                return ifPP();
            else if (!color.Contains(Listener.RED) && color.Contains(Listener.BLUE) && !star && !led)
                return ifSE();
            else if (!color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && star && led)
                return ifB();
            else if (!color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && star && !led)
                return true;
            else if (!color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && !star && led)
                return false;
            else if (!color.Contains(Listener.RED) && !color.Contains(Listener.BLUE) && !star && !led)
                return true;
            talk.speakAsync("Fuck this should not happend! Just cut it");
            return true;
        }

        private bool ifB()
        {
            if (assistant.getNOFBatteries() >= 2)
                return true;
            else
                return false;
        }

        private bool ifSE()
        {
            if (assistant.getLastDigitOdd() == Assistant.TRUE)
                return false;
            else
                return true;
        }

        private bool ifPP()
        {
            if (assistant.getParallelPort() == Assistant.TRUE)
                return true;
            else
                return false;
        }

        private Wire[] getParsedInput()
        {
            List<Wire> wireList = new List<Wire>();
            for(int i = 1; i < input.Length; i++)
            {
                Wire wire = new Wire();
                while(i < input.Length)
                {
                    if (input[i].Equals(LED))
                        wire.led = true;
                    else if (input[i].Equals(STAR))
                        wire.star = true;
                    else if (colors.Contains(input[i]))
                    {
                        if (wire.color == null)
                            wire.color = new string[] { input[i] };
                        else
                        {
                            List<string> colorList = wire.color.ToList();
                            colorList.Add(input[i]);
                            wire.color = colorList.ToArray();
                            //wire.color = new string[] { wire.color[0], input[i] };
                        }
                    }
                    else
                        break;
                    i++;
                }
                wireList.Add(wire);
            }

            return wireList.ToArray();
        }

        private void setBatteries()
        {
            while (assistant.getNOFBatteries() == Assistant.UNKNOWN)
            {
                talk.speakAsync("How many batteries?");
                assistant.setNOFBatteries(rec.getNumber());
            }
        }

        private void setLastDigitOdd()
        {
            if(assistant.getLastDigitOdd() == Assistant.UNKNOWN)
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                    assistant.setLastDigitOdd(Assistant.TRUE);
                else
                    assistant.setLastDigitOdd(Assistant.FALSE);
            }
        }

        private void setParallelPort()
        {
            if (assistant.getParallelPort() == Assistant.UNKNOWN)
            {
                talk.speakAsync("Does the bomb have a parallel port?");
                if (rec.getYesNo())
                    assistant.setParallelPort(Assistant.TRUE);
                else
                    assistant.setParallelPort(Assistant.FALSE);
            }
        }
    }
}
