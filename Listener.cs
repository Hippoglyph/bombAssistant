using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace BombAssistant
{
    class Listener
    {
        public static String BLACK = "black";
        public static String RED = "red";
        public static String BLUE = "blue";
        public static String YELLOW = "yellow";
        public static String WHITE = "white";
        public static String ERROR = "error";
        public static String ABORT = "abort";
        public static String DETONATE = "detonate";
        public static String HOLD = "hold";
        public static String YES = "yes";
        public static String NO = "no";
        public static String TRUE = "true";
        public static String FALSE = "false";

        public static int SETSPEEDCOMMAND = 1;
        private static String SETSPEAKRATESTRING = "setspeakrate";

        public static int EXITCOMMAND = 2;
        private static String EXITSTRING = "exit";

        public static int BUTTONCOMMAND = 3;
        private static String BUTTONSTRING = "button";

        public static int WIRECOMMAND = 4;
        private static String WIRESTRING = "wires";

        SpeechRecognitionEngine rec;
        Assistant assistant;

        public Listener(Assistant lord)
        {
            assistant = lord;
            rec = new SpeechRecognitionEngine();
            rec.SetInputToDefaultAudioDevice();
        }

        public int listenForCommand()
        {
            setCommandGrammar();
            RecognitionResult input = rec.Recognize();
            if(input != null)
            {
                List<String> array = new List<string>();
                foreach (var word in input.Words)
                    array.Add(word.Text);
                assistant.setInput(array.ToArray());
                if (array[0].Equals(SETSPEAKRATESTRING))
                    return SETSPEEDCOMMAND;
                else if (array[0].Equals(EXITSTRING))
                    return EXITCOMMAND;
                else if (array[0].Equals(BUTTONSTRING))
                    return BUTTONCOMMAND;
                else if (array[0].Equals(WIRESTRING))
                    return WIRECOMMAND;
            }
            return 0;
        }

        /*
            command := <Set Speak Rate> | <button> | exit
        */
        private void setCommandGrammar()
        {
            rec.UnloadAllGrammars();

            Choices commands = new Choices();

            commands.Add(createSetSpeakRateGB());
            commands.Add(createExitGB());
            commands.Add(createButtonGB());
            commands.Add(createWiresGB());
            
            GrammarBuilder commandsGB = new GrammarBuilder(commands);
            commandsGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(commandsGB);
            gram.Name = "Commands";
            rec.LoadGrammar(gram);
        }

        /*
            wires := <color> | wires <color>
            color := red | blue | white | yellow | black
        */

        private GrammarBuilder createWiresGB()
        {
            Choices color = new Choices();
            color.Add(new string[] { RED, BLUE, WHITE, YELLOW, BLACK });

            GrammarBuilder gb = new GrammarBuilder(WIRESTRING);
            gb.Append(color,3,6);
            return gb;
        }

        /*
            button := button <color> <text>
            color := red | blue | white | yellow
            text := abort | detonate | hold
        */
        private GrammarBuilder createButtonGB()
        {
            Choices color = new Choices();
            color.Add(new string[] { RED, BLUE, WHITE, YELLOW });

            Choices text = new Choices();
            text.Add(new string[] { ABORT, DETONATE, HOLD });

            GrammarBuilder gb = new GrammarBuilder(BUTTONSTRING);
            gb.Append(color);
            gb.Append(text);
            return gb;
        }
        /*
            Set Speak Rate := setSpeakRateString <number>
            number := -10 | -9 | ... | 8 | 9 | 10
        */
        private GrammarBuilder createSetSpeakRateGB()
        {
            Choices numbers = new Choices();
            for (int i = -10; i <= 10; i++)
                numbers.Add(i.ToString());
            GrammarBuilder gb = new GrammarBuilder(SETSPEAKRATESTRING);
            gb.Append(numbers);
            return gb;
        }

        private GrammarBuilder createExitGB()
        {
            return new GrammarBuilder(EXITSTRING);
        }

        public String getColor()
        {
            setColorGrammar();

            RecognitionResult input = rec.Recognize();
            if(input != null)
            {
                return input.Text;
            }
            return ERROR;
        }

        /*
            color := red | blue | white | yellow
        */
        private void setColorGrammar()
        {
            rec.UnloadAllGrammars();
            Choices color = new Choices();
            color.Add(new string[] { RED, BLUE, WHITE, YELLOW });

            GrammarBuilder colorGB = new GrammarBuilder(color);
            colorGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(colorGB);
            gram.Name = "Color";
            rec.LoadGrammar(gram);
        }

        public int getNumber()
        {
            setNumberGrammar();

            RecognitionResult input = rec.Recognize();
            if (input != null)
            {
                return int.Parse(input.Text);
            }
            return 0;
        }

        /*
            number := 0..10 | none
        */
        private void setNumberGrammar()
        {
            rec.UnloadAllGrammars();
            Choices number = new Choices();
            for (int i = 0; i <= 10; i++)
                number.Add(i.ToString());
            number.Add("none");

            GrammarBuilder numberGB = new GrammarBuilder(number);
            numberGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(numberGB);
            gram.Name = "number";
            rec.LoadGrammar(gram);
        }

        public bool getYesNo()
        {
            setBoolGrammar();

            RecognitionResult input = rec.Recognize();
            if (input != null)
            {
                if (input.Text == YES || input.Text == TRUE)
                    return true;
            }
            return false;
        }
        /*
            bool := yes | no | true | false
        */
        private void setBoolGrammar()
        {
            rec.UnloadAllGrammars();
            Choices boolC = new Choices();
            boolC.Add(new string[] { YES, NO, TRUE, FALSE });

            GrammarBuilder boolGB = new GrammarBuilder(boolC);
            boolGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(boolGB);
            gram.Name = "bool";
            rec.LoadGrammar(gram);
        }
    }
}
