using IM.Client.Presenters;
using IM.Common;
using IM.Common.Controls;
using IM.Protocols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM.Client.Views
{
    public partial class LoginForm : FormBase, ILoginView
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Presenter = new LoginPresenter(this);
            this.Load += LoginForm_Load;
        }

        public event EventHandler<LoginSubmitEventArgs> LoginSubmited;
        public LoginPresenter Presenter { get; private set; }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.btnLogin.Click += btnLogin_Click;
            this.txbLoginName.Text = "chenyongbin";
            this.txbPassword.Text = "123456";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string _loginName = this.txbLoginName.Text.Trim()
                , _loginPwd = this.txbPassword.Text.Trim();

            if (string.IsNullOrEmpty(_loginName))
            {
                MessageBox.Show("请输入账号！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(_loginPwd))
            {
                MessageBox.Show("请输入密码！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.LoginSubmited(this, new LoginSubmitEventArgs
            {
                LoginName = _loginName,
                LoginPwd = _loginPwd,
                LoginTime = DateTime.Now
            });
        }

        public void HandleLoginResponse(LoginResponse response)
        {
            if (response.Status)
            {
                //登陆成功
                MessageBox.Show("登陆成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //登陆失败
                MessageBox.Show("登陆失败！\n" + response.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("登陆失败！" + response.Message);
            }
        }
    }
}
