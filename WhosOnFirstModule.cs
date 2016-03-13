using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class WhosOnFirstModule
    {
        Speaker talk;
        Listener rec;
        String[] input;
        public WhosOnFirstModule(Speaker talk, Listener rec, String[] input)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            solve();
        }

        private void solve()
        {
            Dictionary<String, String> labels = getLabels();
            String display = translateInput();
            if(!labels.ContainsKey(display))
            {
                talk.speakAsync("Word is not in my vocabulary. Please repeat!");
                return;
            }
            play(labels[display]);
        }

        private void play(string position)
        {
            bool running = true;
            while (running)
            {
                talk.speakAsync("What is the word on the " + position + "?");
                if (validate(rec.getWord()))
                    return;
                else
                    talk.speakAsync("Not a word");
            }
        }

        private bool validate(string response)
        {
            response = response.Replace(" ", "");
            if (response.Equals(Listener.EXIT))
            {
                talk.speakAsync("Ready for new command");
                return true;
            }
            Dictionary<String, String> labels = getLabelsStep2();
            if (labels.ContainsKey(response))
            {
                talk.speakAsync("Press first: " + labels[response]);
                return true;
            }
            return false;
        }

        private Dictionary<String, String> getLabelsStep2()
        {
            Dictionary<String, String> labels = new Dictionary<string, string>(28);
            labels.Add("ready", "YES, OKAY, WHAT, MIDDLE, LEFT, PRESS");
            labels.Add("first", "LEFT, OKAY, YES, MIDDLE, NO, RIGHT");
            labels.Add("no", "BLANK, UHHH, WAIT, FIRST, WHAT, READY");
            labels.Add("blank", "WAIT, RIGHT, OKAY, MIDDLE, BLANK, PRESS");
            labels.Add("nothing", "UHHH, RIGHT, OKAY, MIDDLE, YES, BLANK");
            labels.Add("yes", "OKAY, RIGHT, UHHH, MIDDLE, FIRST, WHAT");
            labels.Add("what", "UHHH, WHAT, LEFT, NOTHING, READY, BLANK");
            labels.Add("uhhh", "READY, NOTHING, LEFT, WHAT, OKAY, YES");
            labels.Add("left", "RIGHT, LEFT, FIRST, NO, MIDDLE, YES");
            labels.Add("right", "YES, NOTHING, READY, PRESS, NO, WAIT");
            labels.Add("middle", "BLANK, READY, OKAY, WHAT, NOTHING, PRESS");
            labels.Add("okay", "MIDDLE, NO, FIRST, YES, UHHH, NOTHING");
            labels.Add("wait", "UHHH, NO, BLANK, OKAY, YES, LEFT");
            labels.Add("press", "RIGHT, MIDDLE, YES, READY, PRESS, OKAY");
            labels.Add("you", "SURE, YOU ARE, YOUR, YOU'RE, NEXT, UH HUH");
            labels.Add("youare", "YOUR, NEXT, LIKE, UH HUH, WHAT?, DONE");
            labels.Add("your", "UH UH, YOU ARE, UH HUH, YOUR, NEXT, UR");
            labels.Add("youre", "YOU, YOU'RE, UR, NEXT, UH UH, YOU ARE");
            labels.Add("ur", "DONE, U, UR, UH HUH, WHAT?, SURE");
            labels.Add("u", "UH HUH, SURE, NEXT, WHAT?, YOU'RE, UR");
            labels.Add("uhhuh", "UH HUH, YOUR, YOU ARE, YOU, DONE, HOLD");
            labels.Add("uhuh", "UR, U, YOU ARE, YOU'RE, NEXT, UH UH");
            labels.Add("whatquestionmark", "YOU, HOLD, YOU'RE, YOUR, U, DONE");
            labels.Add("done", "SURE, UH HUH, NEXT, WHAT?, YOUR, UR");
            labels.Add("next", "WHAT?, UH HUH, UH UH, YOUR, HOLD, SURE");
            labels.Add("hold", "YOU ARE, U, DONE, UH UH, YOU, UR");
            labels.Add("sure", "YOU ARE, DONE, LIKE, YOU'RE, YOU, HOLD");
            labels.Add("like", "YOU'RE, NEXT, U, UR, HOLD, DONE");
            return labels;
        }

        private Dictionary<String, String> getLabels()
        {
            Dictionary<String, String> labels = new Dictionary<string, string>(28);
            labels.Add("yes", "left middle");
            labels.Add("first", "top right");
            labels.Add("display", "bottom right");
            labels.Add("okay", "top right");
            labels.Add("says", "bottom right");
            labels.Add("nothing", "middle left");

            labels.Add("empty", "bottom left");
            labels.Add("blank", "middle right");
            labels.Add("no", "bottom right");
            labels.Add("led", "middle left");
            labels.Add("lead", "bottom right");
            labels.Add("read", "middle right");

            labels.Add("red", "middle right");
            labels.Add("reed", "bottom left");
            labels.Add("leed", "bottom left");
            labels.Add("holdon", "bottom right");
            labels.Add("you", "middle right");
            labels.Add("youare", "bottom right");

            labels.Add("your", "middle right");
            labels.Add("youre", "middle right");
            labels.Add("ur", "top left");
            labels.Add("there", "bottom right");
            labels.Add("theyre", "bottom left");
            labels.Add("their", "middle right");

            labels.Add("theyare", "middle left");
            labels.Add("see", "bottom right");
            labels.Add("c", "top right");
            labels.Add("cee", "bottom right");

            return labels;
        }

        private String translateInput()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 1; i < input.Length; i++)
            {
                sb.Append(input[i]);
            }
            return sb.ToString();
        }
    }
}
