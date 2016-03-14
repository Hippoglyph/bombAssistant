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
        public static String UNRECOGNIZED = "UNRECOGNIZED";
        public static String GREEN = "green";
        public static String BLACK = "black";
        public static String RED = "red";
        public static String BLUE = "blue";
        public static String YELLOW = "yellow";
        public static String WHITE = "white";
        public static String YES = "yes";
        public static String NO = "no";
        public static String TRUE = "true";
        public static String FALSE = "false";
        public static String ERROR = "error";
        public static String EXIT = "exit";
        public static String WIN = "we did it";
        public static String LOOSE = "we exploded";


        public static int SETSPEEDCOMMAND = 1;
        private static String SETSPEAKRATESTRING = "setspeakrate";

        public static int EXITCOMMAND = 2;
        private static String EXITSTRING = "exit";

        public static int BUTTONCOMMAND = 3;
        private static String BUTTONSTRING = "button";

        public static int WIRECOMMAND = 4;
        private static String WIRESTRING = "wires";

        public static int KEYPADCOMMAND = 5;
        private static String KEYPADSTRING = "keypads";

        public static int SETSTRIKESCOMMAND = 6;
        private static String SETSTRIKESSTRING = "setstrikes";

        public static int SIMONSAYSCOMMAND = 7;
        private static String SIMONSAYSSTRING = "simonsays";

        public static int WHOSONFIRSTCOMMAND = 8;
        private static String WHOSONFIRSTSTRING = "whosonfirst";

        public static int MEMORYCOMMAND = 9;
        private static String MEMORYSTRING = "memory";

        public static int RESETCOMMAND = 10;
        private static String RESETSTRING = "we";

        public static int MAZESCOMMAND = 11;
        private static String MAZESSTRING = "mazes";

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
                List<String> list = new List<string>();
                foreach (var word in input.Words)
                    list.Add(word.Text);
                assistant.setInput(list.ToArray());
                if (list[0].Equals(SETSPEAKRATESTRING))
                    return SETSPEEDCOMMAND;
                else if (list[0].Equals(EXITSTRING))
                    return EXITCOMMAND;
                else if (list[0].Equals(BUTTONSTRING))
                    return BUTTONCOMMAND;
                else if (list[0].Equals(WIRESTRING))
                    return WIRECOMMAND;
                else if (list[0].Equals(KEYPADSTRING))
                    return KEYPADCOMMAND;
                else if (list[0].Equals(SETSTRIKESSTRING))
                    return SETSTRIKESCOMMAND;
                else if (list[0].Equals(SIMONSAYSSTRING))
                    return SIMONSAYSCOMMAND;
                else if (list[0].Equals(WHOSONFIRSTSTRING))
                    return WHOSONFIRSTCOMMAND;
                else if (list[0].Equals(MEMORYSTRING))
                    return MEMORYCOMMAND;
                else if (list[0].Equals(RESETSTRING))
                    return RESETCOMMAND;
                else if (list[0].Equals(MAZESSTRING))
                    return MAZESCOMMAND;
            }
            return 0;
        }

        /*
            command := <Set Speak Rate> | <button> | exit | <wires> | <keypads> | <Set Strikes> | <Simon Says> | <Whos on First> | <memory> | <reset>
        */
        private void setCommandGrammar()
        {
            rec.UnloadAllGrammars();

            Choices commands = new Choices();

            commands.Add(createSetSpeakRateGB());
            commands.Add(createExitGB());
            commands.Add(createButtonGB());
            commands.Add(createWiresGB());
            commands.Add(createKeypadGB());
            commands.Add(createSetStrikeGB());
            commands.Add(createSimonSaysGB());
            commands.Add(createWhosOnFirstGB());
            commands.Add(createMemoryGB());
            commands.Add(createResetGB());
            commands.Add(createMazesGB());
            
            GrammarBuilder commandsGB = new GrammarBuilder(commands);
            commandsGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(commandsGB);
            gram.Name = "Commands";
            rec.LoadGrammar(gram);
        }

        private GrammarBuilder createMazesGB()
        {
            GrammarBuilder gb = new GrammarBuilder(MAZESSTRING);
            return gb;
        }

        /*
            reset := we did it | we exploded
        */
        private GrammarBuilder createResetGB()
        {
            Choices choise = new Choices();
            choise.Add(WIN);
            choise.Add(LOOSE);
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choise);
            return gb;
        }

        /*
            memory := memory <number>
            number := 1 | 2 | 3 | 4
        */
        private GrammarBuilder createMemoryGB()
        {
            Choices numbers = new Choices();
            for (int i = 1; i < 5; i++)
                numbers.Add(i.ToString());
            GrammarBuilder gb = new GrammarBuilder(MEMORYSTRING);
            gb.Append(numbers);
            return gb;
        }
        /*
            whosonfirst := whosonfirst <letters> | whosonfirst empty
            letters := <letters> letter | letter
        */
        private GrammarBuilder createWhosOnFirstGB()
        {
            Choices letters = getLetterChoises();
            GrammarBuilder gb = new GrammarBuilder(WHOSONFIRSTSTRING);
            gb.Append(letters, 1, 16);
            return gb;
        }

        /*
            simonsays := simonsays <colors>
            colors := <colors> <color> | <color>
            color := red | blue | white | yellow | black | green
        */

        private GrammarBuilder createSimonSaysGB()
        {
            Choices colors = getColorChoices();
            GrammarBuilder gb = new GrammarBuilder(SIMONSAYSSTRING);
            gb.Append(colors, 1, 4);
            return gb;
        }

        /*
            setstrikes := setstrikes <number>
            number := 0 | 1 | 2
        */
        private GrammarBuilder createSetStrikeGB()
        {
            Choices strikes = new Choices();
            strikes.Add(new string[] { 0.ToString(), 1.ToString(), 2.ToString() });
            GrammarBuilder gb = new GrammarBuilder(SETSTRIKESSTRING);
            gb.Append(strikes);
            return gb;
        }

        /*
            keypads := keypads <symbol> <symbol> <symbol> <symbol>
            symbol := a | norwegian | b | blackstar | c | cat | cross | euro | h | halfthree | lambda | lightning | moon | n | nose | omega | pharagraph |
                      psi | q | questionmark | six | smiley | snake | stitches | three | trademark | whitestar
        */

        private GrammarBuilder createKeypadGB()
        {
            Choices symbols = new Choices();
            symbols.Add(new string[] { KeypadModule.A, KeypadModule.AE, KeypadModule.B, KeypadModule.BLACKSTAR, KeypadModule.C,
            KeypadModule.CAT, KeypadModule.CROSS, KeypadModule.EURO, KeypadModule.H, KeypadModule.HALFTHREE, KeypadModule.LAMBDA,
            KeypadModule.LIGHTNING, KeypadModule.MOON, KeypadModule.N, KeypadModule.NOSE, KeypadModule.OMEGA, KeypadModule.PHARAGRAPH,
            KeypadModule.PSI, KeypadModule.Q, KeypadModule.QUESTIONMARK, KeypadModule.SIX, KeypadModule.SMILEY, KeypadModule.SNAKE,
            KeypadModule.STITCHES, KeypadModule.THREE, KeypadModule.TRADEMARK, KeypadModule.WHITESTAR});

            GrammarBuilder gb = new GrammarBuilder(KEYPADSTRING);
            gb.Append(symbols, 4,4);
            return gb;
        }

        /*
            wires := wires <colors>
            colors := <colors> <color> | <color>
            color := red | blue | white | yellow | black
        */

        private GrammarBuilder createWiresGB()
        {
            Choices color = getColorChoices();

            GrammarBuilder gb = new GrammarBuilder(WIRESTRING);
            gb.Append(color,3,6);
            return gb;
        }

        /*
            button := button <color> <text>
            color := red | blue | white | yellow | black | green
            text := abort | detonate | hold
        */
        private GrammarBuilder createButtonGB()
        {
            Choices color = getColorChoices();

            Choices text = new Choices();
            text.Add(new string[] { ButtonModule.ABORT, ButtonModule.DETONATE, ButtonModule.HOLD });

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
                assistant.setInput(new string[] { input.Text });
                return input.Text;
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
            return ERROR;
        }

        /*
            color := red | blue | white | yellow | black | green
        */
        private void setColorGrammar()
        {
            rec.UnloadAllGrammars();
            Choices color = getColorChoices();

            GrammarBuilder colorGB = new GrammarBuilder(color);
            colorGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(colorGB);
            gram.Name = "Color";
            rec.LoadGrammar(gram);
        }

        public String getWord()
        {
            setWordGrammar();

            RecognitionResult input = rec.Recognize();
            if(input != null)
            {
                assistant.setInput(new string[] { input.Text });
                return input.Text;
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
            return ERROR;
        }

        private void setWordGrammar()
        {
            rec.UnloadAllGrammars();
            Choices letters = getLetterChoises();
            letters.Add(EXIT);
            letters.Add("questionmark");
            GrammarBuilder wordGB = new GrammarBuilder(letters,1,16);
            wordGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(wordGB);
            gram.Name = "Word";
            rec.LoadGrammar(gram);
        }

        public int getNumber()
        {
            setNumberGrammar();

            RecognitionResult input = rec.Recognize();
            if (input != null)
            {
                assistant.setInput(new string[] { input.Text });
                return int.Parse(input.Text);
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
            return 0;
        }

        /*
            number := 0..10
        */
        private void setNumberGrammar()
        {
            rec.UnloadAllGrammars();
            Choices number = new Choices();
            for (int i = 0; i <= 10; i++)
                number.Add(i.ToString());

            GrammarBuilder numberGB = new GrammarBuilder(number);
            numberGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(numberGB);
            gram.Name = "number";
            rec.LoadGrammar(gram);
        }

        public int[] getCoords()
        {
            setCoordGrammar();

            int[] coord = new int[2];
            coord[0] = 0;
            coord[1] = 1;
            RecognitionResult input = rec.Recognize();
            if(input != null)
            {
                assistant.setInput(new string[] { input.Text });
                List<String> list = new List<string>();
                foreach (var word in input.Words)
                    list.Add(word.Text);
                coord[0] = int.Parse(list[0]);
                coord[1] = int.Parse(list[1]);
                return coord;
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
            return coord;
        }

        private void setCoordGrammar()
        {
            rec.UnloadAllGrammars();
            Choices number = new Choices();
            for (int i = 0; i < 7; i++)
                number.Add(i.ToString());
            GrammarBuilder coordGB = new GrammarBuilder(number, 2, 2);
            coordGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(coordGB);
            gram.Name = "coord";
            rec.LoadGrammar(gram);
        }

        public bool getYesNo()
        {
            setBoolGrammar();

            RecognitionResult input = rec.Recognize();
            if (input != null)
            {
                assistant.setInput(new string[] { input.Text });
                if (input.Text == YES || input.Text == TRUE)
                    return true;
                return false;
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
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

        private Choices getColorChoices()
        {
            return new Choices(new string[] { RED, BLUE, WHITE, YELLOW, BLACK, GREEN });
        }

        private Choices getLetterChoises()
        {
            return new Choices(new string[] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p",
                                        "a", "s", "d", "f", "g", "h", "j", "k", "l",
                                        "z", "x", "c", "v", "b", "n", "m", "empty"});
        }
    }
}
