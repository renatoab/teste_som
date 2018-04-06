using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace Teste_som
{
    /// <summary>
    /// Implementation of 
    /// </summary>
    public class Noise
    {
        public int[] prob;
        private static bool mute;
        private static bool end_sound;
        private static int base_freq;
        private static int current_grade;
        private static int[] freqSequency;
        private static Random random;
        private static int compass;
        private static int currentChord;
        private static int lastChord;
        private static string seqNotes;
        public static string strCurrentChord;
        private static string strCurrentNote;
        public static bool currentChordChanged;

        public Noise()
        {
            End_sound = false;
            Mute = false;
            
            //int[] freq;
            //int freq = new int[7];
            //Base_freq = int.Parse(Form1.textBox_Hz.Text);
        }

        public static void MountSequence()
        {
            MountSequence(0);
        }


        /// <summary>
        /// Create a sequence of frequencies, based on the Options
        /// </summary>
        public static void MountSequence(int randSeed)
        {
            freqSequency = new int[Options.beat_number];
            if (randSeed == 0)
            {
                random = new Random();
            }
            else
            {
                random = new Random(randSeed);
            }

            //int protec = 10;
            int curr_freq = 0;
            int curr_beat = 0;
            int it;
            int countBeats = 0;
            it = 0;
            while (countBeats < Options.beat_number)
            {
                compass = (countBeats / 4); // numero do compasso, divisao inteira

                currentChord = Options.gradeSeq[compass % Options.nChords]; // acorde atual       

                getNoteName(freq_note(currentChord), ref strCurrentChord);

                if (Options.forceTone && (currentChord != lastChord || countBeats == 0))
                {
                    curr_beat = 1;
                    curr_freq = freq_note(currentChord);
                }
                else
                {
                    curr_beat = 0;
                    if (current_grade > 0)
                    {
                        curr_beat = 1 - newPauseAfterNote(current_grade);
                    }
                    if (curr_beat == 0)
                    {
                        curr_beat = newbeat(it);
                    }
                    if (curr_beat > 0)
                    {
                        curr_freq = newfreq(curr_freq, it);                        
                    }
                    else
                    {
                        curr_freq = 0;
                    }
                }
                freqSequency[countBeats] = curr_freq;

                countBeats++;
                it = ++it % 4;
                lastChord = currentChord;
            }
        }

        public static void GetSequence()
        {
            try
            {
                Files.LoadSequenceFile();
                //freqSequency = new int[];
                freqSequency = new int[Options.beat_number];
                Files.ReadList(freqSequency);
            }
            catch
            {

            }
        }

        public static void PlaySequence()
        {
            int param = 1;

            int countBeats = 0;

            // tentativa fracassada
            //Task taskPlay1 = new Task(() => Noise.playmp3(freqSequency[countBeats]));
            //Task taskPlay2 = new Task(() => Noise.playmp3(freqSequency[countBeats]));
            //Task taskPlay3 = new Task(() => Noise.playmp3(freqSequency[countBeats]));
            //Task taskPlay4 = new Task(() => Noise.playmp3(freqSequency[countBeats]));

            while (countBeats < Options.beat_number && !End_sound)
            {
                //it = 0;
                //while (it++ <= 3)
                //{
                compass = (countBeats / 4); // numero do compasso, divisao inteira
                int auxChord = Options.gradeSeq[compass % Options.nChords]; // acorde atual  
                if (auxChord != currentChord)
                {
                    currentChordChanged = true;
                    currentChord = auxChord;
                }

                if (Options.repeatSeq)
                {
                    if (freqSequency[countBeats % (4 * Options.nChords)] > 0)
                        playmp3(freqSequency[countBeats % (4 * Options.nChords)]);
                }
                else if (freqSequency[countBeats] > 0)
                {
                    playmp3(freqSequency[countBeats]);
                    //switch (countBeats % 4)      // tentativa fracassada
                    //{
                    //    case 0 : taskPlay1.Start(); break;
                    //    case 1: taskPlay2.Start(); break;
                    //    case 2: taskPlay3.Start(); break;
                    //    default:
                    //        taskPlay4.Start(); break;
                    //}      
                }

                System.Threading.Thread.Sleep((int)(1000 * Options.beat_t));
                //Console.Beep(curr_freq, (int)(1000*Form1.beat_t));
                countBeats++;
                //}
            }
        }

        public static void Start()
        {
            MountSequence();
            Files.WriteList(freqSequency);
            PlaySequence();
        }

        public static void Start(int randSeed)
        {
            MountSequence(randSeed);
            PlaySequence();
        }

        public static void StartLoadedSequence()
        {
            PlaySequence();
        }

        public static void Start_old()
        {
            //int protec = 10;
            int curr_freq = 0;
            int curr_beat = 0;
            int it;
            int countBeats = 0;
            while (countBeats <= Options.beat_number && !End_sound)
            {
                it = 0;
                while (it++ <= 3)
                {
                    countBeats++;
                    curr_beat = newbeat(it - 1);               
                    if (curr_beat > 0)
                    {
                        curr_freq = newfreq(curr_freq);
                        playmp3(curr_freq);                        
                    }
                    System.Threading.Thread.Sleep((int)(1000 * Options.beat_t));
                    //Console.Beep(curr_freq, (int)(1000*Form1.beat_t));
                }
            }                      
            
        }

        public static int newfreq(int old_freq)
        {
            return newfreq(old_freq, 0);
        }

        /// <summary>
        /// Returns the new fequency based on probabilities
        /// </summary>
        /// <param name="old_freq"></param>
        /// <returns></returns>
        public static int newfreq(int old_freq, int irand)
        {            
            double randomNote = random.Next(0, 10000);
            randomNote = randomNote / 10000;
            double randomPlus = random.Next(0, 10000);
            randomPlus = randomPlus / 10000;
            
            double prob_ac = 0; // accumulated probability
            int res = 0;
                        
            if (Options.prob_up > 0)
            {
                prob_ac = prob_ac + Options.prob_up;
                if (randomPlus <= prob_ac)
                {
                    current_grade = (current_grade % 7) + 1;
                    while (current_grade < 1) current_grade = current_grade + 7;
                    res = freq_note(current_grade);
                    return res;
                }
            }
            if (Options.prob_0 > 0)
            {
                prob_ac = prob_ac + Options.prob_0;
                if (randomPlus <= prob_ac)
                {
                    res = freq_note(current_grade);
                    return res;
                }
            }
            if (Options.prob_down > 0)
            {
                prob_ac = prob_ac + Options.prob_down;
                if (randomPlus <= prob_ac)
                {
                    current_grade = (current_grade-2) % 7 + 1;
                    while (current_grade < 1) current_grade = current_grade + 7;
                    res = freq_note(current_grade);
                    return res;
                }
            }

            prob_ac = 0;
            for (int i = 0; i <= 6; i++)
            {
                prob_ac = prob_ac + Options.prob_value[(i-currentChord+1+7) % 7];
                if (randomNote <= prob_ac)
                {
                    current_grade = i + 1;
                    res = freq_note(current_grade);
                    return res;
                    //i = 7;
                }
            }
            return freq_note(0);
        }

        /// <summary>
        /// Tell if there will be a bit, based on beat probability
        /// </summary>
        /// <param name="btime"></param>
        /// <returns></returns>
        public static int newbeat(int btime)
        {
            //Random random = new Random();
            double randomNumber = random.Next(0, 10000);
            randomNumber = randomNumber / 10000;
            int res = 0;
            if (randomNumber <= Options.prob_t[btime])
            {
                res = 1;
            }
            return res;
        }

        public static int newPauseAfterNote(int grade)
        {
            //Random random = new Random();
            double randomNumber = random.Next(0, 10000);
            randomNumber = randomNumber / 10000;
            int res = 0;
            //if (randomNumber <= Options.prob_after[grade-1])
            if (randomNumber <= Options.prob_after[(grade - currentChord + 7) % 7])
            {
                res = 1;
            }
            return res;
        }

        public static void getNoteName(double freq, ref string noteName)
        {
            if (quase(freq, 262.0)) noteName = "c";
            else if (quase(freq, 277.0)) noteName = "c#";
            else if (quase(freq, 294.0)) noteName = "d";
            else if (quase(freq, 311.0)) noteName = "d#";
            else if (quase(freq, 330.0)) noteName = "e";
            else if (quase(freq, 349.0)) noteName = "f";
            else if (quase(freq, 370.0)) noteName = "f#";
            else if (quase(freq, 392.0)) noteName = "g";
            else if (quase(freq, 415.0)) noteName = "g#";
            else if (quase(freq, 440.0)) noteName = "a";
            else if (quase(freq, 466.0)) noteName = "a#";
            else if (quase(freq, 493.0)) noteName = "b";
            else if (quase(freq, 523.2)) noteName = "c";
            else if (quase(freq, 554.4)) noteName = "c#";
            else
            {
                noteName = "- ";
            }
        }

        public static void playmp3(double freq)
        {
            string filename =  String.Empty;
            bool toca = true;
            while (freq < 262)
                freq = freq * 2;
            while (freq > 554.4)
                freq = freq / 2;

            getNoteName(freq, ref filename);

            if (filename != "- ")
            {

            }
            else
            {
                toca = false;
                strCurrentNote = "- ";
            }                     

            if (toca)
            {
                strCurrentNote = filename + " ";
                filename = "..\\..\\sons\\uke\\" + filename + ".wav";
                SoundPlayer simpleSound = new SoundPlayer(filename);
                simpleSound.Play();
            }
            seqNotes = seqNotes + strCurrentNote;
        }

        public static bool quase(double a, double b)
        {
            if (System.Math.Abs(a - b) <= 5.0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int freq_note(int grade)
        {
            double frac = 0.0;
            if (grade == 1)
                frac = 1;
            else if (grade == 2)
                frac = 9.0 / 8.0;
            else if (grade == 3)
                frac = 5.0 / 4.0;
            else if (grade == 4)
                frac = 4.0 / 3.0;
            else if (grade == 5)
                frac = 3.0 / 2.0;
            else if (grade == 6)
                frac = 5.0 / 3.0;    ////////////////////// ?????????
            else if (grade == 7)
                frac = 1.875; /// ///////////////////// ???????

            return (int)(Options.base_freq * frac);
        }

        public static void Stop()
        {
            End_sound = true;           
        }

        public static int Base_freq
        {
           get { return base_freq; }
           set { base_freq = value; }
        }

        public static bool Mute
        {
           get { return mute; }
           set { mute = value; }
        }
        public static bool End_sound
        {
           get { return end_sound; }
           set { end_sound = value; }
        }

    }
}
