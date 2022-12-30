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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OnlineMarket
{
    public partial class MüşteriAnaSayfa : Form
    {
        NpgsqlConnection npgsqlConnection = new NpgsqlConnection("server=localHost; port=5432; Database=OnlineMarket1; user ID=postgres; password=123456");
        int id=0;

        public MüşteriAnaSayfa()
        {
            string komut = "Select * from \"tblGonderme\"";
            string kmt = "truncate \"tblGonderme\"";
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader();
            while (npgsqlDataReader.Read())
            {
                id = Convert.ToInt32(npgsqlDataReader[0]);
            }
            npgsqlConnection.Close();
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommandd = new NpgsqlCommand(kmt, npgsqlConnection);
            NpgsqlDataReader npgsqlDataReaderr = npgsqlCommandd.ExecuteReader();
            npgsqlConnection.Close();
         
            InitializeComponent();

            //Müşteri bilgilerinin geldiği yer
            npgsqlConnection.Open();
            string komut1 = "select * from \"Kisi\" where \"Kisi\".\"KisiId\"=" + id;
            NpgsqlDataAdapter npgsqlDataAdapter1 = new NpgsqlDataAdapter(komut1, npgsqlConnection);
            DataTable dt = new DataTable();
            npgsqlDataAdapter1.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                textBox1.Text = dt.Rows[0][1].ToString();
                textBox2.Text = dt.Rows[0][2].ToString();
                maskedTextBox5.Text = dt.Rows[0][9].ToString();
                textBox3.Text = dt.Rows[0][6].ToString();
                textBox5.Text = dt.Rows[0][5].ToString();
                textBox6.Text = dt.Rows[0][7].ToString();
            }
            npgsqlConnection.Close();
            //İŞLEM BİTTİ

            npgsqlConnection.Open();
            string komut2 = "select * from \"Mahalle\" where \"MahalleId\"=(select \"AdresMahalle\" from \"Adres\" where \"AdresId\"=" + id + ")";
            NpgsqlDataAdapter npgsqlDataAdapter2 = new NpgsqlDataAdapter(komut2, npgsqlConnection);
            DataTable dt1 = new DataTable();
            npgsqlDataAdapter2.Fill(dt1);
            if (dt1.Rows.Count == 1)
            {
                comboBox3.Text = dt1.Rows[0][1].ToString();
            }
            npgsqlConnection.Close();
            npgsqlConnection.Open();

            string komut3 = "select * from \"KrediKartı\" where \"KartId\"=(select \"KisiKrediKartı\" from \"Musteri\" where \"KisiId\"=" + id+")";
            NpgsqlDataAdapter npgsqlDataAdapter3 = new NpgsqlDataAdapter(komut3, npgsqlConnection);
            DataTable dt2 = new DataTable();
            npgsqlDataAdapter3.Fill(dt2);
            if (dt2.Rows.Count == 1)
            {
                textBox4.Text = textBox9.Text = dt2.Rows[0][3].ToString();
                textBox7.Text = maskedTextBox2.Text = dt2.Rows[0][2].ToString();
                textBox8.Text = maskedTextBox3.Text = dt2.Rows[0][1].ToString();
            }
            npgsqlConnection.Close();
            npgsqlConnection.Open();

            string komut4 = "select * from \"Musteri\" where \"KisiId\"=" + id;
            NpgsqlDataAdapter npgsqlDataAdapter4 = new NpgsqlDataAdapter(komut4, npgsqlConnection);
            DataTable dt3 = new DataTable();
            npgsqlDataAdapter4.Fill(dt3);
            if (dt3.Rows.Count == 1)
            {
                label28.Text= dt3.Rows[0][1].ToString();
            }
            npgsqlConnection.Close();
            npgsqlConnection.Open();
            string komut5 = "select * from \"SiparisUrun\" where \"SiparisId\"=(select \"SiparisId\" from \"Siparisler\" where \"SiparisMüsteriId\"=" + id + " and \"SiparisDurum\"=false)";
            NpgsqlDataAdapter npgsqlDataAdapter5 = new NpgsqlDataAdapter(komut5,npgsqlConnection);
            DataTable dataTable = new DataTable();
            npgsqlDataAdapter5.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            npgsqlConnection.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MüşteriGiriş müşteriGiriş = new MüşteriGiriş();
            müşteriGiriş.ShowDialog();
            this.Hide();
        }

        private void MüşteriAnaSayfa_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            npgsqlConnection.Open();

            string komut = "call musteri_guncelle(@ad,@soyad,@telno,@email,@dt,@şifre,@id,@kartno,@kartskt,@kartcvv,@mahalle)";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@ad", textBox1.Text);
            npgsqlCommand.Parameters.AddWithValue("@soyad", textBox2.Text);
            npgsqlCommand.Parameters.AddWithValue("@telno", maskedTextBox5.Text);
            npgsqlCommand.Parameters.AddWithValue("@email", textBox3.Text);
            npgsqlCommand.Parameters.AddWithValue("@dt", textBox5.Text);
            npgsqlCommand.Parameters.AddWithValue("@şifre", textBox6.Text);
            npgsqlCommand.Parameters.AddWithValue("@id", id);
            npgsqlCommand.Parameters.AddWithValue("@kartno", textBox4.Text);
            npgsqlCommand.Parameters.AddWithValue("@kartskt", textBox7.Text);
            npgsqlCommand.Parameters.AddWithValue("@kartcvv", Convert.ToInt16(textBox8.Text));
            npgsqlCommand.Parameters.AddWithValue("@mahalle", comboBox3.SelectedItem.ToString());
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();
            MessageBox.Show("Bilgiler güncellendi");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            npgsqlConnection.Open();
            string komut = "call bakiye_yükle(@deger,@id)";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@deger", Convert.ToInt32(maskedTextBox4.Text));
            npgsqlCommand.Parameters.AddWithValue("@id", id);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();

            MessageBox.Show("Bakiye yüklendi");

            npgsqlConnection.Open();
            string komut4 = "select * from \"Musteri\" where \"KisiId\"=" + id;
            NpgsqlDataAdapter npgsqlDataAdapter4 = new NpgsqlDataAdapter(komut4, npgsqlConnection);
            DataTable dt3 = new DataTable();
            npgsqlDataAdapter4.Fill(dt3);
            if (dt3.Rows.Count == 1)
            {
                label28.Text = dt3.Rows[0][1].ToString();
            }
            npgsqlConnection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            npgsqlConnection.Open();
            string komut = "delete from \"SiparisUrun\"";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();

            npgsqlConnection.Open();
            string komut5 = "select * from \"SiparisUrun\" where \"SiparisId\"=(select \"SiparisId\" from \"Siparisler\" where \"SiparisMüsteriId\"=" + id + " and \"SiparisDurum\"=false)";
            NpgsqlDataAdapter npgsqlDataAdapter5 = new NpgsqlDataAdapter(komut5, npgsqlConnection);
            DataTable dataTable = new DataTable();
            npgsqlDataAdapter5.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            npgsqlConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            npgsqlConnection.Open();
            string komut = "delete from \"SiparisUrun\" where \"SiparisUrunId\"=@urun";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            int i = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex);
            npgsqlCommand.Parameters.AddWithValue("@urun", Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value));
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();

            npgsqlConnection.Open();
            string komut5 = "select * from \"SiparisUrun\" where \"SiparisId\"=(select \"SiparisId\" from \"Siparisler\" where \"SiparisMüsteriId\"=" + id + " and \"SiparisDurum\"=false)";
            NpgsqlDataAdapter npgsqlDataAdapter5 = new NpgsqlDataAdapter(komut5, npgsqlConnection);
            DataTable dataTable = new DataTable();
            npgsqlDataAdapter5.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            npgsqlConnection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox2.Items.Clear();
                npgsqlConnection.Open();
                string komut6 = "select * from \"paketurun3\" where \"UrunTuruAdi\"='Paket Ürün'";
                NpgsqlDataAdapter npgsqlDataAdapter6 = new NpgsqlDataAdapter(komut6, npgsqlConnection);
                DataTable dataTable1 = new DataTable();
                npgsqlDataAdapter6.Fill(dataTable1);
                dataGridView2.DataSource = dataTable1;
                string komut7 = "select \"KategoriAdi\" from \"paketurun3\"";
                NpgsqlDataAdapter npgsqlDataAdapter7 = new NpgsqlDataAdapter(komut7, npgsqlConnection);
                DataTable dataTable2 = new DataTable();
                npgsqlDataAdapter7.Fill(dataTable2);
                npgsqlConnection.Close();

                for(int i=0;i<dataTable2.Rows.Count;i++)
                {
                    comboBox2.Items.Add(dataTable2.Rows[i][0].ToString());
                }

            }
            else if(comboBox1.SelectedIndex == 1)
            {
                comboBox2.Items.Clear();
                npgsqlConnection.Open();
                string komut6 = "select * from \"acikurun1\" where \"UrunTuruAdi\"='Açık Ürün'";
                NpgsqlDataAdapter npgsqlDataAdapter6 = new NpgsqlDataAdapter(komut6, npgsqlConnection);
                DataTable dataTable1 = new DataTable();
                npgsqlDataAdapter6.Fill(dataTable1);
                dataGridView2.DataSource = dataTable1;
                string komut7 = "select \"KategoriAdi\" from \"acikurun1\"";
                NpgsqlDataAdapter npgsqlDataAdapter7 = new NpgsqlDataAdapter(komut7, npgsqlConnection);
                DataTable dataTable2 = new DataTable();
                npgsqlDataAdapter7.Fill(dataTable2);
                npgsqlConnection.Close();
                for (int i = 0; i < dataTable2.Rows.Count; i++)
                {
                    comboBox2.Items.Add(dataTable2.Rows[i][0].ToString());
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                npgsqlConnection.Open();
                string komut = "select * from \"paketurun3\" where \"UrunTuruAdi\"='Paket Ürün' and \"KategoriAdi\"='"+comboBox2.SelectedItem.ToString()+"'";
                NpgsqlDataAdapter npgsqlDataAdapter6 = new NpgsqlDataAdapter(komut, npgsqlConnection);
                DataTable dataTable1 = new DataTable();
                npgsqlDataAdapter6.Fill(dataTable1);
                npgsqlConnection.Close();
                dataGridView2.DataSource = dataTable1;
            }

            else if (comboBox1.SelectedIndex == 1)
            {
                npgsqlConnection.Open();
                string komut = "select * from \"acikurun1\" where \"UrunTuruAdi\"='Açık Ürün' and \"KategoriAdi\"='" + comboBox2.SelectedItem.ToString() + "'";
                NpgsqlDataAdapter npgsqlDataAdapter6 = new NpgsqlDataAdapter(komut, npgsqlConnection);
                DataTable dataTable1 = new DataTable();
                npgsqlDataAdapter6.Fill(dataTable1);
                npgsqlConnection.Close();
                dataGridView2.DataSource = dataTable1;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            npgsqlConnection.Open();
            string komut = "insert into \"SiparisUrun\" (\"SiparisId\",\"UrunId\",\"UrunTutari\",\"UrunMiktari\") values ((select \"SiparisId\" from \"Siparisler\" where \"SiparisMüsteriId\"="+id+ " and \"SiparisDurum\"=false),@urunid,@uruntutari,@urunmiktari)";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            int i = Convert.ToInt32(dataGridView2.CurrentCell.RowIndex);
            npgsqlCommand.Parameters.AddWithValue("@urunid", Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value));
            npgsqlCommand.Parameters.AddWithValue("@uruntutari", Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value)*Convert.ToDouble(comboBox4.SelectedItem));
            npgsqlCommand.Parameters.AddWithValue("@urunmiktari", Convert.ToDouble(comboBox4.SelectedItem));
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();

            npgsqlConnection.Open();
            string komut5 = "select * from \"SiparisUrun\" where \"SiparisId\"=(select \"SiparisId\" from \"Siparisler\" where \"SiparisMüsteriId\"=" + id + " and \"SiparisDurum\"=false)";
            NpgsqlDataAdapter npgsqlDataAdapter5 = new NpgsqlDataAdapter(komut5, npgsqlConnection);
            DataTable dataTable = new DataTable();
            npgsqlDataAdapter5.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            npgsqlConnection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NpgsqlDataAdapter npgsqlDataAdapter4;
            DataTable dt3;
            NpgsqlCommand npgsqlCommand;
            if (Convert.ToInt32(label25.Text)<= Convert.ToInt32(label28.Text))
            {
                npgsqlConnection.Open();
                string komut = "call bakiye_dusme(@id)";
                npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
                npgsqlCommand.Parameters.AddWithValue("@id", id);
                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();

                npgsqlConnection.Open();
                string komut5 = "select * from \"SiparisUrun\" where \"SiparisId\"=(select \"SiparisId\" from \"Siparisler\" where \"SiparisMüsteriId\"=" + id + "and \"SiparisDurum\"=false)";
                NpgsqlDataAdapter npgsqlDataAdapter5 = new NpgsqlDataAdapter(komut5, npgsqlConnection);
                DataTable dataTable = new DataTable();
                npgsqlDataAdapter5.Fill(dataTable);
                npgsqlConnection.Close();

                for (int i=0; i<dataTable.Rows.Count; i++)
                {
                    npgsqlConnection.Open();
                    String komut1 = "select \"UrunTuru\" from \"Urunler\" where \"UrunId\"=" + dataTable.Rows[i][2];
                    npgsqlDataAdapter4 = new NpgsqlDataAdapter(komut1, npgsqlConnection);
                    dt3 = new DataTable();
                    npgsqlDataAdapter4.Fill(dt3);
                    npgsqlConnection.Close();

                    if (Convert.ToInt32(dt3.Rows[0][0])==1)
                    {
                        npgsqlConnection.Open();
                        string komut3 = "update \"PaketUrun\" set \"UrunStokMiktari\"=\"UrunStokMiktari\"-@miktar where \"Urunıd\"=" + dataTable.Rows[i][2];
                        npgsqlCommand = new NpgsqlCommand(komut3, npgsqlConnection);
                        npgsqlCommand.Parameters.AddWithValue("@miktar", Convert.ToInt32(dataTable.Rows[i][4]));
                        npgsqlCommand.ExecuteNonQuery();
                        npgsqlConnection.Close();
                    }
                    else
                    {
                        npgsqlConnection.Open();
                        string komut3 = "update \"AcikUrun\" set \"KgStokMiktari\"=\"KgStokMiktari\"-@miktar where \"Urunıd\"=" + dataTable.Rows[i][2];
                        npgsqlCommand = new NpgsqlCommand(komut3, npgsqlConnection);
                        npgsqlCommand.Parameters.AddWithValue("@miktar", Convert.ToInt32(dataTable.Rows[i][4]));
                        npgsqlCommand.ExecuteNonQuery();
                        npgsqlConnection.Close();
                    }
                }

                npgsqlConnection.Open();
                string komut4 = "select * from \"Musteri\" where \"KisiId\"=" + id;
                npgsqlDataAdapter4 = new NpgsqlDataAdapter(komut4, npgsqlConnection);
                dt3 = new DataTable();
                npgsqlDataAdapter4.Fill(dt3);
                if (dt3.Rows.Count == 1)
                {
                    label28.Text = dt3.Rows[0][1].ToString();
                }
                npgsqlConnection.Close();

                npgsqlConnection.Open();
                string komut10 = "update \"Siparisler\" set \"SiparisDurum\"=true where \"SiparisMüsteriId\"=" + id;
                npgsqlCommand = new NpgsqlCommand(komut10, npgsqlConnection);
                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();

                npgsqlConnection.Open();
                string komut11 = "insert into \"Siparisler\"  (\"SiparisMüsteriId\",\"SiparisDurum\") values (" + id + ",false) ";
                npgsqlCommand = new NpgsqlCommand(komut11, npgsqlConnection);
                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();

                MessageBox.Show("siparis verildi");
            }
            else
                MessageBox.Show("Bakiye yetersiz");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            npgsqlConnection.Open();
            string komut = "call SiparisTutar_Hesapla(@id)";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(komut, npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@id",id);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();

            npgsqlConnection.Open();
            string komut4 = "select \"SiparisTutari\"  from \"Siparisler\" where \"SiparisMüsteriId\"="+id+ " and \"SiparisDurum\"=false";
            NpgsqlDataAdapter npgsqlDataAdapter4 = new NpgsqlDataAdapter(komut4, npgsqlConnection);
            DataTable dt3 = new DataTable();
            npgsqlDataAdapter4.Fill(dt3);
            if (dt3.Rows.Count == 1)
            {
                label25.Text = dt3.Rows[0][0].ToString();
            }
            npgsqlConnection.Close();
        }
    }
}
