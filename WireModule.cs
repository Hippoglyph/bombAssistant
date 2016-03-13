using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class WireModule
    {
        Speaker talk;
        Listener rec;
        String[] input;
        public WireModule(Speaker talk, Listener rec, String[] input)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            solve();
        }

        private void solve()
        {
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
            if (getNumberOfWireColor(Listener.BLUE) > 1)
            {
                talk.speakAsync("Cut the last blue wire!");
                return;
            }
            talk.speakAsync("Cut the last wire!");
        }

        private void wireFour()
        {
            int reds = getNumberOfWireColor(Listener.RED);
            if (reds > 1)
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                {
                    talk.speakAsync("Cut the last red wire!");
                    return;
                }
            }
            if (input.Last().Equals(Listener.YELLOW) && reds == 0)
            {
                talk.speakAsync("Cut the last wire!");
                return;
            }
            if (getNumberOfWireColor(Listener.BLUE) == 1)
            {
                talk.speakAsync("Cut the first wire!");
                return;
            }
            if (getNumberOfWireColor(Listener.YELLOW) > 1)
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
            if (getNumberOfWireColor(Listener.YELLOW) > 1 && getNumberOfWireColor(Listener.RED) == 1)
            {
                talk.speakAsync("Cut the first wire!");
                return;
            }
            if (getNumberOfWireColor(Listener.BLACK) == 0)
            {
                talk.speakAsync("Cut the second wire!");
                return;
            }
            talk.speakAsync("Cut the first wire!");
        }

        private void wireSix()
        {
            int yellows = getNumberOfWireColor(Listener.YELLOW);
            if (yellows == 0)
            {
                talk.speakAsync("Is the last digit of the serial number odd?");
                if (rec.getYesNo())
                {
                    talk.speakAsync("Cut the fourth wire!");
                    return;
                }
            }
            if (yellows == 1 && getNumberOfWireColor(Listener.WHITE) > 1)
            {
                talk.speakAsync("Cut the fourth wire!");
                return;
            }
            if (getNumberOfWireColor(Listener.RED) == 0)
            {
                talk.speakAsync("Cut the last wire!");
                return;
            }
            talk.speakAsync("Cut the fourth wire!");
        }
    }
}
