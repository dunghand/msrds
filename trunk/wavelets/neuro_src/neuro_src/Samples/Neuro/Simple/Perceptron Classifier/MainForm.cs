// AForge Framework
// Perceptron Classifier
//
// Copyright ?Andrew Kirillov, 2006
// andrew.kirillov@gmail.com
//

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;

using gameAI.NerualNetwork;

namespace Classifier
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView dataList;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private AForge.Controls.Chart chart;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox learningRateBox;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Label noVisualizationLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView weightsList;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox iterationsBox;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.Label label5;
		private AForge.Controls.Chart errorChart;
		private System.Windows.Forms.CheckBox saveFilesCheck;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int			samples = 0;
		private int			variables = 0;


        KNeural _Neural = new KNeural();

		//private string[,]	data = null;

		//private int[]		classes = null;

		private double		learningRate = 0.1;
		private bool		saveStatisticsToFiles = false;

		private Thread	workerThread = null;
        private Button button1;
        private ListBox listBox1;
        private Button button2;
		private bool	needToStop = false;

		// Constructor
		public MainForm( )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent( );

			// initialize charts
			chart.AddDataSeries( "class1", Color.Red, Chart.SeriesType.Dots, 5 );
			chart.AddDataSeries( "class2", Color.Blue, Chart.SeriesType.Dots, 5 );
			chart.AddDataSeries( "classifier", Color.Gray, Chart.SeriesType.Line, 1, false );

			errorChart.AddDataSeries( "error", Color.Red, Chart.SeriesType.ConnectedDots, 3, false );

			// update some controls
			saveFilesCheck.Checked = saveStatisticsToFiles;
			UpdateSettings( );
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chart = new AForge.Controls.Chart();
            this.loadButton = new System.Windows.Forms.Button();
            this.dataList = new System.Windows.Forms.ListView();
            this.noVisualizationLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveFilesCheck = new System.Windows.Forms.CheckBox();
            this.errorChart = new AForge.Controls.Chart();
            this.label5 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.weightsList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.chart);
            this.groupBox1.Controls.Add(this.loadButton);
            this.groupBox1.Controls.Add(this.dataList);
            this.groupBox1.Controls.Add(this.noVisualizationLabel);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 420);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(91, 391);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Next";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chart
            // 
            this.chart.Location = new System.Drawing.Point(10, 215);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(170, 170);
            this.chart.TabIndex = 2;
            this.chart.Text = "chart1";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(10, 390);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "&Load";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // dataList
            // 
            this.dataList.FullRowSelect = true;
            this.dataList.GridLines = true;
            this.dataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.dataList.Location = new System.Drawing.Point(10, 20);
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(170, 190);
            this.dataList.TabIndex = 0;
            this.dataList.UseCompatibleStateImageBehavior = false;
            this.dataList.View = System.Windows.Forms.View.Details;
            // 
            // noVisualizationLabel
            // 
            this.noVisualizationLabel.Location = new System.Drawing.Point(10, 215);
            this.noVisualizationLabel.Name = "noVisualizationLabel";
            this.noVisualizationLabel.Size = new System.Drawing.Size(170, 170);
            this.noVisualizationLabel.TabIndex = 2;
            this.noVisualizationLabel.Text = "Visualization is not available.";
            this.noVisualizationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noVisualizationLabel.Visible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.saveFilesCheck);
            this.groupBox2.Controls.Add(this.errorChart);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.stopButton);
            this.groupBox2.Controls.Add(this.iterationsBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.weightsList);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.startButton);
            this.groupBox2.Controls.Add(this.learningRateBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(210, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 420);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Training";
            // 
            // saveFilesCheck
            // 
            this.saveFilesCheck.Location = new System.Drawing.Point(10, 80);
            this.saveFilesCheck.Name = "saveFilesCheck";
            this.saveFilesCheck.Size = new System.Drawing.Size(182, 16);
            this.saveFilesCheck.TabIndex = 11;
            this.saveFilesCheck.Text = "Save weights and errors to files";
            // 
            // errorChart
            // 
            this.errorChart.Location = new System.Drawing.Point(10, 270);
            this.errorChart.Name = "errorChart";
            this.errorChart.Size = new System.Drawing.Size(220, 140);
            this.errorChart.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Error\'s dynamics:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(155, 49);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(90, 50);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.ReadOnly = true;
            this.iterationsBox.Size = new System.Drawing.Size(50, 20);
            this.iterationsBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Iterations:";
            // 
            // weightsList
            // 
            this.weightsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.weightsList.FullRowSelect = true;
            this.weightsList.GridLines = true;
            this.weightsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.weightsList.Location = new System.Drawing.Point(10, 130);
            this.weightsList.Name = "weightsList";
            this.weightsList.Size = new System.Drawing.Size(220, 110);
            this.weightsList.TabIndex = 5;
            this.weightsList.UseCompatibleStateImageBehavior = false;
            this.weightsList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Weight";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 100;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Perceptron weights:";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(10, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 2);
            this.label2.TabIndex = 3;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(155, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(90, 20);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(50, 20);
            this.learningRateBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Learning rate:";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 499);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(663, 173);
            this.listBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 453);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 30);
            this.button2.TabIndex = 12;
            this.button2.Text = "Test";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(663, 672);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Perceptron Classifier";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main( ) 
		{
			Application.Run( new MainForm( ) );
		}

		// On main form closing
		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// check if worker thread is running
			if ( ( workerThread != null ) && ( workerThread.IsAlive ) )
			{
				needToStop = true;
				workerThread.Join( );
			}
		}

		// On "Load" button click - load data
		private void loadButton_Click( object sender, System.EventArgs e )
		{
			// data file format:
			// X1, X2, ... Xn, class (0|1)

            _Neural.LoadData();

					// clear current result
			ClearCurrentSolution( );

				// update list and chart
				UpdateDataListView( );

				// show chart or not
				bool showChart = ( variables == 2 );

                //if ( showChart )
                //{
                //    chart.RangeX = new DoubleRange( minX, maxX );
                //    ShowTrainingData( );
                //}

				chart.Visible = showChart;
				noVisualizationLabel.Visible = !showChart;

				// enable start button
				startButton.Enabled = true;
			
		}

		// Update settings controls
		private void UpdateSettings( )
		{
			learningRateBox.Text = learningRate.ToString( );
		}

		// Update data in list view
		private void UpdateDataListView( )
		{
			// remove all curent data and columns
			dataList.Items.Clear( );
			dataList.Columns.Clear( );

			// add columns
			for ( int i = 0, n = _Neural._Data[0]._InputList.Count ; i < n; i++ )
			{
				dataList.Columns.Add( string.Format( "X{0}", i + 1 ),
					50, HorizontalAlignment.Left );
			}
			dataList.Columns.Add( "Class", 50, HorizontalAlignment.Left );

			// add items

            for (int i = 0; i < _Neural._Data.Count;i++)
            {
                KData d = _Neural._Data[i];

                

                for (int j = 0; j < d._InputList.Count; j++)
                {
                    if (j == 0)
                    {
                        dataList.Items.Add(d._InputList[j]);
                    }
                    else
                    {
                        dataList.Items[i].SubItems.Add(d._InputList[j]);    
                    }

                    
                }

                dataList.Items[i].SubItems.Add(d._Output);    
            }

		}

        //// Show training data on chart
        //private void ShowTrainingData( )
        //{
        //    int class1Size = 0;
        //    int class2Size = 0;

        //    // calculate number of samples in each class
        //    for ( int i = 0, n = samples; i < n; i++ )
        //    {
        //        if ( classes[i] == 0 )
        //            class1Size++;
        //        else
        //            class2Size++;
        //    }

        //    // allocate classes arrays
        //    double[,] class1 = new double[class1Size, 2];
        //    double[,] class2 = new double[class2Size, 2];

        //    // fill classes arrays
        //    for ( int i = 0, c1 = 0, c2 = 0; i < samples; i++ )
        //    {
        //        if ( classes[i] == 0 )
        //        {
        //            // class 1
        //            class1[c1, 0] = data[i, 0];
        //            class1[c1, 1] = data[i, 1];
        //            c1++;
        //        }
        //        else
        //        {
        //            // class 2
        //            class2[c2, 0] = data[i, 0];
        //            class2[c2, 1] = data[i, 1];
        //            c2++;
        //        }
        //    }

        //    // updata chart control
        //    chart.UpdateDataSeries( "class1", class1 );
        //    chart.UpdateDataSeries( "class2", class2 );
        //}

		// Enable/disale controls
		private void EnableControls( bool enable )
		{
			learningRateBox.Enabled	= enable;
			loadButton.Enabled		= enable;
			startButton.Enabled		= enable;
			saveFilesCheck.Enabled	= enable;
			stopButton.Enabled		= !enable;
		}

		// Clear current solution
		private void ClearCurrentSolution( )
		{
			chart.UpdateDataSeries( "classifier", null );
			errorChart.UpdateDataSeries( "error", null );
			weightsList.Items.Clear( );
		}


		// On button "Start" - start learning procedure
		private void startButton_Click(object sender, System.EventArgs e)
		{

            //Directory.GetCurrentDirectory();


            



			// get learning rate
			try
			{
				learningRate = Math.Max( 0.00001, Math.Min( 1, double.Parse( learningRateBox.Text ) ) );
			}
			catch
			{
				learningRate = 0.1;
			}
			saveStatisticsToFiles = saveFilesCheck.Checked;

			// update settings controls
			UpdateSettings( );

			// disable all settings controls
			EnableControls( false );

			// run worker thread
			needToStop = false;
			workerThread = new Thread( new ThreadStart( SearchSolution ) );
			workerThread.Start( );
		}

		// On button "Stop" - stop learning procedure
		private void stopButton_Click(object sender, System.EventArgs e)
		{
			// stop worker thread
			needToStop = true;
			workerThread.Join( );
			workerThread = null;
		}

        KLearning _trainer;


        // Worker thread
        void SearchSolution()
        {
            
            KLearnData[] data = KNeural.GetLearnData();


            _trainer = new KLearning(data[0]._Input.Length, 100, 100);


            // input
            double[] input = new double[3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;
            
            
            
            

            // loop
            //while (!needToStop)
            foreach(KLearnData d in data)
            {
                _trainer._LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                _trainer._LearningRadius = (double)radius * (iterations - i) / iterations;

                input[0] = rand.Next(256);
                input[1] = rand.Next(256);
                input[2] = rand.Next(256);

                //trainer.RunEpoch(
                _trainer.Run(d);

                // update map once per 50 iterations
                //if ((i % 10) == 9)
                //{
                //    UpdateMap();
                //}

                // increase current iteration
//                i++;

                // set current iteration's info
                //currentIterationBox.Text = i.ToString();

                //// stop ?
                //if (i >= iterations)
                //    break;
            }


            // enable settings controls
            //EnableControls(true);
        }
        
        int iterations = 5000;
        int index = 0;
        Random rand = new Random();
        double radius = 15;

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            KLearnData[] data = KNeural.GetLearnData();

            foreach(var d in data)
            {
                string symbol = _trainer.Estimate(d._Input);
                listBox1.Items.Add(symbol);
            }

            
            
        }
	}
}
