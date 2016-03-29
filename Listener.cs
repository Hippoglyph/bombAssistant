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
        public static String EMPTY = "empty";
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
        public static String NEXT = "next";
        public static String DONE = "done";


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

        public static int PASSWORDCOMMAND = 12;
        private static String PASSWORDSTRING = "passwords";

        public static int WIRESEQUENCESCOMMAND = 13;
        private static String WIRESEQUENCESSTRING = "wiresequences";

        public static int COMPLICATEDWIRESCOMMAND = 14;
        private static String COMPLICATEDWIRESSTRING = "complicatedwires";

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
                else if (list[0].Equals(PASSWORDSTRING))
                    return PASSWORDCOMMAND;
                else if (list[0].Equals(WIRESEQUENCESSTRING))
                    return WIRESEQUENCESCOMMAND;
                else if (list[0].Equals(COMPLICATEDWIRESSTRING))
                    return COMPLICATEDWIRESCOMMAND;
            }
            return 0;
        }

        /*
            command := <Set Speak Rate> | <button> | exit | <wires> | <keypads> | <Set Strikes> | <Simon Says> | <Whos on First> | <memory> | <reset>
                        <password> | <complicatedWires>
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
            commands.Add(createPasswordGB());
            commands.Add(createWireSequencesGB());
            commands.Add(createComplicatedWiresGB());
            
            GrammarBuilder commandsGB = new GrammarBuilder(commands);
            commandsGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(commandsGB);
            gram.Name = "Commands";
            rec.LoadGrammar(gram);
        }

        /*
            complicatedWires := complicatedwires <wires>
            wires := <wire> <wires> | <wire>
            wire := <led> <colors> <star> next | <led> <colors> <star> done
            led := led | ""
            star := star | ""
            colors := <color> <color> | <color>
            color := red | blue | white | yellow | black | green
        */
        private GrammarBuilder createComplicatedWiresGB()
        {
            Choices color = getColorChoices();
            Choices next = new Choices(new string[] { NEXT, DONE});
            GrammarBuilder wire = new GrammarBuilder();
            Choices wire_ = new Choices(new string[] { ComplicatedWiresModule.LED, ComplicatedWiresModule.STAR });
            wire_.Add(color);
            wire.Append(wire_, 1, 4);
            wire.Append(next);
            GrammarBuilder gb = new GrammarBuilder(COMPLICATEDWIRESSTRING);
            gb.Append(wire, 1, 16);
            return gb;
        }

        /*
            WireSeqences := wiresequences
        */
        private GrammarBuilder createWireSequencesGB()
        {
            return new GrammarBuilder(WIRESEQUENCESSTRING);
        }

        /*
            password := password <letters>
            letters := <letters> letter | letter
            letter := alfa | bravo | charlie | delta | echo | foxtrot | golf | hotel | india | juliett | kilo | lima | mike | november | oscar |
                        papa | quebec | romeo | sierra | tango | uniform | victor | whiskey | xray | yankee | zulu
        */

        private GrammarBuilder createPasswordGB()
        {
            Choices militaryLetters = getMilitaryLetterChoises();
            GrammarBuilder gb = new GrammarBuilder(PASSWORDSTRING);
            gb.Append(militaryLetters, 1, 10);
            return gb;
        }

        /*
            mazes := mazes
        */
        private GrammarBuilder createMazesGB()
        {
            return new GrammarBuilder(MAZESSTRING);
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
            Choices letters = getMilitaryLetterChoises();
            letters.Add(EMPTY);
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
            text := abort | detonate | hold | press
        */
        private GrammarBuilder createButtonGB()
        {
            Choices color = getColorChoices();

            Choices text = new Choices();
            text.Add(new string[] { ButtonModule.ABORT, ButtonModule.DETONATE, ButtonModule.HOLD, ButtonModule.PRESS });

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

        public String[] getWireSequence()
        {
            setWireSequenceGrammar();

            RecognitionResult input = rec.Recognize();
            if (input != null)
            {
                assistant.setInput(new string[] { input.Text });
                List<String> list = new List<string>();
                foreach (var word in input.Words)
                    list.Add(word.Text);
                return list.ToArray();
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
            return new string[] { UNRECOGNIZED };
        }

        /*
            wiresequence := done | exit | <color> <letter>
            color := red | blue | black
            letter := alfa | bravo | charlie
        */
        private void setWireSequenceGrammar()
        {
            rec.UnloadAllGrammars();
            Choices sequence = new Choices(new string[] { DONE, EXIT});
            Choices letters = new Choices(new string[] { "alfa", "bravo", "charlie" });
            Choices wires = new Choices(new string[] { RED, BLUE, BLACK });
            GrammarBuilder sequenceGB_ = new GrammarBuilder();
            sequenceGB_.Append(wires);
            sequenceGB_.Append(letters);
            sequenceGB_.Culture = new System.Globalization.CultureInfo("en-GB");
            sequence.Add(sequenceGB_);

            GrammarBuilder sequenceGB = new GrammarBuilder(sequence);
            sequenceGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(sequenceGB);
            gram.Name = "Sequence";
            rec.LoadGrammar(gram);
        }

        public String[] getLetterSequence()
        {
            setMilitaryLetterGrammar();

            RecognitionResult input = rec.Recognize();
            if (input != null)
            {
                List<string> letter = new List<string>();
                foreach (var letter_ in input.Words)
                    letter.Add(letter_.Text);
                assistant.setInput( letter.ToArray() );
                return letter.ToArray();
            }
            assistant.setInput(new string[] { UNRECOGNIZED });
            return new string[] { UNRECOGNIZED };
        }

        /*
            letters := <letters> letter | letter
            letter := alfa | bravo | charlie | delta | echo | foxtrot | golf | hotel | india | juliett | kilo | lima | mike | november | oscar |
                        papa | quebec | romeo | sierra | tango | uniform | victor | whiskey | xray | yankee | zulu | exit | questionmark | empty
        */
        private void setMilitaryLetterGrammar()
        {
            rec.UnloadAllGrammars();
            Choices letter = getMilitaryLetterChoises();
            letter.Add(EXIT);
            letter.Add("questionmark");
            letter.Add(EMPTY);

            GrammarBuilder letterGB = new GrammarBuilder(letter, 1 , 10);
            letterGB.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar gram = new Grammar(letterGB);
            gram.Name = "Military";
            rec.LoadGrammar(gram);
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
            return -1;
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
            coord[0] = -1;
            coord[1] = -1;
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

        /*
            coord := <number> <number>
            number := 0...7
        */
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

        private Choices getMilitaryLetterChoises()
        {
            Choices letter = new Choices();
            foreach(String s in assistant.getAllLetters())
            {
                letter.Add(s);
            }
            return letter;
        }
    }
}
