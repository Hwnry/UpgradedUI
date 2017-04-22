using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DesignLibrary_Tutorial.Fragments
{
    public class Fragment2 : SupportFragment
    {
        private string AccessToken;
        private TextInputLayout passwordWrapper;
        private EditText etEmail;
        private Button btnLogin;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Fragment2, container, false);

            btnLogin = view.FindViewById<Button>(Resource.Id.btnLogin);
            passwordWrapper = view.FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutPassword);
            
            etEmail = (EditText)view.FindViewById(Resource.Id.etEmail);

            btnLogin.Click += BtnLogin_Click;

            return view;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
           
        }

        //public async override void OnResume()
        //{
        //    base.OnResume();
           

        //}

        private async void BtnLogin_Click(object sender, System.EventArgs e)
        {
            string txtPassword = passwordWrapper.EditText.Text;
            //   passwordWrapper.Error = txtPassword
            string url = "http://172.27.104.244/partyup/" + "token";

            string data = "grant_type=password&username=" + etEmail.Text + "&password=" + txtPassword;


            string response = await MakePostRequest(url, data, false);

            dynamic jsonData = JsonConvert.DeserializeObject(response);

            AccessToken = jsonData.access_token;

            passwordWrapper.Error = AccessToken;
        }

        public async Task<string> MakePostRequest(string url, string serializedDataString, bool isJson)
        {
            //simple request function 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (isJson)
                request.ContentType = "application/json";
            else
                request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "POST";
            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(serializedDataString);
                writer.Flush();
                writer.Dispose();
            }

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();

            using (StreamReader sr = new StreamReader(respStream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}