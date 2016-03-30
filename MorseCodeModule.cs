using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class MorseCodeModule
    {
        public static string SHORT = "short";
        public static string LONG = "long";

        Speaker talk;
        Listener rec;
        String[] input;
        Assistant assistant;
        int index;
        bool running;
        bool repeat;
        public MorseCodeModule(Speaker talk, Listener rec, String[] input, Assistant lord)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            assistant = lord;
            index = 0;
            running = true;
            repeat = false;
            solve();
        }

        private void solve()
        {
            List<string> options = getOptionsFromInput();
            check(options);
            play(options);
        }

        private void play(List<string> options)
        {
            while (running)
            {
                //debugPrint(options);
                talk.speakAsync("Next letter!");
                string[] answer = rec.getMorseLetter();
                while (answer.Contains(Listener.UNRECOGNIZED))
                {
                    talk.speakAsync("Repeat!");
                    answer = rec.getMorseLetter();
                }
                char letter = translateLetter(answer);
                while (letter.Equals('ö'))
                {
                    if (answer.Contains(Listener.EXIT))
                    {
                        talk.speakAsync("Ready for new module!");
                        return;
                    }
                    talk.speakAsync("Impossible letter. Please repeat!");
                    answer = rec.getMorseLetter();
                    while (answer.Contains(Listener.UNRECOGNIZED))
                    {
                        talk.speakAsync("Repeat!");
                        answer = rec.getMorseLetter();
                    }
                    letter = translateLetter(answer);
                }
                options = removeOption(options, index, letter);
                index++;
                check(options);
            }
        }

        private void check(List<string> options)
        {
            if (options.Count == 1)
            {
                talk.speakAsync("The frequency is: " + getFrequency(options.First()));
                running = false;
            }
            else if (options.Count < 1)
            {
                talk.speakAsync("Word does not exists. Please repeat module!");
                running = false;
            }
        }

        private List<string> getOptionsFromInput()
        {
            List<string> options = getWords();
            string input_ = translateInput();
            foreach(char letter in input_)
            {
                options = removeOption(options, index, letter);
                index++;
            }
            return options;
        }

        private List<string> removeOption(List<string> options, int index, char letter)
        {
            List<string> newOptions = new List<string>();
            foreach(string option in options)
            {
                if (validate(option, index, letter))
                    newOptions.Add(option);
            }
            return newOptions;
        }

        private bool validate(string option, int index, char letter)
        {
            if (option.Length <= index)
                return false;
            if (option[index].Equals(letter))
                return true;
            return false;
        }

        private string translateInput()
        {
            StringBuilder sb = new StringBuilder();
            List<string> letter = new List<string>();
            for(int i = 1; i < input.Length; i++)
            {
                if (input[i].Equals(Listener.NEXT)) {
                    sb.Append(translateLetter(letter.ToArray()));
                    letter.Clear();
                }
                else
                    letter.Add(input[i]);
            }
            return sb.ToString();
        }

        private char translateLetter(string[] input)
        {
            int len = input.Length;
            if (len == 2 && input[0].Equals(SHORT) && input[1].Equals(LONG))
                return 'a';
            else if (len == 4 && input[0].Equals(LONG) && input[1].Equals(SHORT) && input[2].Equals(SHORT) && input[3].Equals(SHORT))
                return 'b';
            else if (len == 4 && input[0].Equals(LONG) && input[1].Equals(SHORT) && input[2].Equals(LONG) && input[3].Equals(SHORT))
                return 'c';
            else if (len == 3 && input[0].Equals(LONG) && input[1].Equals(SHORT) && input[2].Equals(SHORT))
                return 'd';
            else if (len == 1 && input[0].Equals(SHORT))
                return 'e';
            else if (len == 4 && input[0].Equals(SHORT) && input[1].Equals(SHORT) && input[2].Equals(LONG) && input[3].Equals(SHORT))
                return 'f';
            else if (len == 3 && input[0].Equals(LONG) && input[1].Equals(LONG) && input[2].Equals(SHORT))
                return 'g';
            else if (len == 4 && input[0].Equals(SHORT) && input[1].Equals(SHORT) && input[2].Equals(SHORT) && input[3].Equals(SHORT))
                return 'h';
            else if (len == 2 && input[0].Equals(SHORT) && input[1].Equals(SHORT))
                return 'i';
            else if (len == 4 && input[0].Equals(SHORT) && input[1].Equals(LONG) && input[2].Equals(LONG) && input[3].Equals(LONG))
                return 'j';
            else if (len == 3 && input[0].Equals(LONG) && input[1].Equals(SHORT) && input[2].Equals(LONG))
                return 'k';
            else if (len == 4 && input[0].Equals(SHORT) && input[1].Equals(LONG) && input[2].Equals(SHORT) && input[3].Equals(SHORT))
                return 'l';
            else if (len == 2 && input[0].Equals(LONG) && input[1].Equals(LONG))
                return 'm';
            else if (len == 2 && input[0].Equals(LONG) && input[1].Equals(SHORT))
                return 'n';
            else if (len == 3 && input[0].Equals(LONG) && input[1].Equals(LONG) && input[2].Equals(LONG))
                return 'o';
            else if (len == 4 && input[0].Equals(SHORT) && input[1].Equals(LONG) && input[2].Equals(LONG) && input[3].Equals(SHORT))
                return 'p';
            else if (len == 4 && input[0].Equals(LONG) && input[1].Equals(LONG) && input[2].Equals(SHORT) && input[3].Equals(LONG))
                return 'q';
            else if (len == 3 && input[0].Equals(SHORT) && input[1].Equals(LONG) && input[2].Equals(SHORT))
                return 'r';
            else if (len == 3 && input[0].Equals(SHORT) && input[1].Equals(SHORT) && input[2].Equals(SHORT))
                return 's';
            else if (len == 1 && input[0].Equals(LONG))
                return 't';
            else if (len == 3 && input[0].Equals(SHORT) && input[1].Equals(SHORT) && input[2].Equals(LONG))
                return 'u';
            else if (len == 4 && input[0].Equals(SHORT) && input[1].Equals(SHORT) && input[2].Equals(SHORT) && input[3].Equals(LONG))
                return 'v';
            else if (len == 3 && input[0].Equals(SHORT) && input[1].Equals(LONG) && input[2].Equals(LONG))
                return 'w';
            else if (len == 4 && input[0].Equals(LONG) && input[1].Equals(SHORT) && input[2].Equals(SHORT) && input[3].Equals(LONG))
                return 'x';
            else if (len == 4 && input[0].Equals(LONG) && input[1].Equals(SHORT) && input[2].Equals(LONG) && input[3].Equals(LONG))
                return 'y';
            else if (len == 4 && input[0].Equals(LONG) && input[1].Equals(LONG) && input[2].Equals(SHORT) && input[3].Equals(SHORT))
                return 'z';
            return 'ö';
        }

        private List<string> getWords()
        {
            List<string> codes = new List<string>();
            codes.Add("shell");
            codes.Add("halls");
            codes.Add("slick");
            codes.Add("trick");
            codes.Add("boxes");
            codes.Add("leaks");
            codes.Add("strobe");
            codes.Add("bistro");
            codes.Add("flick");
            codes.Add("bombs");
            codes.Add("break");
            codes.Add("brick");
            codes.Add("steak");
            codes.Add("sting");
            codes.Add("vector");
            codes.Add("beats");
            return codes;
        }

        private string getFrequency(string key)
        {
            if (key.Equals("shell"))
                return "3.505";
            else if (key.Equals("halls"))
                return "3.515";
            else if (key.Equals("slick"))
                return "3.522";
            else if (key.Equals("trick"))
                return "3.532";
            else if (key.Equals("boxes"))
                return "3.535";
            else if (key.Equals("leaks"))
                return "3.542";
            else if (key.Equals("strobe"))
                return "3.545";
            else if (key.Equals("bistro"))
                return "3.552";
            else if (key.Equals("flick"))
                return "3.555";
            else if (key.Equals("bombs"))
                return "3.565";
            else if (key.Equals("break"))
                return "3.572";
            else if (key.Equals("brick"))
                return "3.575";
            else if (key.Equals("steak"))
                return "3.582";
            else if (key.Equals("sting"))
                return "3.592";
            else if (key.Equals("vector"))
                return "3.595";
            else if (key.Equals("beats"))
                return "3.600";
            else
                return "FUCK";
        }

        private void debugPrint(List<string> options)
        {
            foreach(string s in options)
            {
                Console.WriteLine(s);
            }
        }
    }
}
