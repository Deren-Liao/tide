using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

namespace GoogleCloudAspNet
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox2.Text = "Press Write Button";
        }

        private void HelloWorld()
        {
            // This does nothing.
        }
         
        protected void Button1_Click(object sender, EventArgs e)
        {
            var result = StackdriverLogging.StackdriverLog.WriteLogV2(TextBox1.Text, new Guid("3D52EA2B-DDD7-4CA6-9741-22963A482E74"));
            TextBox2.Text = $"Written{Environment.NewLine}" + result;
        }

        //protected void Button1_Click_2(object sender, EventArgs e)
        //{
        //    var result = StackdriverLogging.StackdriverLog.WriteLog(TextBox1.Text, new Guid("3D52EA2B-DDD7-4CA6-9741-22963A482E74"));
        //    TextBox2.Text = $"Written{Environment.NewLine}" + result;
        //}

        protected void Button2_Click(object sender, EventArgs e)
        {
            //foreach (string logMessage in Program.Main(1))
            //{
            //    TextBox2.Text += Environment.NewLine + logMessage;
            //}

            // Retrieve a logger for this context.
            ILog log = LogManager.GetLogger(typeof(WebForm1));

            // Log some information to Google Stackdriver Logging.
            log.Info("Happy Valentine's Day.");

            TextBox2.Text += "Do you see the log";
        }
    }
}