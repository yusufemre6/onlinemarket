using Npgsql;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineMarket
{
    public partial class MüşteriGiriş : Form
    {
        NpgsqlConnection npgsqlConnection = new NpgsqlConnection("server=localHost; port=5432; Database=OnlineMarket1; user ID=postgres; password=123456");
        
        int kisiId =0;

        NpgsqlCommand npgsqlCommand;

        public MüşteriGiriş()
        {
            InitializeComponent();
        }

        private void MüşteriGiriş_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string komut = "Select * from \"Kisi\" where \"Kisi\".\"KisiEMail\"=\'"+textBox1.Text.Trim()+ "\' and \"Kisi\".\"KisiŞifre\"=\'"+textBox2.Text.Trim()+ "\' and \"Kisi\".\"KisiTürü\"=1";
            NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(komut,npgsqlConnection);
            DataTable dt = new DataTable();
            npgsqlDataAdapter.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                kisiId = Convert.ToInt32(dt.Rows[0][0]);
                komut = "insert into \"tblGonderme\" (veri) values (" + kisiId + ")";
                npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
                npgsqlConnection.Open();
                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();
                MüşteriAnaSayfa müşteriAnaSayfa = new MüşteriAnaSayfa();
                müşteriAnaSayfa.ShowDialog();
                this.Hide();
            }
            else
                MessageBox.Show("Hatalı bir deneme yaptınız");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MusteriKayıtSayfa musteriKayıtSayfa = new MusteriKayıtSayfa();
            musteriKayıtSayfa.ShowDialog();
            this.Hide();
        }
    }
}
