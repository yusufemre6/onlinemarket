using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OnlineMarket
{
    public partial class MusteriKayıtSayfa : Form
    {
        NpgsqlConnection npgsqlConnection = new NpgsqlConnection("server=localHost; port=5432; Database=OnlineMarket1; user ID=postgres; password=123456");
        public MusteriKayıtSayfa()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool islem = false;

            npgsqlConnection.Open();
            string komut = "call musteri_kayit(@ad,@soyad,@telno,@adres,@email,@dt,@cinsiyet,@sifre,@kartno,@kartskt,@kartcvv)";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@ad", textBox1.Text);
            npgsqlCommand.Parameters.AddWithValue("@soyad", textBox2.Text);
            npgsqlCommand.Parameters.AddWithValue("@telno", maskedTextBox5.Text);
            npgsqlCommand.Parameters.AddWithValue("@email", textBox3.Text);
            npgsqlCommand.Parameters.AddWithValue("@dt", textBox5.Text);
            npgsqlCommand.Parameters.AddWithValue("@sifre", textBox6.Text);
            npgsqlCommand.Parameters.AddWithValue("@cinsiyet", Convert.ToInt32(comboBox4.SelectedIndex)+1);
            npgsqlCommand.Parameters.AddWithValue("@kartno", textBox4.Text);
            npgsqlCommand.Parameters.AddWithValue("@kartskt", textBox8.Text);
            npgsqlCommand.Parameters.AddWithValue("@kartcvv", Convert.ToInt16(textBox9.Text));
            npgsqlCommand.Parameters.AddWithValue("@adres", Convert.ToInt32(comboBox3.SelectedIndex)+1);
            npgsqlCommand.ExecuteNonQuery();
            islem=true;
            npgsqlConnection.Close();
            MessageBox.Show("Kayıt olundu");

            if(islem==true)
            {
                npgsqlConnection.Open();
                string komut11 = "insert into \"Siparisler\"  (\"SiparisMüsteriId\",\"SiparisDurum\") values ((select \"KisiId\" from \"Musteri\" order by \"KisiId\" desc limit 1 ),false) ";
                npgsqlCommand = new NpgsqlCommand(komut11, npgsqlConnection);
                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MüşteriGiriş müşteriGiriş = new MüşteriGiriş();
            müşteriGiriş.ShowDialog();
            this.Hide();
        }

        private void MusteriKayıtSayfa_Load(object sender, EventArgs e)
        {

        }
    }
}
