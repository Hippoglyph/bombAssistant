using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class WireSequencesModule
    {
        int redSequenceNumber;
        int blueSequenceNumber;
        int blackSequnceNumber;

        bool running;
        Speaker talk;
        Listener rec;
        Assistant assistant;
        public WireSequencesModule(Speaker talk, Listener rec, Assistant lord)
        {
            this.talk = talk;
            this.rec = rec;
            assistant = lord;
            redSequenceNumber = 0;
            blueSequenceNumber = 0;
            blackSequnceNumber = 0;
            running = true;
            solve();
        }

        private void solve()
        {
            talk.speakAsync("GO!");
            while (running)
            {
                string[] input = rec.getWireSequence();
                if(input.Contains(Listener.DONE) || input.Contains(Listener.EXIT))
                {
                    talk.speakAsync("Ready for new module!");
                    return;
                }
                play(input);
            }
        }

        private void play(string[] input)
        {
            if (input[0].Equals(Listener.RED))
            {
                playRed(input[1]);
            }
            else if (input[0].Equals(Listener.BLUE))
            {
                playBlue(input[1]);
            }
            else if (input[0].Equals(Listener.BLACK))
            {
                playBlack(input[1]);
            }
            else
            {
                talk.speakAsync("Fuck. This should never happend!");
                running = false;
            }
        }

        private void playRed(string militaryLetter)
        {
            char letter = getLetter(militaryLetter);
            
            switch (redSequenceNumber)
            {
                case 0:
                    cut(letter.Equals('c'));
                    break;
                case 1:
                    cut(letter.Equals('b'));
                    break;
                case 2:
                    cut(letter.Equals('a'));
                    break;
                case 3:
                    cut(letter.Equals('a') || letter.Equals('c'));
                    break;
                case 4:
                    cut(letter.Equals('b'));
                    break;
                case 5:
                    cut(letter.Equals('a') || letter.Equals('c'));
                    break;
                case 6:
                    cut(true);
                    break;
                case 7:
                    cut(letter.Equals('a') || letter.Equals('b'));
                    break;
                case 8:
                    cut(letter.Equals('b'));
                    break;
            }
            redSequenceNumber++;
        }

        private void playBlue(string militaryLetter)
        {
            char letter = getLetter(militaryLetter);

            switch (blueSequenceNumber)
            {
                case 0:
                    cut(letter.Equals('b'));
                    break;
                case 1:
                    cut(letter.Equals('a') || letter.Equals('c'));
                    break;
                case 2:
                    cut(letter.Equals('b'));
                    break;
                case 3:
                    cut(letter.Equals('a'));
                    break;
                case 4:
                    cut(letter.Equals('b'));
                    break;
                case 5:
                    cut(letter.Equals('b') || letter.Equals('c'));
                    break;
                case 6:
                    cut(letter.Equals('c'));
                    break;
                case 7:
                    cut(letter.Equals('a') || letter.Equals('c'));
                    break;
                case 8:
                    cut(letter.Equals('a'));
                    break;
            }
            blueSequenceNumber++;
        }

        private void playBlack(string militaryLetter)
        {
            char letter = getLetter(militaryLetter);

            switch (blackSequnceNumber)
            {
                case 0:
                    cut(true);
                    break;
                case 1:
                    cut(letter.Equals('a') || letter.Equals('c'));
                    break;
                case 2:
                    cut(letter.Equals('b'));
                    break;
                case 3:
                    cut(letter.Equals('a') || letter.Equals('c'));
                    break;
                case 4:
                    cut(letter.Equals('b'));
                    break;
                case 5:
                    cut(letter.Equals('b') || letter.Equals('c'));
                    break;
                case 6:
                    cut(letter.Equals('a') || letter.Equals('b'));
                    break;
                case 7:
                    cut(letter.Equals('c'));
                    break;
                case 8:
                    cut(letter.Equals('c'));
                    break;
            }
            blackSequnceNumber++;
        }

        private void cut(bool doCut)
        {
            if (doCut)
                talk.speakAsync("Cut!");
            else
                talk.speakAsync("Leave!");
        }

        private char getLetter(string militaryLetter)
        {
            string letter_ = assistant.getLetter(militaryLetter);
            if (letter_.Equals(Listener.ERROR))
            {
                talk.speakAsync("Error in Wire Sequence!");
                running = false;
                return 'z';
            }
            return letter_[0];
        }
    }
}
