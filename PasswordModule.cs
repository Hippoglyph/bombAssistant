using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class PasswordModule
    {
        Speaker talk;
        Listener rec;
        String[] input;
        Assistant assistant;
        int sequenceNumber;
        bool running;
        List<string> passwordList;
        public PasswordModule(Speaker talk, Listener rec, String[] input, Assistant lord)
        {
            this.talk = talk;
            this.rec = rec;
            this.input = input;
            assistant = lord;
            sequenceNumber = 1;
            running = true;
            initList();
            solve();
        }

        private void solve()
        {
            while (running)
            {
                if (play(input))
                    return;
                sequenceNumber++;
                talk.speakAsync("Say sequence " + sequenceNumber + "!");
                input = rec.getLetterSequence();
                while (input.Contains(Listener.UNRECOGNIZED))
                {
                    talk.speakAsync("Repeat!");
                    input = rec.getLetterSequence();
                }
                if (input.Contains(Listener.EXIT))
                {
                    talk.speakAsync("Sequence contained EXIT. Ready for new module!");
                    return;
                }
            }
        }

        private void done()
        {
            running = false;
            if(passwordList.Count == 1)
                talk.speakAsync("The password is: " + passwordList.First());
            else
                talk.speakAsync("Impossible password. Please repeat module!");
        }

        private bool play(string[] militaryLetter)
        {
            removePasswords(militaryLetter);
            if(passwordList.Count <= 1 || sequenceNumber > 4)
            {
                done();
                return true;
            }
            return false;
        }

        private void removePasswords(string[] militaryLetter)
        {
            List<char> letters = new List<char>();
            foreach(string MLetter in militaryLetter)
            {
                String letter = assistant.getLetter(MLetter);
                if (!letter.Equals(Listener.ERROR))
                    letters.Add(letter[0]);
            }
            string[] passwordList_ = passwordList.ToArray();
            foreach(string password in passwordList_)
            {
                if (!validate(password, letters))
                    passwordList.Remove(password);
            }
        }

        private bool validate(string password, List<char> letters)
        {
            char passwordletter = password[sequenceNumber - 1];
            if (letters.Contains(passwordletter))
                return true;
            return false;
        }

        private void initList()
        {
            passwordList = new List<string>();
            passwordList.Add("about");
            passwordList.Add("every");
            passwordList.Add("large");
            passwordList.Add("plant");
            passwordList.Add("spell");
            passwordList.Add("these");
            passwordList.Add("where");
            passwordList.Add("after");
            passwordList.Add("first");
            passwordList.Add("learn");
            passwordList.Add("point");
            passwordList.Add("still");
            passwordList.Add("thing");
            passwordList.Add("which");
            passwordList.Add("again");
            passwordList.Add("found");
            passwordList.Add("never");
            passwordList.Add("right");
            passwordList.Add("study");
            passwordList.Add("think");
            passwordList.Add("world");
            passwordList.Add("below");
            passwordList.Add("great");
            passwordList.Add("other");
            passwordList.Add("small");
            passwordList.Add("their");
            passwordList.Add("three");
            passwordList.Add("would");
            passwordList.Add("could");
            passwordList.Add("house");
            passwordList.Add("place");
            passwordList.Add("sound");
            passwordList.Add("there");
            passwordList.Add("water");
            passwordList.Add("write");
        }
    }
}
