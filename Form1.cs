using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Net.Http;



namespace SecondTryTest
{
    public partial class Form1 : Form
    {
        // Atributos
        NetworkService networkService;

        ApiService apiService;

        // Métodos / Construtores

        private DialogService dialogService;

        private List<Activities> activity;

        private DataService dataService;

        public Form1()
        {
            InitializeComponent();
            networkService = new NetworkService();
            apiService = new ApiService();
            List<Activities> activity = new List<Activities>();
            dialogService = new DialogService();
            dataService = new DataService();
            LoadActivity();

        }

        private async void LoadActivity()
        {
            bool load;

            var connection = networkService.CheckConnection();
            if (!connection.IsSuccess)
            {
                LoadLocalActivity();
                load = false;
            }
            else
            {
                await LoadApiActivity();
                load = true;
            }

            if (activity.Count == 0)
            {
                progressBar1.Value = 100;
                label1.Text = "Erro na API a devolver dados";
                return;
            }


           
            listBox1.DataSource = activity.Select(a => $"{a.Key}--{a.Activity}--{a.Type}--{a.Participants}").ToList();
            listBox1.DisplayMember = "Activity";

            comboBox1.DataSource = null;

            comboBox1.Items.Add("busywork");
            comboBox1.Items.Add("charity");
            comboBox1.Items.Add("cooking");
            comboBox1.Items.Add("diy");
            comboBox1.Items.Add("education");
            comboBox1.Items.Add("music");
            comboBox1.Items.Add("recreational");
            comboBox1.Items.Add("relaxation");
            comboBox1.Items.Add("social");
            
            




        }

        private void LoadLocalActivity()
        {
            DataService dataService = new DataService();

            // Pupular a lista localRates com os dados da base de dados
            List<Activities> localActivity = new List<Activities>();


            // Call GetData w
            dataService.GetData<Activities>(localActivity);
        }

        /* private async Task LoadApiActivity()
         {
             progressBar1.Value = 0;

             label1.Text = "A carregar dados da API...";

             var response = await apiService.GetActivity("https://bored-api.appbrewery.com", "/random");

             activity = new List<Activities> { (Activities)response.Result };


             dataService.SaveData(activity);
             progressBar1.Value = 100;
             label1.Text = "Dados carregados da API";


         }*/

        private async Task LoadApiActivity()
        {
            progressBar1.Value = 0;
            label1.Text = "A carregar dados da API...";

            activity = new List<Activities>();

            for (int i = 0; i < 5; i++) // Faz 5 chamadas à API
            {
                var response = await apiService.GetActivity("https://bored-api.appbrewery.com", "/random");
                activity.Add((Activities)response.Result);
            }

            dataService.SaveData(activity); // Salvar
            progressBar1.Value = 100;
            label1.Text = "Dados carregados da API";


        }

       
   

           
        private async void button2_Click(object sender, EventArgs e)
        {
            

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleciona uma atividade.");
                return;
            }

            string selectedType = comboBox1.SelectedItem.ToString();
            List<Activities> activities = await GetActivitiesAsync(selectedType);

          
            listBox2.DataSource = activities.Select(a => $"{a.Key}--{a.Activity}--{a.Type}--{a.Participants}").ToList();
            listBox2.DisplayMember = "Activity";


        }
        private async Task<List<Activities>> GetActivitiesAsync(string selectedType)
        {
            List<Activities> activities = new List<Activities>();
            string apiUrl = $"https://bored-api.appbrewery.com/filter?type={selectedType}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                       
                        activities = JsonConvert.DeserializeObject<List<Activities>>(jsonResponse);
                    }
                    else
                    {
                        MessageBox.Show($"API Error: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request Error: {e.Message}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"General Error: {e.Message}");
            }

            return activities;
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Tem de escolher uma actividade");
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\busywork.png");
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\charity.png");
            }
            else if (comboBox1.SelectedIndex == 2)
            {
               pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\cooking.png");
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\diy.png");
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\education.png");
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\music.png");
            }
            else if (comboBox1.SelectedIndex == 6)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\recreation.png");
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\relaxation.png");
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                pictureBox1.BackgroundImage = Image.FromFile("C:\\Users\\ROG\\OneDrive\\Documentos\\CINEL-CET\\5413-Programacao_Orientada_OBJ\\SecondTryTest\\SecondTryTest\\img\\social.png");
            }
        }
    }
}
