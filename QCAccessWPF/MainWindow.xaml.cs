using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDAPIOLELib;

namespace QCAccessWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string qcUrl; //declaring member variable

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                TDConnection tdConn = new TDConnection();
                tdConn.InitConnectionEx(qcUrl.Text);
                tdConn.ConnectProjectEx(qcDomain.Text, qcProject.Text, qcLogin.Text, qcPassword.Password);

                //MessageBox.Show((string)qcUrl.Text);
                //MessageBox.Show((string)qcDomain.Text);
                //MessageBox.Show((string)qcProject.Text);
                //MessageBox.Show((string)qcLogin.Text);
                //MessageBox.Show((string)qcPassword.Password);
                //MessageBox.Show((string)qcDomain.Text);
                //MessageBox.Show((string)testFolder.Text);
                //MessageBox.Show((string)testSet.Text);
                //RunFactory runFactory = (RunFactory)test.RunFactory;//

                //string testFolder = "^" + @"Root\MULTUM\Monthly Testing\SDK v4\";
                //string testSet = "BNF SDKv4 Server and Update";

                TestSetFactory tsFactory = (TestSetFactory)tdConn.TestSetFactory;
                TestSetTreeManager tsTreeMgr = (TestSetTreeManager)tdConn.TestSetTreeManager;
                TestSetFolder tsFolder = (TestSetFolder)tsTreeMgr.get_NodeByPath(testFolder.Text);


                TDAPIOLELib.List tsList = (TDAPIOLELib.List)tsFolder.FindTestSets((string)testSet.Text, false, null);
                //TDAPIOLELib.List tsTestList = tsFactory.NewList("");

                //List tsList = tsFolder.FindTestSets(testSet, false, "status=No Run");
                //Feature\Multum\Black Box\Monthly Testing\SDK v4

                foreach (TestSet testset in tsList)
                {
                    Console.WriteLine("Test Set Folder Path: {0}", testFolder);
                    Console.WriteLine("Test Set:  {0}", testset.Name);

                    TestSetFolder tsfolder = (TestSetFolder)testset.TestSetFolder;
                    //string testFolder = "^" + @"Root\MULTUM\Monthly Testing\SDK v4\";
                    TSTestFactory tsTestFactory = (TSTestFactory)testset.TSTestFactory;
                    TDAPIOLELib.List tsTestList = tsTestFactory.NewList("");
                    //}

                    foreach (TSTest tsTest in tsTestList)//no such interface supported
                    {

                        Console.WriteLine("Test Set:  {0}", tsTest.Name);
                        //Console.ReadLine();

                        string status = (string)tsTest.Status;
                        Console.WriteLine("STATUS {0}", status);
                        //Console.WriteLine("PARAMS {0}",tsTest.Params);

                        Console.WriteLine("PARAMS {0}", tsTest.History);


                        TDAPIOLELib.Run lastRun = (TDAPIOLELib.Run)tsTest.LastRun;
 
                        // don't update test if it may have been modified by someone else
                        if (lastRun == null)
                        {
                            RunFactory runFactory = (RunFactory)tsTest.RunFactory;

                            TDAPIOLELib.List runs = runFactory.NewList("");
                            Console.WriteLine("test runs:  {0}", runs.Count);
                            String date = DateTime.Now.ToString("MM-dd_hh-mm-ss");
                            TDAPIOLELib.Run run = (TDAPIOLELib.Run)runFactory.AddItem("Run_" + date); //Run_5-23_14-49-52                        

                            var oRunInstance = (RunFactory)tsTest.RunFactory;
                            //var oRun = (Run)oRunInstance.AddItem("Performance Test");

                            //run.Status = "Passed";
                            //run.Post();
                            //run.Refresh();


                            var oTest = (Test)tsTest.Test;
                            var tsDesignStepList = oTest.DesignStepFactory.NewList("");
                            var oStepFactory = (StepFactory)run.StepFactory;
                            foreach (DesignStep oDesignStep in tsDesignStepList)
                            {
                                var oStep = (Step)oStepFactory.AddItem(oDesignStep.StepName);

                                //oStep.Status = "Passed";
                                //oStep.Post();
                            }
                        }

                    }
                }

                tdConn.DisconnectProject();
                tdConn.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }

        }
    }
}
