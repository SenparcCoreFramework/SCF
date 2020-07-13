using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Linq;
using System.Threading;

namespace Senparc.Web.FirefoxDriverTest
{
    /// <summary>
    /// ui�Զ������Ի���
    /// </summary>
    [TestClass]
    public class FireFoxBaseDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from https://github.com/mozilla/geckodriver/releases
        // to install Firefox WebDriver.

        protected RemoteWebDriver _driver;

        /// <summary>
        /// ��ͼ·��
        /// </summary>
        private string screenShotPath;//"F:\\auto-test\\scf";

        /// <summary>
        /// ����
        /// </summary>
        private int step = 0;

        /// <summary>
        /// SCF��Ŀ��ַ
        /// </summary>
        const string webSite = "https://localhost:44311/Admin/Login";

        [TestInitialize]
        public virtual void EdgeDriverInitialize()
        {
            // Initialize firefox driver 
            System.Text.CodePagesEncodingProvider.Instance.GetEncoding(437);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//ע�����
            var options = new FirefoxOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true//����ssl����
            };
            screenShotPath = System.IO.Path.Combine(Environment.CurrentDirectory, "screenShot\\", DateTime.Now.ToString("yyyy��MM��dd��HHʱmm��"));
            if (!System.IO.Directory.Exists(screenShotPath))
            {
                System.IO.Directory.CreateDirectory(screenShotPath);
            }
            _driver = new FirefoxDriver(options);
            loadPage();
        }

        /// <summary>
        /// ��ʼ��ҳ��
        /// </summary>
        private void loadPage() => _driver.Url = webSite;

        /// <summary>
        /// ����
        /// </summary>
        [TestMethod]
        public void VerifyPageTitle()
        {
            captureScreenShot("��¼ҳ��");
            Assert.AreEqual("��¼ SCF �����̨", _driver.Title);
        }

        /// <summary>
        /// ��֤��¼�߼�
        /// </summary>
        [TestMethod]
        public void VerifyLoginModule()
        {
            _verifyLogin();
        }

        /// <summary>
        /// ��֤��¼�߼�
        /// </summary>
        public void _verifyLogin()
        {
            string userName = "SenparcCoreAdmin36";//��¼��
            string pwd = "123456";//��½����
            IWebElement loginInput = _driver.FindElementByCssSelector("div.el-form-item:nth-child(1) > div:nth-child(1) > div:nth-child(1) > input:nth-child(1)");//�û��������
            IWebElement pwdInput = _driver.FindElementByCssSelector("div.el-form-item:nth-child(2) > div:nth-child(1) > div:nth-child(1) > input:nth-child(1)");//���������
            IWebElement loginBtn = _driver.FindElementByCssSelector(".el-button");//��¼��ť
            loginInput.SendKeys(userName);
            pwdInput.SendKeys(pwd);
            captureScreenShot();
            loginBtn.Click();//��¼��ťclick�¼�
            captureScreenShot();
            Assert.AreEqual("����Ա��̨��ҳ", _driver.Title);
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }

        /// <summary>
        /// ��ͼ������
        /// </summary>
        /// <param name="stepName"></param>
        protected void captureScreenShot(string stepName = "")
        {
            Screenshot screenShot = _driver.GetScreenshot();
            Interlocked.Increment(ref step);
            screenShot.SaveAsFile(string.Concat(screenShotPath, "\\step_", step, '_', stepName, ".png"), ScreenshotImageFormat.Png);
        }
    }
}
