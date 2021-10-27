﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.IO;

namespace InterviewExcercise.Reporter
{
    public class ExtentReportsHelper
    {
        public ExtentReports extent { get; set; }
        public ExtentV3HtmlReporter reporter { get; set; }
        public ExtentTest test { get; set; }
        public ExtentReportsHelper()
        {
            extent = new ExtentReports();
            reporter = new ExtentV3HtmlReporter(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "ExtentReports.html"));

            reporter.Config.DocumentTitle = "Automation Testing Report";
            reporter.Config.ReportName = "Regression Testing";
            reporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent.AttachReporter(reporter);
            extent.AddSystemInfo("Application Under Test", "nop Commerce Demo");
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("Machine", Environment.MachineName);
            extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);
        }
        public void CreateTest(string testName)
        {
            test = extent.CreateTest(testName);
        }
        public void SetStepStatusPass(string stepDescription)
        {
            Console.WriteLine(stepDescription);
            test.Log(Status.Pass, stepDescription);
        }
        public void SetStepStatusWarning(string stepDescription)
        {
            Console.WriteLine(stepDescription);
            test.Log(Status.Warning, stepDescription);
        }
        public void SetTestStatusPass()
        {
            test.Pass("Test Executed Sucessfully!");
        }
        public void SetTestStatusFail(string message = null)
        {
            var printMessage = "<p><b>Test FAILED!</b></p>";
            if (!string.IsNullOrEmpty(message))
            {
                printMessage += $"Message: <br>{message}<br>";
            }
            test.Fail(printMessage);
        }
        public void AddTestFailureScreenshot(string base64ScreenCapture)
        {
            test.AddScreenCaptureFromBase64String(base64ScreenCapture, "Screenshot on Error:");
        }
        public void SetTestStatusSkipped()
        {
            test.Skip("Test skipped!");
        }
        public void Close()
        {
            extent.Flush();
        }

        public void EndTest(TestContext testContext)
        {
            var status = testContext.Result.Outcome.Status;
            var stacktrace = testContext.Result.StackTrace;
            var errorMessage = "<pre>" + testContext.Result.Message + "</pre>";
            switch (status)
            {
                case TestStatus.Failed:
                    SetTestStatusFail($"<br>{errorMessage}<br>Stack Trace: <br>{stacktrace}<br>");
                    break;
                case TestStatus.Skipped:
                    SetTestStatusSkipped();
                    break;
                default:
                    SetTestStatusPass();
                    break;
            }

        }
    }
}
