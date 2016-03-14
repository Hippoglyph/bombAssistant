using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class MemoryModule
    {
        int firstDiplayNumber = 0;
        int firstLabel = 0;
        int firstPosition = 0;

        int secondDisplayNumber = 0;
        int secondLabel = 0;
        int secondPosition = 0;

        int thirdDisplayNumber = 0;
        int thirdLabel = 0;
        int thirdPosition = 0;

        int fourthDisplayNumber = 0;
        int fourthLabel = 0;
        int fourthPosition = 0;

        String exit;
        String standard;
        String position;
        String label;
        String display;
        String repeat;
        bool askedForLabel = false;
        bool running;

        Speaker talk;
        Listener rec;
        String[] input;
        public MemoryModule(Speaker talk, Listener rec, String[] input)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            running = true;
            standard = "Press the button ";
            position = " and say the position";
            label = " and say the label";
            exit = "Number not between 1-4. Ready for new module";
            display = "What is the number on the display?";
            repeat = "Repeat!";
            solve();
        }

        public void solve()
        {
            firstDiplayNumber = int.Parse(input.Last());
            if (!step1())
                return;
            firstLabel = getNumber();
            if (!running)
            {
                talk.speakAsync(exit);
                return;
            }
                

            talk.speakAsync(display);
            secondDisplayNumber = getNumber();
            if (!step2())
                return;
            if (askedForLabel)
                secondLabel = getNumber();
            else
                secondPosition = getNumber();
            if (!running)
            {
                talk.speakAsync(exit);
                return;
            }

            talk.speakAsync(display);
            thirdDisplayNumber = getNumber();
            if (!step3())
                return;
            if (askedForLabel)
                thirdLabel = getNumber();
            else
                thirdPosition = getNumber();
            if (!running)
            {
                talk.speakAsync(exit);
                return;
            }

            talk.speakAsync(display);
            fourthDisplayNumber = getNumber();
            if (!step4())
                return;
            if (askedForLabel)
                fourthLabel = getNumber();
            else
                fourthPosition = getNumber();
            if (!running)
            {
                talk.speakAsync(exit);
                return;
            }

            talk.speakAsync(display);
            step5(getNumber());
        }

        private int getNumber()
        {
            int number = rec.getNumber();
            if (number == 0)
            {
                running = false;
                return 0;
            }
            while (!validate(number))
            {
                talk.speakAsync(repeat);
                number = rec.getNumber();
            }
            return number;
        }

        private bool validate(int n)
        {
            if (n >= 1 && n <= 4)
                return true;
            return false;
        }


        private String getStringFromInt(int i)
        {
            if (i == 1)
                return "first";
            if (i == 2)
                return "second";
            if (i == 3)
                return "third";
            if (i == 4)
                return "fourth";
            return "error";
        }

        private bool step1()
        {
            if(firstDiplayNumber == 1 || firstDiplayNumber == 2)
            {
                talk.speakAsync(standard + "in the second position" + label);
                firstPosition = 2;
                return true;
            }
            else if(firstDiplayNumber == 3)
            {
                talk.speakAsync(standard + "in the third position" + label);
                firstPosition = 3;
                return true;
            }
            else if (firstDiplayNumber == 4)
            {
                talk.speakAsync(standard + "in the fourth position" + label);
                firstPosition = 4;
                return true;
            }
            talk.speakAsync(exit);
            return false;
        }

        private bool step2()
        {
            if(secondDisplayNumber == 1)
            {
                talk.speakAsync(standard + "labeled 4" + position);
                askedForLabel = false;
                secondLabel = 4;
                return true;
            }
            else if(secondDisplayNumber == 2 || secondDisplayNumber == 4)
            {
                talk.speakAsync(standard + "in the " + getStringFromInt(firstPosition) + " position" + label);
                askedForLabel = true;
                secondPosition = firstPosition;
                return true;
            }
            else if(secondDisplayNumber == 3)
            {
                talk.speakAsync(standard + "in the first position" + label);
                askedForLabel = true;
                secondPosition = 1;
                return true;
            }
            talk.speakAsync(exit);
            return false;
        }

        private bool step3()
        {
            if(thirdDisplayNumber == 1)
            {
                talk.speakAsync(standard + "labeled " + secondLabel + position);
                askedForLabel = false;
                thirdLabel = secondLabel;
                return true;
            }
            else if(thirdDisplayNumber == 2)
            {
                talk.speakAsync(standard + "labeled " + firstLabel + position);
                askedForLabel = false;
                thirdLabel = firstLabel;
                return true;
            }
            else if(thirdDisplayNumber == 3)
            {
                talk.speakAsync(standard + "in the third position" + label);
                askedForLabel = true;
                thirdPosition = 3;
                return true;
            }
            else if(thirdDisplayNumber == 4)
            {
                talk.speakAsync(standard + "labeled 4" + position);
                askedForLabel = false;
                thirdLabel = 4;
                return true;
            }
            talk.speakAsync(exit);
            return false;
        }

        private bool step4()
        {
            if(fourthDisplayNumber == 1)
            {
                talk.speakAsync(standard + "in the " + getStringFromInt(firstPosition) + " position" + label);
                askedForLabel = true;
                fourthPosition = firstPosition;
                return true;
            }
            else if(fourthDisplayNumber == 2)
            {
                talk.speakAsync(standard + "in the first position" + label);
                askedForLabel = true;
                fourthPosition = 1;
                return true;
            }
            else if(fourthDisplayNumber == 3 || fourthDisplayNumber == 4)
            {
                talk.speakAsync(standard + "in the " + getStringFromInt(secondPosition) + " position" + label);
                askedForLabel = true;
                fourthPosition = secondPosition;
                return true;
            }

            talk.speakAsync(exit);
            return false;
        }

        private void step5(int display)
        {
            if (display == 1)
                talk.speakAsync(standard + "labeled " + firstLabel);
            else if (display == 2)
                talk.speakAsync(standard + "labeled " + secondLabel);
            else if (display == 4)
                talk.speakAsync(standard + "labeled " + thirdLabel);
            else if (display == 3)
                talk.speakAsync(standard + "labeled " + fourthLabel);
            else
                talk.speakAsync(exit);
        }
    }
}
