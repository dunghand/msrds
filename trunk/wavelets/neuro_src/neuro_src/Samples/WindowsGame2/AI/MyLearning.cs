﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gameAI.NerualNetwork;

namespace WindowsGame2.AI
{

    // DISTANCE,ES_ATTACK,ES_STAND,ES_NEAR,ES_FAR,MS_ATTACK,MS_STAND,MS_NEAR,MS_FAR,OUTPUT


    public class InputData
    {
        public double[] _Input;
        public InputData()
        {
            _Input = new double[(int)INPUT.MAX];

        }

        public double this[INPUT input]
        {
            get
            {
                return _Input[(int)input];
            }
            set
            {
                if (1 < value)
                    value = 1;
                _Input[(int)input] = value;
            }
        }

        public double this[int input]
        {
            get
            {
                return _Input[(int)input];
            }
            set
            {
                if (1 < value)
                    value = 1;
                _Input[(int)input] = value;
            }
        }


    }

    public enum INPUT : int
    {
        DISTANCE = 0,
        
        ES_STAND,
        ES_NEAR,
        ES_FAR,
        ES_ATTACK,
        ES_DODGE,
        
        MS_STAND,
        MS_NEAR,
        MS_FAR,
        MS_ATTACK,
        MS_DODGE,

        MS_STAND2,
        MS_NEAR2,
        MS_FAR2,
        MS_ATTACK2,
        MS_DODGE2,

        MS_STAND3,
        MS_NEAR3,
        MS_FAR3,
        MS_ATTACK3,
        MS_DODGE3,


        MAX
    }

    public enum OUTPUT
    {
        STAND,
        NEAR,
        FAR,
        ATTACK,
        DODGE,
        MAX

    }

    public class MyLearning
    {

        static public MyLearning I;

        AI.Form1 _form = new WindowsGame2.AI.Form1();

        public MyLearning()
        {

            I = this;

            _form.Show();


            _Learning = new KLearning((int)INPUT.MAX, 100, 100);

        }



        KLearning _Learning;

        public void AddData(InputData input,OUTPUT output )
        {
            StringBuilder s = new StringBuilder();
            s.Append("[");

            int c = 0;
            foreach (var item in input._Input)
            {
                
                switch ((INPUT)c)
                {
                    case INPUT.ES_STAND:
                    case INPUT.MS_STAND:
                    case INPUT.MS_STAND2:
                    case INPUT.MS_STAND3:
                        s.Append("[");
                        s.Append(item.ToString());
                        break;
                    case INPUT.ES_DODGE:
                    case INPUT.MS_DODGE:
                    case INPUT.MS_DODGE2:
                    case INPUT.MS_DODGE3:
                        s.Append(item.ToString());
                        s.Append("]");

                        break;
                    default:
                        s.Append(item.ToString());
                        break;

                }
                c++;
                
            }

            s.Append("] [");

            s.Append(output.ToString());
            
            

            s.Append("]");


            _form.listBox1.Items.Insert(0,s.ToString());


            KLearnData data = new KLearnData();
            data._Input = input._Input;
            data._Symbol = output.ToString();

            _Learning.Run(data);
        }

        public OUTPUT GetNextAction(InputData input)
        {
            string strOutPut = _Learning.Estimate(input._Input);
            if (strOutPut == null)
                return OUTPUT.STAND;
            OUTPUT output = (OUTPUT)Enum.Parse(typeof(OUTPUT), strOutPut);

            return output;


        }

    }
}
