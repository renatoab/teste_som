using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Media;
//using System.Media.SystemSound;
using System.Threading.Tasks;
using System.Timers;

namespace Teste_som
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer aTimer;

        public Form1()
        {
            InitializeComponent();

            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            aTimer.Enabled = true;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            textChordPlaying.Text = "- ";

        }

        private void updateOptions()
        {
            Noise.End_sound = false;
            Options.prob_value = new double[7];
            Options.prob_after = new double[7];
            Options.prob_t = new double[4];
            Options.base_freq = int.Parse(this.txtHz.Text);

            Options.prob_value[0] = double.Parse(this.txtProb1.Text); Options.prob_value[1] = double.Parse(this.txtProb2.Text);
            Options.prob_value[2] = double.Parse(this.txtProb3.Text); Options.prob_value[3] = double.Parse(this.txtProb4.Text);
            Options.prob_value[4] = double.Parse(this.txtProb5.Text); Options.prob_value[5] = double.Parse(this.txtProb6.Text);
            Options.prob_value[6] = double.Parse(this.txtProb7.Text);

            Options.prob_after[0] = double.Parse(this.txtProbStop1.Text); Options.prob_after[1] = double.Parse(this.txtProbStop2.Text);
            Options.prob_after[2] = double.Parse(this.txtProbStop3.Text); Options.prob_after[3] = double.Parse(this.txtProbStop4.Text);
            Options.prob_after[4] = double.Parse(this.txtProbStop5.Text); Options.prob_after[5] = double.Parse(this.txtProbStop6.Text);
            Options.prob_after[6] = double.Parse(this.txtProbStop7.Text);

            Options.prob_t[0] = double.Parse(this.probT1.Text); Options.prob_t[1] = double.Parse(this.probT2.Text);
            Options.prob_t[2] = double.Parse(this.probT3.Text); Options.prob_t[3] = double.Parse(this.probT4.Text);
            
            Options.prob_up = double.Parse(this.probUp.Text);
            Options.prob_0 = double.Parse(this.prob0.Text);
            Options.prob_down = double.Parse(this.probDown.Text);

            Options.beat_t = double.Parse(this.txtBeatTime.Text);
            Options.beat_number = int.Parse(this.txt_total.Text);

            double sum_prob = 0;
            for (int i = 0; i <= 6; i++)
            {
                sum_prob = sum_prob + Options.prob_value[i];
            }
            for (int i = 0; i <= 6; i++)
            {
                Options.prob_value[i] = Options.prob_value[i] / sum_prob;
            }
            for (int i = 0; i <= 6; i++)
            {
                Options.prob_after[i] = Options.prob_after[i] / 100;
            }
            for (int i = 0; i <= 3; i++)
            {
                Options.prob_t[i] = Options.prob_t[i] / 100;
            }
            Options.prob_up = Options.prob_up / 100;
            Options.prob_down = Options.prob_down / 100;
            Options.prob_0 = Options.prob_0 / 100;

            string gradeSeqAux = txtSeqChords.Text;
            Options.nChords = gradeSeqAux.Length;

            Options.gradeSeq = new int[Options.nChords];
            for (int i = 0; i <= gradeSeqAux.Length - 1; i++)
            {
                Options.gradeSeq[i] = int.Parse(gradeSeqAux.Substring(i,1));
            }
            Options.forceTone = chkForceTone.Checked;
            Options.repeatSeq = chkRepeatSeq.Checked;
        }


        private void button_play_Click(object sender, EventArgs e)
        {
            updateOptions();
            //RandGenerator randObj = new RandGenerator();
            //randObj.ReadList();

            int i1 = 1;
            Task taskPlay = new Task(() => Noise.Start());
            //i1 = 50;
            //Task taskPlay2 = new Task(() => Noise.Start(i1));

            taskPlay.Start();
            //taskPlay2.Start();

            //Noise.Start();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            Noise.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\Samsung\Desktop\soul_sister.wav");
            //simpleSound.Play();
            //simpleSound.Play();
        }

        private void textChordPlaying_TextChanged(object sender, EventArgs e)
        {
            //Task taskPlay = new Task(() => Noise.Start());
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (Noise.currentChordChanged)
                {
                    // TODO: pq nao funciona ???????
                    //this.textChordPlaying.Text = Noise.strCurrentChord;
                    Noise.currentChordChanged = false;
                }                    
            }
            catch
            {
                int i = 1;
            }            
        }

        private void button_load_play_Click(object sender, EventArgs e)
        {
            Noise.Stop();
            updateOptions();            
            Noise.GetSequence();
            Task taskPlay = new Task(() => Noise.StartLoadedSequence());
            taskPlay.Start();
        }

        private void button_rec_Click(object sender, EventArgs e)
        {
            // Salva o arquivo temporário num caminho escolhido
            Files.SaveSequenceFile();
        }

        private void button_replay_Click(object sender, EventArgs e)
        {
            updateOptions();
            Task taskPlay = new Task(() => Noise.StartLoadedSequence());
            taskPlay.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LyricGen.Fala.teste();
        }
    }
}
