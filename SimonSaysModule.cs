using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class SimonSaysModule
    {
        Speaker talk;
        Listener rec;
        String[] input;
        Assistant assistant;
        public SimonSaysModule(Speaker talk, Listener rec, String[] input, Assistant lord)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            assistant = lord;
            solve();
        }

        private void solve()
        {

            if(assistant.getHasVowel() == Assistant.UNKNOWN)
            {
                talk.speakAsync("Does the serial number contain a vowel?");
                if (rec.getYesNo())
                    assistant.setHasVowel(Assistant.TRUE);
                else
                    assistant.setHasVowel(Assistant.FALSE);
            }
            play(createMatrix());
        }

        private void play(String[,,] matrix)
        {
            int[] colors = translateInput();
            StringBuilder sb = new StringBuilder();
            sb.Append("Simon Says ");
            foreach(int color in colors)
            {
                sb.Append(matrix[assistant.getHasVowel(), assistant.getStrikes(), color] + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            talk.speakAsync(sb.ToString());
        }

        private int[] translateInput()
        {
            List<int> colorList = new List<int>();
            foreach (String color in input)
            {
                if (color.Equals(Listener.RED))
                    colorList.Add(0);
                else if (color.Equals(Listener.BLUE))
                    colorList.Add(1);
                else if (color.Equals(Listener.GREEN))
                    colorList.Add(2);
                else if (color.Equals(Listener.YELLOW))
                    colorList.Add(3);
            }
            return colorList.ToArray();
        }

        private string[,,] createMatrix()
        {
            String[,,] matrix = new string[2, 3, 4]; //[Vowel, Strikes, Color]
            matrix[Assistant.FALSE, 0, 0] = Listener.BLUE;
            matrix[Assistant.FALSE, 0, 1] = Listener.YELLOW;
            matrix[Assistant.FALSE, 0, 2] = Listener.GREEN;
            matrix[Assistant.FALSE, 0, 3] = Listener.RED;

            matrix[Assistant.FALSE, 1, 0] = Listener.RED;
            matrix[Assistant.FALSE, 1, 1] = Listener.BLUE;
            matrix[Assistant.FALSE, 1, 2] = Listener.YELLOW;
            matrix[Assistant.FALSE, 1, 3] = Listener.GREEN;

            matrix[Assistant.FALSE, 2, 0] = Listener.YELLOW;
            matrix[Assistant.FALSE, 2, 1] = Listener.GREEN;
            matrix[Assistant.FALSE, 2, 2] = Listener.BLUE;
            matrix[Assistant.FALSE, 2, 3] = Listener.RED;

            matrix[Assistant.TRUE, 0, 0] = Listener.BLUE;
            matrix[Assistant.TRUE, 0, 1] = Listener.RED;
            matrix[Assistant.TRUE, 0, 2] = Listener.YELLOW;
            matrix[Assistant.TRUE, 0, 3] = Listener.GREEN;

            matrix[Assistant.TRUE, 1, 0] = Listener.YELLOW;
            matrix[Assistant.TRUE, 1, 1] = Listener.GREEN;
            matrix[Assistant.TRUE, 1, 2] = Listener.BLUE;
            matrix[Assistant.TRUE, 1, 3] = Listener.RED;

            matrix[Assistant.TRUE, 2, 0] = Listener.GREEN;
            matrix[Assistant.TRUE, 2, 1] = Listener.RED;
            matrix[Assistant.TRUE, 2, 2] = Listener.YELLOW;
            matrix[Assistant.TRUE, 2, 3] = Listener.BLUE;

            return matrix;
        }

    }
}
