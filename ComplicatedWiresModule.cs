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
            setLastDigitOdd();
            setParallelPort();
            Wire[] wire = getParsedInput();
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
                            wire.color = new string[] { wire.color[0], input[i] };
                    }
                    else
                        break;
                    i++;
                }
                wireList.Add(wire);
            }

            return wireList.ToArray();
        }

        private void setLastDigitOdd()
        {
            if(assistant.getLastDigitOdd() == Assistant.UNKNOWN)
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                {
                    assistant.setInput(new string[] { "yes" });
                    assistant.setLastDigitOdd(Assistant.TRUE);
                }
                else {
                    assistant.setInput(new string[] { "no" });
                    assistant.setLastDigitOdd(Assistant.FALSE);
                }
            }
        }

        private void setParallelPort()
        {
            if (assistant.getParallelPort() == Assistant.UNKNOWN)
            {
                talk.speakAsync("Does the bomb have a parallel port?");
                if (rec.getYesNo())
                {
                    assistant.setInput(new string[] { "yes" });
                    assistant.setParallelPort(Assistant.TRUE);
                }
                else {
                    assistant.setInput(new string[] { "no" });
                    assistant.setParallelPort(Assistant.FALSE);
                }
            }
        }
    }
}
